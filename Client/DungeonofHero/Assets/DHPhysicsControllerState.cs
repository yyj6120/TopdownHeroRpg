using UnityEngine;

public class DHPhysicsControllerState
{
    public bool isGrounded;
    public bool isJumping;
    public bool JustGotGrounded { get; set; }
    public bool WasGroundedLastFrame { get; set; }
    public bool jumpAirControl = true;
    public virtual void Reset()
    {
        JustGotGrounded = false;
    }
}
