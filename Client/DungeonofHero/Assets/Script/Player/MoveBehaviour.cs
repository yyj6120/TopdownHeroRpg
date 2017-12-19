using UnityEngine;

public class MoveBehaviour : GenericBehaviour
{
	public float walkSpeed = 0.15f;               
	public float runSpeed = 1.0f;
    public float sprintSpeed = 2.0f;
    public float speedDampTime = 0.1f;
    protected bool canSprint;

    protected override void Initialization()
    {
        base.Initialization();
        speedSeeker = runSpeed;
        hFloat = Animator.StringToHash("H");
        vFloat = Animator.StringToHash("V");
        canSprint = false;
    }

    protected override void HandleInput()
    {
        if (inputmanager.SprintButton.State.CurrentState == DHInput.ButtonStates.ButtonDown || inputmanager.SprintButton.State.CurrentState == DHInput.ButtonStates.ButtonPressed)
        {
            canSprint = true;
        }
        else
        {
            canSprint = false;
        }
    }

    
    public override void ProcessAbility()
    {
        base.ProcessAbility();
        MovementManagement();
        Rotating();
    }

	void MovementManagement()
	{	
		if (behaviourManager.IsGrounded())
			_rigidbody.useGravity = true;

		Vector2 dir = new Vector2(horizontalInput, verticalInput);
		speed = Vector2.ClampMagnitude(dir, 1f).magnitude;
	
		speedSeeker += Input.GetAxis("Mouse ScrollWheel");
		speedSeeker = Mathf.Clamp(speedSeeker, walkSpeed, runSpeed);
		speed *= speedSeeker;

        if (canSprint)
        {
            speed = sprintSpeed;
        }
    }

	Vector3 Rotating()
	{
		Vector3 forward = mainCamara.transform.TransformDirection(Vector3.forward);

        forward.y = 0.0f;
		forward = forward.normalized;

		Vector3 right = new Vector3(forward.z, 0, -forward.x);
		Vector3 targetDirection;
		targetDirection = forward * verticalInput + right * horizontalInput;

		if((behaviourManager.IsMoving() && targetDirection != Vector3.zero))
		{
			Quaternion targetRotation = Quaternion.LookRotation (targetDirection);         
            Quaternion newRotation = Quaternion.Slerp(behaviourManager.GetRigidBody.rotation, targetRotation, behaviourManager.turnSmoothing);
            behaviourManager.GetRigidBody.MoveRotation (newRotation);
            behaviourManager.SetLastDirection(targetDirection);
		}

		if(!(Mathf.Abs(verticalInput) > 0.9 || Mathf.Abs(horizontalInput) > 0.9))
		{
			behaviourManager.Repositioning();
		}

		return targetDirection;
	}

    public override void UpdateAnimator()
    {
        animator.SetFloat(hFloat, horizontalInput, 0.1f, Time.deltaTime);
        animator.SetFloat(vFloat, verticalInput, 0.1f, Time.deltaTime);
        animator.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime);
    }
}
