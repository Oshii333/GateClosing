using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class FloatingCapsule : MonoBehaviour
{
    public CapsuleProfile p;

    public float forceHeight;
    public Vector3 movementInput;
    public Vector3 lookInput;
    public Vector3 gravityDirection;

    private Vector3 hitNormal;
    private float springForce;
    private Vector3 vel;
    private Vector3 lateralVel;
    private Vector3 verticalVel;
    private Vector3 angVel;
    private Vector3 movementDirection;
    private Vector3 boostDirection;
    private float acceleration;
    private Vector3 velocityChange;
    private Quaternion slopeAdjust;

    //AddTorque
    private Vector3 forwardTorque;
    private Vector3 upwardTorque;
    private Vector3 finalTorque;
    private float verticalForce;

    //Animator
    Vector3 animVelocity;
    public float speedLevel;
    public float speedForward;
    public float speedRight;
    public bool isGrounded;
    bool falling = true;
    public bool jumping;
    bool jumped;

    private Rigidbody rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        //speedLevel = Mathf.Lerp(speedLevel, movementInput.magnitude, 0.5f);
        vel = rb.velocity;
        angVel = rb.angularVelocity;

        if (!falling)
        {
            if (transform.InverseTransformDirection(vel).y < 0) falling = true;
        }

        Raycast();
        AddLateralForce();
        AddVerticalForce();
        AddTorque();
        Gravity();
        SetAnimatorValues();

        if (boostDirection != Vector3.zero)
        {
            rb.AddForce(p.boostForce * boostDirection, ForceMode.VelocityChange);
            boostDirection = Vector3.zero;
        }

        movementInput = Vector3.zero;
    }


    public void Move(Vector3 newMovementInput)
    {
        movementInput = newMovementInput;
        //lookInput = movementInput;
    }

    public void Look(Vector3 newLookInput)
    {
        lookInput = newLookInput;
    }

    public void Boost(Vector3 direction)
    {
        boostDirection = direction;
    }

    public void Jump(bool value)
    {
        jumping = value;
    }

    public void Raycast()
    {
        if (jumped & !falling) return;

        if (Physics.Raycast(rb.worldCenterOfMass, 
            gravityDirection, out RaycastHit hit, p.rideHeight))
        {
            jumped = false;
            Debug.DrawLine(rb.worldCenterOfMass, hit.point, Color.green);
                springForce =
                ((p.rideHeight - hit.distance) * p.springStrength) -
                (Vector3.Dot(-gravityDirection, vel) * p.springDamper);
                rb.AddForce(-gravityDirection * springForce);
                isGrounded = true;
            hitNormal = hit.normal;
        }
        else
        {
            Debug.DrawRay(rb.worldCenterOfMass, gravityDirection * p.rideHeight, Color.red);
            isGrounded = false;
            hitNormal = Vector3.up;
        }
    }

    //add movement input as lateral force adjusted for slope
    public void AddLateralForce()
    {
        lateralVel = vel;
        lateralVel.y = 0;
        movementDirection = p.lateralSpeed * movementInput.magnitude * movementInput.normalized;
        movementDirection.y = 0;

        slopeAdjust = Quaternion.FromToRotation(-gravityDirection, hitNormal);
        movementDirection = slopeAdjust * movementDirection;
        lateralVel = slopeAdjust * lateralVel;
        velocityChange = movementDirection - lateralVel;
        acceleration = movementDirection.magnitude - lateralVel.magnitude;
        if (acceleration < 0)
        {
            acceleration = p.deceleration;
        }
        else
        {
            acceleration = Mathf.Lerp(p.initialAcceleration, p.finalAcceleration, speedForward);
        }
        

        velocityChange = Vector3.ClampMagnitude(velocityChange, acceleration);
        Vector3 forcePosition = rb.worldCenterOfMass + transform.up * forceHeight;
        rb.AddForceAtPosition(velocityChange, forcePosition, ForceMode.VelocityChange);
    }

    public void AddVerticalForce()
    {
        if (!jumping) return;
        float vel = Vector3.Dot(rb.velocity, -gravityDirection);
        if (isGrounded)
        {
            rb.AddForce(p.jumpForce * -gravityDirection, ForceMode.Impulse);
            jumped = true;
        }
        vel = Mathf.Clamp(p.verticalSpeed - vel, 0, p.maxVerticalAcceleration);
        rb.AddForce(vel * -gravityDirection, ForceMode.VelocityChange);
    }

    private void Gravity()
    {
        //if rising decrease gravity
        if (isGrounded)
        {
            verticalForce = rb.mass * p.gravity;
        }
        else
        {
            float velDown = Vector3.Dot(gravityDirection, vel.normalized-gravityDirection * 0.01f);
            falling = velDown > 0;
            velDown = Mathf.Clamp(velDown, 0, 1f);
            velDown = Mathf.Lerp(1f, p.fallingMultiplier, velDown);
            verticalForce = rb.mass * p.gravity * velDown;
        }
        rb.AddForce(gravityDirection * verticalForce, ForceMode.Acceleration);
    }

    public void AddTorque()
    {
        Quaternion rot = rb.rotation;
        forwardTorque = Vector3.Cross(rot * Vector3.forward, lookInput);
        upwardTorque = Vector3.Cross(rot * Vector3.up, -gravityDirection);
        Debug.DrawRay(rb.worldCenterOfMass, rot * Vector3.forward * 100f, Color.blue);
        Vector3 torque = Vector3.ClampMagnitude(forwardTorque + upwardTorque, 1f);

        rb.AddTorque(torque * p.torqueStrength - rb.angularVelocity * p.torqueDamper);
    }

    public void SetAnimatorValues()
    {
        //speedLevel = animVelocity.magnitude;
        speedLevel = Mathf.Lerp(speedLevel, movementInput.magnitude, 0.5f);
        animVelocity = transform.InverseTransformDirection(rb.velocity);
        speedForward = Mathf.Lerp(-1, 1, Mathf.InverseLerp(-p.lateralSpeed, p.lateralSpeed, animVelocity.z));
        speedRight = Mathf.Lerp(-1, 1, Mathf.InverseLerp(-p.lateralSpeed, p.lateralSpeed, animVelocity.x));
    }
}


