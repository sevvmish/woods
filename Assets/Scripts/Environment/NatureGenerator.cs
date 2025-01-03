using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using VContainer;

public class NatureGenerator : MonoBehaviour
{
    [Inject] private AssetManager assetManager;
    [Inject] private WorldGenerator worldGenerator;

    private HashSet<int> busyIndexes = new HashSet<int>();
    private List<VoidZone> voidZones = new List<VoidZone>();
    private List<VoidZone> emptyZones = new List<VoidZone>();

    private TerrainData data;
    private MeshFilter mf;
    private Vector3[] verts;
    private Vector3[] normals;
    private List<Vector3> positionOnMesh;
        

    public async UniTaskVoid GenerateNatureInTerrainChunk(GameObject terrainChunk, bool isFast)
    {        
        data = terrainChunk.GetComponent<TerrainData>();
        mf = data.MeshFilter;


        Mesh mesh = mf.mesh;
        verts = mesh.vertices;
        normals = mesh.normals;
        positionOnMesh = verts.ToList();

        if (worldGenerator.IsEmptyPlace(terrainChunk.transform.position))
        {            
            emptyZones.Add(new VoidZone(terrainChunk.transform.position, 40));            
        }

        generateTrees();
        if (!isFast) await UniTask.Delay(50);
        generateResources();
        if (!isFast) await UniTask.Delay(50);
        generateGrass();
        if (!isFast) await UniTask.Delay(50);
        clearAll();

    }

    
    private void clearAll()
    {        
        //CLEAN ALL
        busyIndexes.Clear();
        voidZones.Clear();
        emptyZones.Clear();

        data = null;
        mf = null;

        verts = new Vector3[0];
        normals = new Vector3[0];
        positionOnMesh.Clear();
    }

    private void generateGrass()
    {        
        for (int i = 0; i < verts.Length; i++)
        {
            if (busyIndexes.Contains(i)) continue;

            int lowerIndex = 0;
            bool isInEmpty = IsInEmptyZone(mf.transform.position + verts[i]);
            List<int> listWithIDs = new List<int>();

            if (!Globals.IsMobile)
            {
                if (isInEmpty)
                {
                    int rndIndexGrass = worldGenerator.GetRandomIndex(0, i, 3, Globals.MainPlayerData.NatureSeed + 3);

                    if (rndIndexGrass < 2)
                    {
                        AddNewGrass(assetManager.GetAssetByID(18),
                        mf.transform.position + verts[i] + Vector3.down * 0.1f,
                        new Vector3(90, 0, 0),
                        normals[i]
                        );
                    }


                    lowerIndex = -50;
                    listWithIDs = new List<int> { 34, 35, 33, 15, 16 };
                }
                else
                {
                    lowerIndex = -80;
                    listWithIDs = new List<int> { 18, 18, 18, 9, 9, 9, 17 };
                }

                if (Globals.IsLowFPS) lowerIndex -= 10;
            }
            else
            {
                if (isInEmpty)
                {
                    if (!Globals.IsLowFPS)
                    {
                        int rndIndexGrass = worldGenerator.GetRandomIndex(0, i, 3, Globals.MainPlayerData.NatureSeed + 3);

                        if (rndIndexGrass == 1)
                        {
                            AddNewGrass(assetManager.GetAssetByID(18),
                            mf.transform.position + verts[i] + Vector3.down * 0.1f,
                            new Vector3(90, 0, 0),
                            normals[i]
                            );
                        }
                        

                        lowerIndex = -70;
                        listWithIDs = new List<int> { 34, 35, 33, 15, 16 };
                    }
                    else
                    {
                                                
                        int rndIndexGrass = worldGenerator.GetRandomIndex(0, i, 5, Globals.MainPlayerData.NatureSeed + 3);

                        if (rndIndexGrass == 1)
                        {
                            AddNewGrass(assetManager.GetAssetByID(18),
                            mf.transform.position + verts[i] + Vector3.down * 0.1f,
                            new Vector3(90, 0, 0),
                            normals[i]
                            );
                        }


                        lowerIndex = -100;
                        listWithIDs = new List<int> { 34, 35, 33, 15, 16 };
                    }
                    
                }
                else
                {
                    if (!Globals.IsLowFPS)
                    {
                        lowerIndex = -90;
                        listWithIDs = new List<int> { 18, 18, 18, 9, 9, 9, 17 };
                    }
                    else
                    {
                        lowerIndex = -150;
                        listWithIDs = new List<int> { 18, 18, 18, 9, 9, 9, 17 };
                    }
                    
                }
            }



            int rndIndex = worldGenerator.GetRandomIndex(lowerIndex, i, listWithIDs.Count, Globals.MainPlayerData.NatureSeed + 2);

            if (rndIndex >= 0)
            {                
                AddNewGrass(assetManager.GetAssetByIndexFromLists(listWithIDs, rndIndex),
                    mf.transform.position + verts[i] + Vector3.down * 0.1f,
                    new Vector3(90, 0, 0),
                    normals[i]
                    );
            }
        }
    }

