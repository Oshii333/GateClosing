using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Ragdoll ragdoll;
    [SerializeField] Animator animator;

    public void OnEnable()
    {
        ragdoll.Ragdolled += OnRagdoll;
        ragdoll.RagdollReset += OnRagdollReset;
    }

    public void OnDisable()
    {
        ragdoll.Ragdolled -= OnRagdoll;
        ragdoll.RagdollReset -= OnRagdollReset;
    }

    public void OnRagdoll()
    {
        SetNavMeshAgent(active:false);
        SetAnimator(active:false);
    }

    public void OnRagdollReset()
    {
        SetNavMeshAgent(active:true);
        SetAnimator(active:true);
    }

    public void SetNavMeshAgent(bool active)
    {
        if (active)
        {
            navMeshAgent.nextPosition = transform.position;
        }
        navMeshAgent.updatePosition = active;
        navMeshAgent.updateRotation = active;
        navMeshAgent.enabled = active;
    }

    public void SetAnimator(bool active)
    {
        animator.enabled = active;
    }

}
