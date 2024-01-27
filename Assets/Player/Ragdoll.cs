using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] Animator animator;

    Collider[] colliders;
    Rigidbody[] rigidbodies;
    public float resetTime;
    public float resetTimer;
    public bool ragdolling;

    bool hit = false;

    public void OnHit(Vector3 force)
    {
        if (hit) return;
        hit = true;
        animator.enabled = false;
        ragdolling = true;
        resetTimer = 0;
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].velocity = Vector3.up * 2f;
            rigidbodies[i].AddForce(force, ForceMode.Impulse);
        }

    }

    public void OnEnable()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            ColliderManager.Ragdolls.Add(collider, this);
        }
    }

    public void OnDisable()
    {
        foreach (var collider in colliders)
        {
            ColliderManager.Ragdolls.Remove(collider);
        }
    }


    public void Update()
    {
        if (ragdolling)
        {
            resetTimer += Time.deltaTime;
            if (resetTimer > resetTime)
            {
                ResetRagdoll();
            }
        }
    }

    public void ResetRagdoll()
    {
        ragdolling = false;
        animator.enabled = true;
        hit = false;
    }
}
