using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshSurfaceCreator : MonoBehaviour
{
    [SerializeField] private NavMeshSurface surface;

    /// <summary>
    /// The center of the build 
    /// </summary>
    private NavMeshData data;

    private void OnEnable()
    {
        surface.size = new Vector3(12, 12, 12);
        surface.BuildNavMesh();
        data = surface.navMeshData;
        surface.UpdateNavMesh(data);
    }
}
