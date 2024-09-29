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
}
