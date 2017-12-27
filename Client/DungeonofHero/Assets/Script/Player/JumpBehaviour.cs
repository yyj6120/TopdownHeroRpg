using UnityEngine;

public class JumpBehaviour : GenericBehaviour
{
    private float jumpTimer = 0.3f;

    protected override void Initialization()
    {
        base.Initialization();
    }
    protected override void HandleInput()
    {
        if (movement.CurrentState != CharacterStates.MovementStates.Jumping && inputmanager.JumpButton.State.CurrentState == DHInput.ButtonStates.ButtonDown)
        {
            JumpStart();
        }
    }

    void JumpStart()
    {
        if (!EvaluateJumpConditions())
            return;

        movement.ChangeState(CharacterStates.MovementStates.Jumping);
        condition.ChangeState(CharacterStates.CharacterConditions.Normal);
        _controller.SetVerticalTimer(jumpTimer);
        _animator.CrossFadeInFixedTime("jump_start", 0.1f);
    }

    protected virtual bool EvaluateJumpConditions()
    {
        if (_controller.State.isJumping || condition.CurrentState != CharacterStates.CharacterConditions.Normal)
            return false;

        return true;
    }
}
