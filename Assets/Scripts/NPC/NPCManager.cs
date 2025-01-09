using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCManager : MonoBehaviour
{
    public float Diameter => diameter;    

    [Header("AI")]
    [SerializeField] private NavMeshOnPlaceGenerator navMeshOnPlaceGenerator;
    [SerializeField] private float diameter = 10;
    private NavMeshAgent agent;
    private NPCstats stats;

    private void Awake()
    {
        navMeshOnPlaceGenerator.Create(Vector3.one * diameter);
        navMeshOnPlaceGenerator.gameObject.SetActive(false);
        createNavMeshSurface().Forget();

        agent = GetComponent<NavMeshAgent>();
        agent.Warp(transform.parent.position);

        stats = GetComponent<NPCstats>();
    }
    private async UniTaskVoid createNavMeshSurface()
    {
        await UniTask.Delay(500);
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
