using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    [SerializeField] private Cell cell;
    public Cell Cell { get => cell; }


    [SerializeField] private Asset[] assets;

    private Dictionary<int, Asset> assetsDictionary = new Dictionary<int, Asset>();

    //assets by types
    private Dictionary<AssetTypes, List<Asset>> assetsByTypes = new Dictionary<AssetTypes, List<Asset>>();

    private void Awake()
    {
        if (assets != null && assets.Length > 0)
        {
            for (int i = 0; i < assets.Length; i++)
            {
                if (assetsDictionary.ContainsKey(assets[i].ID))
                {
                    Debug.LogError("such ID allready in assets: " + assets[i].ID);
                }
                else
                {
                    assetsDictionary.Add(assets[i].ID, assets[i]);
                    addAssetByType(assets[i]);
                    print("last id: " + assets[i].ID);
                }
            }
        }
    }
    private void addAssetByType(Asset _asset)
    {
        if (assetsByTypes.ContainsKey(_asset.AssetType))
        {
            assetsByTypes[_asset.AssetType].Add(_asset);
        }
        else
        {
            assetsByTypes.Add(_asset.AssetType, new List<Asset>() { _asset });
        }
    }


    public GameObject GetAssetByID(int id)
    {
        if (!assetsDictionary.ContainsKey(id))
        {
            Debug.LogError("no such ID in assets: " + id);
            return null;
        }

        return assetsDictionary[id].gameObject;
    }

    public GameObject GetAssetByTerrainType(TerrainTypes _type)
    {


        List<Asset> pool = assetsByTypes[AssetTypes.terrain];
        List<GameObject> results = new List<GameObject> ();

        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i].TerrainType == _type)
            {
                results.Add(pool[i].gameObject);
            }
        }

        if (results.Count == 0)
        {
            Debug.LogError("no such assets by terrain type: " + _type);
            return null;
        }
        else
        {
            return results[UnityEngine.Random.Range(0, results.Count)];
        }
    }

    public GameObject GetTree()
    {
        List<Asset> pool = assetsByTypes[AssetTypes.tree];

        return pool[UnityEngine.Random.Range(0, pool.Count)].gameObject;
    }
}
