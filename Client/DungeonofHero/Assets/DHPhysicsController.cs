using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DHPhysicsController : MonoBehaviour
{
    public DHPhysicsControllerState State { get; protected set; }
    protected Rigidbody _rigidbody;
    protected BasicBehaviour behaviourManager;
    protected CameraMMO mainCamara;
    protected Vector3 _velocity;

    public float defaultVelocity = 1f;
    [SerializeField]
    protected float extraGravity = -30f;

    public Animator animator;

    protected InputManager input;
    float stepOffsetEnd = 0.45f;
    float stepOffsetStart = 0.05f;
    protected CapsuleCollider _capsuleCollider;
    public LayerMask groundLayer = 1 << 0;
    public float stepSmooth = 2f;

    bool isDead;

    public RaycastHit groundHit;

    float slopeLimit = 45;
    [HideInInspector]
    public PhysicMaterial frictionPhysics, maxFrictionPhysics, slippyPhysics;

    float colliderHeight;
    Vector3 colliderCenter;
    float colliderRadius;

    float groundMinDistance = 0.25f;
    float groundMaxDistance = 0.5f;
    float groundDistance;
    float verticalVelocity;

    protected Vector2 _externalForce;

    protected void Start()
    {
        Initialization();
    }

    protected virtual void Initialization()
    {
        State = new DHPhysicsControllerState();
        behaviourManager = GetComponent<BasicBehaviour>();
        input = behaviourManager.LinkedInputManager;
        _rigidbody = behaviourManager.GetRigidBody;
        animator = behaviourManager.GetAnimator;
        mainCamara = behaviourManager.Getcamera;
        mainCamara.target = transform;
        _rigidbody = behaviourManager.GetRigidBody;

        frictionPhysics = new PhysicMaterial();
        frictionPhysics.name = "frictionPhysics";
        frictionPhysics.staticFriction = .25f;
        frictionPhysics.dynamicFriction = .25f;
        frictionPhysics.frictionCombine = PhysicMaterialCombine.Multiply;

        maxFrictionPhysics = new PhysicMaterial();
        maxFrictionPhysics.name = "maxFrictionPhysics";
        maxFrictionPhysics.staticFriction = 1f;
        maxFrictionPhysics.dynamicFriction = 1f;
        maxFrictionPhysics.frictionCombine = PhysicMaterialCombine.Maximum;

        slippyPhysics = new PhysicMaterial();
        slippyPhysics.name = "slippyPhysics";
        slippyPhysics.staticFriction = 0f;
        slippyPhysics.dynamicFriction = 0f;
        slippyPhysics.frictionCombine = PhysicMaterialCombine.Minimum;

        _capsuleCollider = GetComponent<CapsuleCollider>();

        colliderCenter = GetComponent<CapsuleCollider>().center;
        colliderRadius = GetComponent<CapsuleCollider>().radius;
        colliderHeight = GetComponent<CapsuleCollider>().height;
    }

    protected virtual void ApplyGravity()
    {

    }

    private void Update() { LocalUpdate(); }
    private void FixedUpdate() { LocalFixedUpdate(); }

    protected void LocalUpdate()
    {
        CheckGroundDistance();
        CheckGround();
    }

    protected void LocalFixedUpdate()
    {
        StepOffset();
        Rotating();
    }

    public void ControlSpeed(float velocity)
    {
        var deltaPosition = new Vector3(animator.deltaPosition.x, transform.position.y, animator.deltaPosition.z);
        Vector3 v = (deltaPosition * (velocity > 0 ? velocity : 1f)) / Time.deltaTime;
        v.y = _rigidbody.velocity.y;
        _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, v, 20f * Time.deltaTime);
    }

    public void OnAnimatorMove()
    {
        ControlSpeed(defaultVelocity);
    }

    bool StepOffset()
    {
        if (input.PrimaryMovement.sqrMagnitude < 0.1 || !State.isGrounded)
            return false;
        
        var _hit = new RaycastHit();
        var _movementDirection = transform.forward;
        Ray rayStep = new Ray((transform.position + new Vector3(0, stepOffsetEnd, 0) + _movementDirection * ((_capsuleCollider).radius + 0.05f)), Vector3.down);

        if (Physics.Raycast(rayStep, out _hit, stepOffsetEnd - stepOffsetStart, groundLayer))
        {
            if (_hit.point.y >= (transform.position.y) && _hit.point.y <= (transform.position.y + stepOffsetEnd))
            {
                var _speed = Mathf.Abs(_externalForce.x) + Mathf.Abs(_externalForce.y);
                var velocityDirection = (_hit.point - transform.position).normalized;
                _rigidbody.velocity = velocityDirection * stepSmooth * _speed;              
                return true;
            }
        }
        return false;
    }

    Vector3 Rotating()
    {
        Vector3 forward = mainCamara.transform.TransformDirection(Vector3.forward);

        forward.y = 0.0f;
        forward = forward.normalized;

        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        Vector3 targetDirection;
        targetDirection = forward * input.PrimaryMovement.y + right * input.PrimaryMovement.x;

        if ((behaviourManager.movementState.CurrentState == CharacterStates.MovementStates.Running
            && targetDirection != Vector3.zero))
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion newRotation = Quaternion.Slerp(_rigidbody.rotation, targetRotation, behaviourManager.turnSmoothing);
            behaviourManager.GetRigidBody.MoveRotation(newRotation);
            behaviourManager.SetLastDirection(targetDirection);
        }

        if (!(Mathf.Abs(input.PrimaryMovement.y) > 0.9 || Mathf.Abs(input.PrimaryMovement.x) > 0.9))
        {
            behaviourManager.Repositioning();
        }

        return targetDirection;
    }

    void CheckGround()
    {
        if (isDead)
        {
            State.isGrounded = true;
            return;
        }

        _capsuleCollider.material = (State.isGrounded && GroundAngle() <= slopeLimit + 1) ? frictionPhysics : slippyPhysics;

        if (State.isGrounded && _externalForce == Vector2.zero)
            _capsuleCollider.material = maxFrictionPhysics;
        else if (State.isGrounded && _externalForce != Vector2.zero)
            _capsuleCollider.material = frictionPhysics;
        else
            _capsuleCollider.material = slippyPhysics;

        var magVel = (float)System.Math.Round(new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z).magnitude, 2);
        magVel = Mathf.Clamp(magVel, 0, 1);

        var groundCheckDistance = groundMinDistance;
        if (magVel > 0.25f) groundCheckDistance = groundMaxDistance;

        var onStep = StepOffset();

        if (groundDistance <= 0.05f)
        {
            State.isGrounded = true;
        }
        else
        {
            if (groundDistance >= groundCheckDistance)
            {
                State.isGrounded = false;
                verticalVelocity = _rigidbody.velocity.y;

                if (!onStep)
                {
                    _rigidbody.AddForce(transform.up * extraGravity * Time.deltaTime, ForceMode.VelocityChange);
                }
            }
            else if (!onStep)
            {
                _rigidbody.AddForce(transform.up * (extraGravity * 2 * Time.deltaTime), ForceMode.VelocityChange);
            }
        }

    }

    public virtual float GroundAngle()
    {
        var groundAngle = Vector3.Angle(groundHit.normal, Vector3.up);
        return groundAngle;
    }

    void CheckGroundDistance()
    {
        if (isDead)
            return;

        if (_capsuleCollider != null)
        {
            float radius = _capsuleCollider.radius * 0.9f;
            var dist = 10f;

            Vector3 pos = transform.position + Vector3.up * (_capsuleCollider.radius);

            Ray ray1 = new Ray(transform.position + new Vector3(0, colliderHeight / 2, 0), Vector3.down);
            Ray ray2 = new Ray(pos, -Vector3.up);

            if (Physics.Raycast(ray1, out groundHit, colliderHeight / 2 + 2f, groundLayer))
                dist = transform.position.y - groundHit.point.y;
            if (Physics.SphereCast(ray2, radius, out groundHit, _capsuleCollider.radius + 2f, groundLayer))
            {
                if (dist > (groundHit.distance - _capsuleCollider.radius * 0.1f))
                    dist = (groundHit.distance - _capsuleCollider.radius * 0.1f);
            }
            groundDistance = (float)System.Math.Round(dist, 2);
        }
    }

    public virtual void SetForce(Vector2 force)
    {
        _externalForce = force;
    }

    public void SetVerticalForce()
    {
        if (!isJumping) return;

        jumpCounter -= Time.deltaTime;
        if (jumpCounter <= 0)
        {
            jumpCounter = 0;
            isJumping = false;
        }
        // apply extra force to the jump height   
        var vel = _rigidbody.velocity;
        vel.y = jumpHeight;
        Debug.Log(vel.y);
        _rigidbody.velocity = vel;
    }
}
