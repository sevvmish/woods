using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class TerrainData : MonoBehaviour
{
    

    [SerializeField] private MeshFilter meshFilter;
    public MeshFilter MeshFilter { get => meshFilter; }

    public List<Cell> Cells { get; private set; }

    private AssetManager assetManager;


    private void Awake()
    {
        Cells = new List<Cell>();
        assetManager = GameObject.Find("AssetManager").GetComponent<AssetManager>();
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
                    while (Cells[i].transform.GetChild(0).childCount > 0)
                    {                        
                        assetManager.ReturnAsset(Cells[i].transform.GetChild(0).GetChild(0).gameObject);
                    }
                    
                }

                if (Cells[i].transform.childCount > 0 && Cells[i].transform.GetChild(1).childCount > 0)
                {                    
                    while (Cells[i].transform.GetChild(1).childCount > 0)
                    {
                        assetManager.ReturnAsset(Cells[i].transform.GetChild(1).GetChild(0).gameObject);
                    }
                    
                }
            }
        }

        if (Cells.Count > 0)
        {
            for (int i = 0; i < Cells.Count; i++)
            {
                //print(Cells[i].transform.GetChild(0).childCount + " = " + Cells[i].transform.GetChild(1).childCount);
                assetManager.ReturnCell(Cells[i]);//Destroy(Cells[i].gameObject);
            }
        }

        
        Cells = new List<Cell>();
    }
}
