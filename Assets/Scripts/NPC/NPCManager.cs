using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class NPCManager : MonoBehaviour
{
    public float Diameter => diameter;
    public bool IsDead { get; private set; } = false;
    public float CurrentSpeed => agent.velocity.magnitude;

    private HitControl hitControl;
    public HitControl HitControl => hitControl;

    [Header("AI")]
    [SerializeReference] private NavMeshOnPlaceGenerator navMeshOnPlaceGenerator;
    [SerializeReference] private float diameter = 10;
    private NavMeshAgent agent;    
    private NPCstats stats;
    private NPCAnimator animator;
    private bool isCanAct = true;
    private bool isCanHit = true;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        stats = GetComponent<NPCstats>();
        animator = transform.GetChild(0).GetComponent<NPCAnimator>();
    }

    public void Restart()
    {
        IsDead = false;
        stats.Restart();
        agent.Warp(transform.parent.position);
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public void MakeDead()
    {
        IsDead = true;
        animator.Dead();
        agent.isStopped = true;
        agent.ResetPath();
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public void GetHit()
    {
        animator.GetHit();
    }
        

    private void Start()
    {
        hitControl = GameObject.Find("=====MAIN=====").GetComponent<HitControl>();
        navMeshOnPlaceGenerator.Create(Vector3.one * diameter);
        navMeshOnPlaceGenerator.gameObject.SetActive(false);
        createNavMeshSurface().Forget();

        
        //agent.Warp(transform.parent.position);
        
        
    }
    private async UniTaskVoid createNavMeshSurface()
    {
        await UniTask.Delay(100);
        navMeshOnPlaceGenerator.gameObject.SetActive(true);
    }

    public bool WalkToPoint(Vector3 point)
    {        
        if (agent.isOnNavMesh)
        {
            agent.speed = stats.WalkSpeed;
            agent.ResetPath();
            return agent.SetDestination(point);
        }
        else
        {
            return false;
        }
    }

    public async UniTaskVoid Hit()
    {
        if (isCanAct && isCanHit)
        {
            agent.isStopped = true;
            agent.ResetPath();
            isCanAct = false;
            await animator.Hit();
            setHitCooldown(stats.AttackSpeed).Forget();
            isCanAct = true;
        }        
    }
    private async UniTaskVoid setHitCooldown(float _time)
    {
        isCanHit = false;
        await UniTask.Delay((int)(_time * 1000));
        isCanHit = true;
    }

    
    public bool RunToPoint(Vector3 point)
    {        
        if (agent.isOnNavMesh)
        {
            agent.speed = stats.MaxSpeed;
            agent.ResetPath();
            return agent.SetDestination(point);
        }
        else
        {
            return false;
        }
    }

}
