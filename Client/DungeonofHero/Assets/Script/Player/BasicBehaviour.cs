using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
/// <summary>
/// 케릭터 행동처리 , ex) 움직임 , 점프 ..
/// </summary>
public class BasicBehaviour : NetworkBehaviour
{

    public DHStateMachine<CharacterStates.MovementStates> movementState;
    public DHStateMachine<CharacterStates.CharacterConditions> conditionState;

    protected DHPhysicsController _controller;
    public List<string> _animatorParameters { get; set; }

    public bool SendStateChangeEvents = true;
    public bool SendStateUpdateEvents = true;

    public float turnSmoothing = 0.06f;
    private Vector3 lastDirection;
    private Animator animator;
    private GenericBehaviour[] overridingBehaviours;
    private Rigidbody rBody;

    public Rigidbody GetRigidBody { get { return rBody; } }

    public Animator GetAnimator { get { return animator; } }

    private CameraMMO mainCamera;

    public CameraMMO Getcamera { get { return mainCamera; } }

    public InputManager LinkedInputManager { get; protected set; }

    void Awake() { Initialization(); }

    protected virtual void Initialization()
    {
        Debug.Log(gameObject.name);
        _controller = GetComponent<DHPhysicsController>();
        movementState = new DHStateMachine<CharacterStates.MovementStates>(gameObject, SendStateChangeEvents);
        conditionState = new DHStateMachine<CharacterStates.CharacterConditions>(gameObject, SendStateChangeEvents);
        overridingBehaviours = GetComponents<GenericBehaviour>();
        mainCamera = Camera.main.GetComponent<CameraMMO>();
        animator = GetComponent<Animator>();
        rBody = GetComponent<Rigidbody>();
        GetInputManager();

        if (animator != null)
        {
            InitializeAnimatorParameters();
        }
    }

    protected virtual void InitializeAnimatorParameters()
    {
        if (animator == null) { return; }
        _animatorParameters = new List<string>();

        DHAnimator.AddAnimatorParamaterIfExists(animator, "Idle", AnimatorControllerParameterType.Bool, _animatorParameters);
        DHAnimator.AddAnimatorParamaterIfExists(animator, "Grounded", AnimatorControllerParameterType.Bool, _animatorParameters);
        DHAnimator.AddAnimatorParamaterIfExists(animator, "VerticalVelocity", AnimatorControllerParameterType.Float, _animatorParameters);
        DHAnimator.AddAnimatorParamaterIfExists(animator, "GroundDistance", AnimatorControllerParameterType.Float, _animatorParameters);

    }

    void Update()
    {
        if (isClient)
        {
            EveryFrame();
        }

        if(isServer)
        {
            ServerFrame();
        }
    }

