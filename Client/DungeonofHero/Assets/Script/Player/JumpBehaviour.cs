using UnityEngine;

public class JumpBehaviour : GenericBehaviour
{
    protected override void Initialization()
    {
        base.Initialization();
    }
    protected override void HandleInput()
    {
        if (_controller.State.isJumping == false && inputmanager.JumpButton.State.CurrentState == DHInput.ButtonStates.ButtonDown)
        {
            JumpStart();
        }
    }
    //void JumpManagement()
    //{
    //    if (jump && !behaviourManager.GetAnimator.GetBool(jumpBool) && _controller.State.isGrounded)
    //    {
    //        movement.ChangeState(CharacterStates.MovementStates.Jumping);
    //        behaviourManager.GetAnimator.SetBool(jumpBool, true);
    //        if (behaviourManager.GetAnimator.GetFloat(speedFloat) > 0.1)
    //        {
    //            GetComponent<CapsuleCollider>().material.dynamicFriction = 0f;
    //            GetComponent<CapsuleCollider>().material.staticFriction = 0f;
    //            float velocity = 2f * Mathf.Abs(Physics.gravity.y) * jumpHeight;
    //            velocity = Mathf.Sqrt(velocity);
    //            _rigidbody.AddForce(Vector3.up * velocity, ForceMode.VelocityChange);
    //        }
    //    }
    //    else if (behaviourManager.GetAnimator.GetBool(jumpBool))
    //    {
    //        if (!_controller.State.isGrounded && !isColliding)
    //        {
    //            behaviourManager.GetRigidBody.AddForce(transform.forward * jumpIntertialForce * Physics.gravity.magnitude * accelerationSpeed, ForceMode.Acceleration);
    //        }

    //        if ((behaviourManager.GetRigidBody.velocity.y < 0) && _controller.State.isGrounded)
    //        {
    //            behaviourManager.GetAnimator.SetBool(groundedBool, true);
    //            GetComponent<CapsuleCollider>().material.dynamicFriction = 0.6f;
    //            GetComponent<CapsuleCollider>().material.staticFriction = 0.6f;
    //            jump = false;
    //            behaviourManager.GetAnimator.SetBool(jumpBool, false);
    //        }
    //    }
    //}
    void JumpStart()
    {
        if (!EvaluateJumpConditions())
            return;

        movement.ChangeState(CharacterStates.MovementStates.Jumping);
        condition.ChangeState(CharacterStates.CharacterConditions.Normal);
        //    _controller.GravityActive(true);
      //  _controller.SetVerticalForce();
    }

    protected virtual bool EvaluateJumpConditions()
    {
        if (_controller.State.isJumping || condition.CurrentState != CharacterStates.CharacterConditions.Normal)
            return false;

        return true;
    }

    protected override void InitializeAnimatorParameters()
    {
        RegisterAnimatorParameter("Jumping", AnimatorControllerParameterType.Bool);
    //    RegisterAnimatorParameter("HitTheGround", AnimatorControllerParameterType.Bool);
    }

    public override void UpdateAnimator()
    {
        DHAnimator.UpdateAnimatorBool(_animator, "Jumping", (movement.CurrentState == CharacterStates.MovementStates.Jumping), behaviourManager._animatorParameters);
     //   DHAnimator.UpdateAnimatorBool(_animator, "HitTheGround", _controller.State.isGrounded, behaviourManager._animatorParameters);
    }

}
