using UnityEngine;

public class JumpBehaviour : GenericBehaviour
{
    private bool jump;
    private int jumpBool;
    public float jumpHeight = 1.5f;
    private bool isColliding;
    public float jumpIntertialForce = 10f;
    private int groundedBool;
    private float accelerationSpeed = 2.0f;
    protected override void Initialization()
    {
        base.Initialization();
        jumpBool = Animator.StringToHash("Jump");
        groundedBool = Animator.StringToHash("Grounded");
        behaviourManager.GetAnim.SetBool(groundedBool, true);
    }

    protected override void HandleInput()
    {
        if (jump == false && inputmanager.JumpButton.State.CurrentState == DHInput.ButtonStates.ButtonDown)
        {
            jump = true;
        }
    }

    public override void LocalFixedUpdate() { JumpManagement(); }

    void JumpManagement()
    {
        if (jump && !behaviourManager.GetAnim.GetBool(jumpBool) && behaviourManager.IsGrounded())
        {
            behaviourManager.GetAnim.SetBool(jumpBool, true);
            if (behaviourManager.GetAnim.GetFloat(speedFloat) > 0.1)
            {
                GetComponent<CapsuleCollider>().material.dynamicFriction = 0f;
                GetComponent<CapsuleCollider>().material.staticFriction = 0f;
                float velocity = 2f * Mathf.Abs(Physics.gravity.y) * jumpHeight;
                velocity = Mathf.Sqrt(velocity);
                _rigidbody.AddForce(Vector3.up * velocity, ForceMode.VelocityChange);
            }
        }
        else if (behaviourManager.GetAnim.GetBool(jumpBool))
        {
            if (!behaviourManager.IsGrounded() && !isColliding)
            {
                behaviourManager.GetRigidBody.AddForce(transform.forward * jumpIntertialForce * Physics.gravity.magnitude * accelerationSpeed, ForceMode.Acceleration);
            }

            if ((behaviourManager.GetRigidBody.velocity.y < 0) && behaviourManager.IsGrounded())
            {
                behaviourManager.GetAnim.SetBool(groundedBool, true);
                GetComponent<CapsuleCollider>().material.dynamicFriction = 0.6f;
                GetComponent<CapsuleCollider>().material.staticFriction = 0.6f;
                jump = false;
                behaviourManager.GetAnim.SetBool(jumpBool, false);
            }
        }
    }

    public override void UpdateAnimator()
    {
    
    }

    private void OnCollisionStay(Collision collision)
    {
        isColliding = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isColliding = false;
    }
}