    [Server]
    protected virtual void ServerFrame()
    {
        foreach (GenericBehaviour behaviour in overridingBehaviours)
        {
            if (behaviour.enabled && behaviour.behaviourInitialized)
            {
                behaviour.ServerFrame();
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        if (isClient)
        {
            FixedProcessAbilities();
        }
    }

    [Client]
    protected virtual void EveryFrame()
    {
        EarlyProcessAbilities();
        ProcessAbilities();
        LateProcessAbilities();
        UpdateAnimators();
    }

    [Client]
    protected virtual void EarlyProcessAbilities()
    {
        foreach (GenericBehaviour behaviour in overridingBehaviours)
        {
            if (behaviour.enabled && behaviour.behaviourInitialized)
            {
                behaviour.EarlyProcessAbility();
            }
        }
    }

    [Client]
    protected virtual void ProcessAbilities()
    {
        foreach (GenericBehaviour behaviour in overridingBehaviours)
        {
            if (behaviour.enabled && behaviour.behaviourInitialized)
            {
                behaviour.ProcessAbility();
            }
        }
    }

    [Client]
    protected virtual void LateProcessAbilities()
    {
        foreach (GenericBehaviour behaviour in overridingBehaviours)
        {
            if (behaviour.enabled && behaviour.behaviourInitialized)
            {
                behaviour.LateProcessAbility();
            }
        }
    }

    [Client]
    protected virtual void FixedProcessAbilities()
    {
        foreach (GenericBehaviour behaviour in overridingBehaviours)
        {
            if (behaviour.enabled && behaviour.behaviourInitialized)
            {
                behaviour.LocalFixedUpdate();
            }
        }
        Repositioning();
    }

    [Client]
    protected virtual void UpdateAnimators()
    {
        if (animator != null)
        {
            DHAnimator.UpdateAnimatorBool(animator, "Grounded", _controller.State.isGrounded, _animatorParameters);
            DHAnimator.UpdateAnimatorFloat(animator, "VerticalVelocity", _controller.verticalVelocity, _animatorParameters);
            DHAnimator.UpdateAnimatorFloat(animator, "GroundDistance", _controller.groundDistance, _animatorParameters);
            DHAnimator.UpdateAnimatorBool(animator, "Idle", (movementState.CurrentState == CharacterStates.MovementStates.Idle), _animatorParameters);
            foreach (GenericBehaviour behaviour in overridingBehaviours)
            {
                if (behaviour.enabled && behaviour.behaviourInitialized)
                {
                    behaviour.UpdateAnimator();
                }
            }
        }
    }

    protected virtual void GetInputManager()
    {
        LinkedInputManager = FindObjectOfType(typeof(InputManager)) as InputManager;
    }

    public Vector3 GetLastDirection()
    {
        return lastDirection;
    }

    public void SetLastDirection(Vector3 direction)
    {
        lastDirection = direction;
    }

    public void Repositioning()
    {
        if (lastDirection != Vector3.zero)
        {
            lastDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(lastDirection);
            Quaternion newRotation = Quaternion.Slerp(rBody.rotation,
                targetRotation, turnSmoothing);
            rBody.MoveRotation(newRotation);
        }
    }
}

public abstract class GenericBehaviour : NetworkBehaviour
{
    protected int speedFloat;
    protected BasicBehaviour behaviourManager;
    protected InputManager inputmanager;
    protected Animator _animator;
    protected Rigidbody _rigidbody;
    protected DHStateMachine<CharacterStates.MovementStates> movement;

    protected DHStateMachine<CharacterStates.CharacterConditions> condition;
    protected DHPhysicsController _controller;
    public bool behaviourInitialized = false;

    protected float verticalInput;
    protected float horizontalInput;

    protected float speed;

    protected void Start()
    {
        Initialization();
    }

    public override void OnStartLocalPlayer()
    {
        Camera.main.GetComponent<CameraMMO>().target = transform;
    }

    protected virtual void Initialization()
    {
        behaviourManager = GetComponent<BasicBehaviour>();
        _controller = GetComponent<DHPhysicsController>();

        movement = behaviourManager.movementState;
        condition = behaviourManager.conditionState;
        speedFloat = Animator.StringToHash("Speed");
        _animator = behaviourManager.GetAnimator;
        inputmanager = behaviourManager.LinkedInputManager;
        _rigidbody = behaviourManager.GetRigidBody;
        behaviourInitialized = true;

        if (_animator != null)
        {
            InitializeAnimatorParameters();
        }
    }

    [Server]
    public virtual void ServerFrame()
    {

    }

    [Client]
    public virtual void EarlyProcessAbility()
    {
        InternalHandleInput();
    }

    [Client]
    public virtual void ProcessAbility() { }

    [Client]
    public virtual void LateProcessAbility() { }

    [Client]
    public virtual void LocalFixedUpdate() { }

    [Client]
    public virtual void LocalLateUpdate() { }

    [Client]
    public virtual void UpdateAnimator() { }

    [Client]
    protected virtual void InternalHandleInput()
    {
        if (!isLocalPlayer)
            return;

        if (inputmanager == null)
            return;

        verticalInput = inputmanager.PrimaryMovement.y;
        horizontalInput = inputmanager.PrimaryMovement.x;
        HandleInput();
    }

    [Client]
    protected virtual void HandleInput() { }

    protected virtual void InitializeAnimatorParameters()
    {

    }

    protected virtual void RegisterAnimatorParameter(string parameterName, AnimatorControllerParameterType parameterType)
    {
        if (_animator == null)
        {
            return;
        }

        if (_animator.HasParameterOfType(parameterName, parameterType))
        {
            behaviourManager._animatorParameters.Add(parameterName);
        }
    }
}
