using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GateClosing;

public class NPC : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Ragdoll ragdoll;
    [SerializeField] Animator animator;
    [SerializeField] Transform hips;

    [SerializeField] float getUpTime;
    [SerializeField] float getUpTimer;
    [SerializeField] float newTargetTime;
    [SerializeField] float newTargetTimer;
    [SerializeField] float newTargetRadius;
    [SerializeField] float panicTime;
    [SerializeField] float panicTimer;
    [SerializeField] int character;

    [SerializeField] float walkSpeed;
    [SerializeField] float panicSpeed;
    [SerializeField] AudioManager audioManager;
    [SerializeField] int NPCType = 0;

    private AudioSource audioSource;
    bool _ragdolling;
    bool _navMeshActive;
    bool _panicking;

    public void OnEnable()
    {
        ragdoll.Ragdolled += OnRagdoll;
        ragdoll.RagdollReset += OnRagdollReset;

        newTargetTimer = UnityEngine.Random.Range(0f, newTargetTime);

        FindObjectOfType<Player>().HitSomeone += OnPlayerHitSomeone;
    }

    public void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void OnPlayerHitSomeone()
    {
        if (Vector3.Distance(transform.position, Player.instance.transform.position) < 10f)
        {
            Vector3 dir = transform.position - Player.instance.transform.position;
            Vector3 target = transform.position + UnityEngine.Random.insideUnitSphere * newTargetRadius + dir;
            navMeshAgent.SetDestination(target);
            newTargetTimer = newTargetTime;
            panicTimer = panicTime;
            _panicking = true;
        }
    }

    public void OnDisable()
    {
        ragdoll.Ragdolled -= OnRagdoll;
        ragdoll.RagdollReset -= OnRagdollReset;
        Player.instance.HitSomeone -= OnPlayerHitSomeone;
    }

    public void OnRagdoll()
    {
        audioSource.clip = audioManager.GetAudio(NPCType, 0);
        audioSource.spatialize = true;
        audioSource.spatialBlend = 1;
        audioSource.Play();
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
        } else
        {
            audioSource.transform.position = this.transform.position;
        }

        if (_panicking)
        {
            if (panicTimer > 0)
            {
                panicTimer -= Time.deltaTime;
                navMeshAgent.speed = panicSpeed;
            }
            else
            {
                _panicking = false;
                navMeshAgent.speed = walkSpeed;
            }
        }


        if (_navMeshActive)
        {
            if (newTargetTimer > 0)
            {
                float time = _panicking ? Time.deltaTime * 6 : Time.deltaTime;
                newTargetTimer -= time;
            }
            else
            {
                Vector3 target = transform.position + UnityEngine.Random.insideUnitSphere * newTargetRadius;
                navMeshAgent.SetDestination(target);
                newTargetTimer = newTargetTime;
            }
        }

        animator.SetBool("Panicking", _panicking);
        animator.SetBool("Walking", navMeshAgent.velocity != Vector3.zero);

    }
}
