using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asset : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private AssetTypes assetType;
    [SerializeField] private TerrainTypes terrainType;

    public int ID { get => id; }
    public AssetTypes AssetType { get => assetType; }        
    public TerrainTypes TerrainType { get => terrainType; }


}

public enum AssetTypes
{
    none,
    terrain,
    tree,
    stone,
    rock
}

public enum TerrainTypes
{
    none,
    flat,
    mediumHighlands,
    mountains
}
