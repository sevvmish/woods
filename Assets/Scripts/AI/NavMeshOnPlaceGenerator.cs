using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshOnPlaceGenerator : MonoBehaviour
{
    [SerializeField] private NavMeshSurface surface;
    
    private Vector3 size = new Vector3(10,10,10);
    private NavMeshData data;

    public void Create(Vector3 newSize)
    {
        data = null;

        size = newSize;
        surface.center = Vector3.zero;
        surface.size = size;

        surface.BuildNavMesh();
        data = surface.navMeshData;
        surface.UpdateNavMesh(data);
    }
}
