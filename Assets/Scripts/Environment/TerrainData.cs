using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainData : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    public MeshFilter MeshFilter { get => meshFilter; }

    public List<Cell> Cells { get; private set; }

    private void Awake()
    {
        Cells = new List<Cell>();
    }

    public void AddCell(Cell c) => Cells.Add(c);

    public void ReleaseCells(AssetManager assetManager)
    {
        if (Cells.Count > 0)
        {
            for (int i = 0; i < Cells.Count; i++)
            {
                if (Cells[i].transform.childCount > 0 && Cells[i].transform.GetChild(0).childCount > 0)
                {
                    for (int j = 0; j < Cells[i].transform.GetChild(0).childCount; j++)
                    {                        
                        assetManager.ReturnAsset(Cells[i].transform.GetChild(0).GetChild(j).gameObject);
                    }
                }
            }
        }

        if (Cells.Count > 0)
        {
            for (int i = 0; i < Cells.Count; i++)
            {
                Destroy(Cells[i].gameObject);
            }
        }

        
        Cells = new List<Cell>();
    }
}