    private void AddNewGrass(GameObject g, Vector3 pos, Vector3 eulerAngle, Vector3 normals)
    {
        g.transform.position = pos;
        g.transform.LookAt(g.transform.position + normals);
        g.transform.eulerAngles += eulerAngle;
        g.SetActive(true);

        for (int j = 0; j < data.Cells.Count; j++)
        {
            if (data.Cells[j].IsInsideBounds(g.transform.position))
            {
                g.transform.parent = data.Cells[j].LocationClose;
                //data.Cells[j].closeObjects.Add(g);
                break;
            }
        }
    }

    private void generateTrees()
    {
                
        //TREE GENERATOR
        for (int i = 0; i < verts.Length; i++)
        {
            if (IsInVoidZone(mf.transform.position + positionOnMesh[i])) continue;
            if (IsInEmptyZone(mf.transform.position + positionOnMesh[i])) continue;

            int rndIndex = worldGenerator.GetRandomIndex(-150, i, 8, Globals.MainPlayerData.NatureSeed);

            if (rndIndex >= 0)
            {
                AddNewTree(assetManager.GetTree(AssetTypes.terrain_flat, rndIndex),
                    mf.transform.position + positionOnMesh[i] + Vector3.down * 0.2f,
                    new Vector3(0, UnityEngine.Random.Range(90, 270), 0), i);
            }
        }
    }

    private void generateResources()
    {
        
        //resources
        for (int i = 0; i < verts.Length; i++)
        {
            if (IsInVoidZone(mf.transform.position + positionOnMesh[i])) continue;

            List<int> listWithIDs = new List<int>();
            int lowerIndex = 0;
            
            bool isInEmpty = IsInEmptyZone(mf.transform.position + positionOnMesh[i]);

            if (isInEmpty)
            {
                lowerIndex = -700;
                listWithIDs = new List<int> { 22, 22, 23, 23, 20, 20, 21, 21, 36 };
            }
            else
            {
                lowerIndex = -500;
                listWithIDs = new List<int> { 44, 45, 38, 10, 10, 10, 10, 19, 19, 19, 19, 22, 22, 22, 23, 23, 20, 20, 21, 21, 36, 26, 27 };
            }

            int rndIndex = worldGenerator.GetRandomIndex(lowerIndex, i, listWithIDs.Count, Globals.MainPlayerData.NatureSeed + 1);

            if (rndIndex >= 0)
            {
                AddNewResource(assetManager.GetAssetByIndexFromLists(listWithIDs, rndIndex),
                    mf.transform.position + verts[i] + Vector3.down * 0.1f,
                    new Vector3(0, UnityEngine.Random.Range(90, 270), 0));

                //medium stones
                if (listWithIDs[rndIndex] == 44 || listWithIDs[rndIndex] == 45)
                {
                    busyIndexes.Add(i);
                }
            }
        }
    }

    private void AddNewResource(GameObject g, Vector3 pos, Vector3 eulerAngle)
    {
        g.transform.position = pos;        
        g.transform.localEulerAngles += eulerAngle;
        g.SetActive(true);

        for (int j = 0; j < data.Cells.Count; j++)
        {
            if (data.Cells[j].IsInsideBounds(g.transform.position))
            {
                g.transform.parent = data.Cells[j].LocationClose;
                //data.Cells[j].closeObjects.Add(g);
                break;
            }
        }
    }

    private void AddNewTree(GameObject g, Vector3 pos, Vector3 localEulerAngle, int index)
    {
        busyIndexes.Add(index);
        
        g.transform.position = pos;
        g.transform.localEulerAngles += localEulerAngle;

        if (g.TryGetComponent(out Asset asset))
        {
            switch (asset.AssetType)
            {
                case AssetTypes.tree_hard:
                    voidZones.Add(new VoidZone(g.transform.position, 7));
                    break;

                case AssetTypes.tree_small:
                    voidZones.Add(new VoidZone(g.transform.position, 3));
                    break;
            }
        }


        for (int j = 0; j < data.Cells.Count; j++)
        {
            if (data.Cells[j].IsInsideBounds(g.transform.position))
            {
                g.transform.parent = data.Cells[j].LocationFar;
                break;
            }
        }
        g.SetActive(true);
    }



    private bool IsInVoidZone(Vector3 point)
    {
        if (voidZones.Count > 0)
        {
            for (int i = 0; i < voidZones.Count; i++)
            {
                if (voidZones[i].IsInRadius(point))
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    private bool IsInEmptyZone(Vector3 point)
    {
        if (emptyZones.Count > 0)
        {
            for (int i = 0; i < emptyZones.Count; i++)
            {
                if (emptyZones[i].IsInRadius(point))
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            return false;
        }
    }


}

public struct VoidZone
{    
    private Vector3 center;
    private float radius;


    public VoidZone(Vector3 center, float radius)
    {
        this.center = center;
        this.radius = radius;
    }

    public bool IsInRadius(Vector3 pointer)
    {
        float distance = (center - pointer).magnitude;

        return distance < radius;
    }
}
