using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

public class NatureGenerator : MonoBehaviour
{
    [Inject] private AssetManager assetManager;

    [SerializeField] private GameObject testGrass;

    public void GenerateNatureInTerrainChunk(GameObject terrainChunk)
    {
        TerrainData data = terrainChunk.GetComponent<TerrainData>();
        MeshFilter mf = data.MeshFilter;
        

        Mesh mesh = mf.mesh;
        Vector3[] verts = mesh.vertices;
        List<Vector3> forRandom = verts.ToList();

        for (int i = 0; i < 50; i++)
        {
            int index = Globals.MainPlayerData.MainRandom.Next(0, forRandom.Count);

            GameObject g = Instantiate(assetManager.GetTree());
            g.transform.position = mf.transform.position + forRandom[index] + Vector3.down * 0.2f;

            for (int j = 0; j < data.Cells.Count; j++)
            {
                if (data.Cells[j].IsInsideBounds(g.transform.position))
                {
                    g.transform.parent = data.Cells[j].Location;
                    break;
                }
            }


            
            g.SetActive(true);

            forRandom.Remove(forRandom[index]);
        }

        

        
        for (int i = 0; i < verts.Length; i++)
        {
            int rnd = UnityEngine.Random.Range(0, 12);

            if (rnd == 0)
            {
                GameObject g = Instantiate(testGrass);
                g.transform.position = mf.transform.position + verts[i] + Vector3.down * 0.1f;
                g.transform.localEulerAngles += new Vector3(0, UnityEngine.Random.Range(90, 270), 0);
                g.SetActive(false);

                for (int j = 0; j < data.Cells.Count; j++)
                {
                    if (data.Cells[j].IsInsideBounds(g.transform.position))
                    {
                        g.transform.parent = data.Cells[j].Location;
                        data.Cells[j].closeObjects.Add(g);
                        break;
                    }
                }
            }

            
        }
        

    }

}
