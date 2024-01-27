using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CapsuleProfile", menuName = "ScriptableObjects/CapsuleProfile")]
public class CapsuleProfile : ScriptableObject
{
    public float springStrength;
    public float springDamper;

    public float torqueStrength;
    public float torqueDamper;

    public float friction;
    public float rideHeight;

    [Header("Acceleration at top speed")]
    public float finalAcceleration;
    [Header("Acceleration from a standstill")]
    public float initialAcceleration;
    public float deceleration;

    public float maxVerticalAcceleration;

    [Header("MaxSpeeds")]
    public float lateralSpeed;
    public float verticalSpeed;

    public float boostForce;
    public float jumpForce;
    public float fallingMultiplier;

    public float gravity;

}
