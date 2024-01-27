using GateClosing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardNPC : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Ragdoll ragdoll;
    [SerializeField] Animator animator;
    [SerializeField] Transform hips;
    [SerializeField] Collider guardHurt;
    [SerializeField] PlayerInput player;
    [SerializeField] Transform playerTransform;

    [SerializeField] float getUpTime;
    [SerializeField] float getUpTimer;
    [SerializeField] float newTargetTime;
    [SerializeField] float newTargetTimer;

    bool _ragdolling;
    bool _navMeshActive;

    public void OnEnable()
    {
        ragdoll.Ragdolled += OnRagdoll;
        ragdoll.RagdollReset += OnRagdollReset;
        guardHurt.enabled = true;
        newTargetTimer = UnityEngine.Random.Range(0f, newTargetTime);
    }

    public void OnDisable()
    {
        ragdoll.Ragdolled -= OnRagdoll;
        ragdoll.RagdollReset -= OnRagdollReset;
    }

    public void OnRagdoll()
    {
        _ragdolling = true;
        _navMeshActive = false;
        SetNavMeshAgent(active:false);
        SetAnimator(active:false);
    }

    public void OnRagdollReset()
    {
        NavMesh.SamplePosition(hips.position, out NavMeshHit navMeshHit, 10f, NavMesh.AllAreas);
        transform.position = navMeshHit.position;


        Vector3 lookDir = Vector3.ProjectOnPlane(hips.up, Vector3.up);
        Quaternion lookRot = Quaternion.LookRotation(lookDir);
        transform.rotation = lookRot;

        _ragdolling = false;
        getUpTimer = getUpTime;

        //if facing up get up back else get up front
        if (Vector3.Dot(hips.forward, Vector3.up) > 0)
        {
            animator.SetTrigger("GetUpBack");
        }
        else
        {
            animator.SetTrigger("GetUpFront");
        }

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

    public void Update()
    {
        if (!_ragdolling)
        {
            if (getUpTimer > 0)
            {
                getUpTimer -= Time.deltaTime;
            }
            else if (!_navMeshActive)
            {
                SetNavMeshAgent(active: true);
                _navMeshActive = true;
            }
        }

        if (_navMeshActive)
        {
            if (newTargetTimer > 0)
            {
                newTargetTimer -= Time.deltaTime;
            }
            else
            {
                if (playerTransform)
                {
                    if (!guardHurt.enabled)
                    {
                        guardHurt.enabled = true;
                    }
                    Vector3 playerTarget = playerTransform.position;
                    float  distance = Vector3.Distance(playerTransform.position, transform.position);
                    if (distance < 1.5) 
                    {
                        attack();
                    }
                    navMeshAgent.SetDestination(playerTarget);
                }
            }
        }
        
        animator.SetBool("Walking", navMeshAgent.velocity != Vector3.zero);

    }

    void attack()
    {
        player.floatingCapsule.rb.velocity = transform.forward * 100;
    }
}
