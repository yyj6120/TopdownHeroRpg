using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DHPhysicsController : MonoBehaviour
{
    protected float _speed;
    protected Rigidbody _rigidbody;
    [HideInInspector]
    protected float speed, verticalVelocity;

    protected BasicBehaviour behaviourManager;
    protected CameraMMO mainCamara;
    protected Vector3 _velocity;

    protected void Start()
    {
        Initialization();
    }

    protected virtual void Initialization()
    {
        behaviourManager = GetComponent<BasicBehaviour>();
        input = behaviourManager.LinkedInputManager;
        _rigidbody = behaviourManager.GetRigidBody;
        mainCamara = behaviourManager.Getcamera;
        mainCamara.target = transform;
        _rigidbody = behaviourManager.GetRigidBody;
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    protected virtual void ApplyGravity()
    {

    }

    private void FixedUpdate()
    {
        StepOffset();
        _rigidbody.velocity = _velocity;
        _rigidbody.AddForce(transform.forward * (_speed) * Time.deltaTime, ForceMode.VelocityChange);
    }

    public void ControlSpeed(float velocity)
    {
        _speed = velocity;
        _velocity.y = _rigidbody.velocity.y;
        _velocity = transform.forward * velocity * _speed;
    }


    bool isGrounded;
    protected InputManager input;
    float stepOffsetEnd = 0.45f;
    float stepOffsetStart = 0.05f;
    protected CapsuleCollider _capsuleCollider;
    public LayerMask groundLayer = 1 << 0;
    public float stepSmooth = 4f;

    bool StepOffset()
    {
        var _hit = new RaycastHit();
        var _movementDirection = input.PrimaryMovement.magnitude > 0 ? (transform.right * input.PrimaryMovement.x + transform.forward * input.PrimaryMovement.y).normalized : transform.forward;
        Ray rayStep = new Ray((transform.position + new Vector3(0, stepOffsetEnd, 0) + _movementDirection * ((_capsuleCollider).radius + 0.05f)), Vector3.down);
        
        if (Physics.Raycast(rayStep, out _hit, stepOffsetEnd - stepOffsetStart, groundLayer))
        {
            if (_hit.point.y >= (transform.position.y) && _hit.point.y <= (transform.position.y + stepOffsetEnd))
            {
                var velocityDirection = (_hit.point - transform.position).normalized;
                _rigidbody.velocity = velocityDirection * stepSmooth * (speed);
                return true;
            }
        }
        return false;
    }
}
