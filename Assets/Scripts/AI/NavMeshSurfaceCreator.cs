using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshSurfaceCreator : MonoBehaviour
{
    private NavMeshSurface surface;

    private NavMeshData data;

    private void OnEnable()
    {
        
        //NavMeshData ddd = new NavMeshData();
        //NavMeshDataInstance inst = new NavMeshDataInstance();
        //inst = NavMesh.AddNavMeshData(ddd, new Vector3(20, 0, 20), Quaternion.identity);

        
        surface = NavMeshSurface.activeSurfaces[0];

        surface.collectObjects = CollectObjects.Volume;
        surface.size = new Vector3(40, 40, 40);
        surface.center = new Vector3(110, 0, 110);
        
        surface.BuildNavMesh();
        data = surface.navMeshData;
        surface.UpdateNavMesh(data);


    }

    private void OnDisable()
    {
        
        surface.RemoveData();
        
    }
}
