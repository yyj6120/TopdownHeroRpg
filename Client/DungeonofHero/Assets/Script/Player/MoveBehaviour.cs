﻿using UnityEngine;
using UnityEngine.Networking;

public class MoveBehaviour : GenericBehaviour
{
    public float sprintSpeed = 2.0f;
    protected bool canSprint;

    protected override void Initialization()
    {
        base.Initialization();
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
    }

    void MovementManagement()
	{
        if (_controller.State.JustGotGrounded)
        {
            _rigidbody.useGravity = true;
            movement.ChangeState(CharacterStates.MovementStates.Idle);
        }

		Vector2 dir = new Vector2(horizontalInput, verticalInput);
		speed = Vector2.ClampMagnitude(dir, 1f).magnitude;
        _controller.SetForce(dir);

        if (speed != 0
            && _controller.State.isGrounded
            && movement.CurrentState == CharacterStates.MovementStates.Idle)
        {
            movement.ChangeState(CharacterStates.MovementStates.Running);
        }

        if (speed == 0 
            && _controller.State.isGrounded
            && movement.CurrentState == CharacterStates.MovementStates.Running)
        {
            movement.ChangeState(CharacterStates.MovementStates.Idle);
        }

        if (!_controller.State.isGrounded &&
            ((movement.CurrentState == CharacterStates.MovementStates.Running) || (movement.CurrentState == CharacterStates.MovementStates.Idle)))
        {
            movement.ChangeState(CharacterStates.MovementStates.Falling);
        }

        if (canSprint)
        {
            speed = sprintSpeed;
        }
    }

    protected override void InitializeAnimatorParameters()
    {
        RegisterAnimatorParameter("Speed", AnimatorControllerParameterType.Float);
        RegisterAnimatorParameter("InputVertical", AnimatorControllerParameterType.Float);
        RegisterAnimatorParameter("Movement", AnimatorControllerParameterType.Bool);
    }

    public override void UpdateAnimator()
    {
        DHAnimator.UpdateAnimatorFloat(_animator, "Speed", speed);
        DHAnimator.UpdateAnimatorFloat(_animator, "InputVertical", speed , 0.25f);
        DHAnimator.UpdateAnimatorBool(_animator, "Movement", movement.CurrentState == CharacterStates.MovementStates.Running);
    }
}
