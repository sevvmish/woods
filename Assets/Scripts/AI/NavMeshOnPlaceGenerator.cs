using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshOnPlaceGenerator : MonoBehaviour
{
    [SerializeField] private NavMeshSurface surface;
    
    private Vector3 size = new Vector3(10,100,10);
    private NavMeshData data;

    public void Create(Vector3 newSize)
    {
        data = null;

        size = new Vector3(newSize.x, 100, newSize.z);
        surface.center = Vector3.zero;
        surface.size = size;

        
    }

    private void OnEnable()
    {
        surface.BuildNavMesh();
        data = surface.navMeshData;
        surface.UpdateNavMesh(data);
    }
}
