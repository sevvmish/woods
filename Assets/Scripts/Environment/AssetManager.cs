using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AssetManager : MonoBehaviour
{
    [SerializeField] private Cell cell;
    public Cell GetCell()
    {
        return cellPool.GetObject().GetComponent<Cell>();
    }
    public void ReturnCell(Cell c)
    {
        cellPool.ReturnObject(c.gameObject);
    }
    private ObjectPool cellPool;



    [SerializeField] private Asset[] assets;

    private Dictionary<int, Asset> assetsDictionary = new Dictionary<int, Asset>();
    private Dictionary<int, ObjectPool> assetsPools = new Dictionary<int, ObjectPool>();

    //assets by types
    private Dictionary<AssetTypes, List<Asset>> assetsByTypes = new Dictionary<AssetTypes, List<Asset>>();

    //toDEL
    private Dictionary<int, int> howmany = new Dictionary<int, int>();
    private Dictionary<int, int> howmanyReturnedDictionary = new Dictionary<int, int>();
    private int howmanyReturned;

    private AssetInteraction assetInteraction;
    public void SpawnAssetGiverAtLocation(Vector3 pos, int resourceID, int itemID, int amountOfItem) => assetInteraction.SpawnAssetGiverAtLocation(pos, resourceID, itemID, amountOfItem);

    private void Awake()
    {
        assetInteraction = GetComponent<AssetInteraction>();

        cellPool = new ObjectPool(300, cell.gameObject, transform);

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

                    int amount = 1;

                    assetsPools.Add(assets[i].ID, new ObjectPool(amount, assets[i].gameObject, transform));
                    addAssetByType(assets[i]);                    
                }
            }
        }

        prepareAdditionalPools().Forget();
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
    private async UniTaskVoid prepareAdditionalPools()
    {
        await UniTask.Delay(500);

        foreach (var item in howmany.Keys)
        {
            if (howmany[item] > 1)
            {
                HashSet<int> doubleIDs = new HashSet<int>() {9,15,16,17,18,33,34,35,   10,19,    22,23 };
                float koef = 0.7f;

                if (doubleIDs.Contains(item))
                {
                    koef = 2f;
                }


                assetsPools[item].AddAdditionalLimit((int)(howmany[item] * koef));
            }
            //print("ID taken: " + item + ", amount taken: " + howmany[item]);
        }

        howmany.Clear();
    }



    public GameObject GetAssetByID(int id)
    {
        if (!assetsDictionary.ContainsKey(id))
        {
            Debug.LogError("no such ID in assets: " + id);
            return null;
        }

        if (howmany.ContainsKey(id))
        {
            howmany[id] += 1; 
        }
        else
        {
            howmany.Add(id, 1);
        }

        //return assetsDictionary[id].gameObject;
        return assetsPools[id].GetObject();
    }

    
    public void ReturnAsset(GameObject g)
    {
        int id = -1;

        if (g.TryGetComponent(out Asset asset))
        {
            id = asset.ID;
        }
        else
        {
            Debug.LogError("no asset on object");
            return;
        }

        if (!assetsPools.ContainsKey(id))
        {
            Debug.LogError("no such object pool in assets with ID: " + id);
        }
        else
        {
            if (howmanyReturnedDictionary.ContainsKey(id))
            {
                howmanyReturnedDictionary[id] += 1;
            }
            else
            {
                howmanyReturnedDictionary.Add(id, 1);
            }
            assetsPools[id].ReturnObject(g);            
        }
        
    }

    public GameObject GetAssetByTerrainType(AssetTypes _type)
    {
        List<int> IDs = new List<int>();

        switch (_type)
        {
            case AssetTypes.terrain_flat:
                IDs = new List<int>() { 1, 2 };
                return GetAssetByID(IDs[UnityEngine.Random.Range(0, IDs.Count)]);

        }

        return GetAssetByID(1);
    }

    public GameObject GetAssetByTerrainType(AssetTypes _type, int index)
    {
        List<int> IDs = new List<int>();

        switch (_type)
        {
            case AssetTypes.terrain_flat:
                IDs = new List<int>() { 1, 2 };
                return GetAssetByID(IDs[index]);

        }

        return GetAssetByID(1);
    }


    public GameObject GetTree(AssetTypes treeSize, AssetTypes terrainType)
    {
        List<Asset> poolHard = new List<Asset>();
        List<Asset> poolSmall = new List<Asset>();
        List<int> treesHardIDs = new List<int>() { 13, 11, 28 };
        List<int> treesSmallIDs = new List<int>() { 14, 12 };

        if (Globals.IsLowFPS)
        {
            treesHardIDs = new List<int>() { 7, 5, 6, 31 };
            treesSmallIDs = new List<int>() { 8, 3, 4, 32 };
        }

        List<int> IDs = new List<int>();

        switch (terrainType)
        {
            case AssetTypes.terrain_flat:
                switch (treeSize)
                {
                    case AssetTypes.tree_small:

                        if (Globals.IsLowFPS)
                        {
                            IDs = new List<int>() { 8, 3, 4, 32 };
                        }
                        else
                        {
                            IDs = new List<int>() { 14, 12 };
                        }
                        return GetAssetByID(IDs[UnityEngine.Random.Range(0, IDs.Count)]);

                    case AssetTypes.tree_hard:

                        if (Globals.IsLowFPS)
                        {
                            IDs = new List<int>() { 7, 5, 6, 31 };
                        }
                        else
                        {
                            IDs = new List<int>() { 13, 11, 28 };
                        }
                        return GetAssetByID(IDs[UnityEngine.Random.Range(0, IDs.Count)]);

                }
                break;

        }

        throw new System.NotImplementedException();

    }

    public GameObject GetTree(AssetTypes terrainType, int index)
    {  
        List<int> IDs = new List<int>();

        switch (terrainType)
        {
            case AssetTypes.terrain_flat:           

                if (Globals.IsLowFPS && Globals.IsMobile)
                {
                    IDs = new List<int>() { 8, 3, 4, 32,    7, 5, 6, 31 };
                }
                else
                {
                    IDs = new List<int>() { 14, 12, 13, 30,    11, 28, 29 };
                }

                if (index >= IDs.Count)
                {
                    return GetAssetByID(IDs[IDs.Count - 1]);
                }
                else
                {
                    return GetAssetByID(IDs[index]);
                }

        }

        throw new System.NotImplementedException();
    }

    public GameObject GetAssetByIndexFromLists(List<int> forLowFPS, List<int> forGoodFPS, int index)
    {
        List<int> IDs = new List<int>();

        if (Globals.IsLowFPS)
        {
            IDs = forLowFPS;
        }
        else
        {
            IDs = forGoodFPS;
        }

        if (index >= IDs.Count)
        {
            return GetAssetByID(IDs[IDs.Count - 1]);
        }
        else
        {
            return GetAssetByID(IDs[index]);
        }

        throw new System.NotImplementedException();
    }

    public GameObject GetAssetByIndexFromLists(List<int> mainList, int index)
    {
        List<int> IDs = new List<int>();

        IDs = mainList;

        if (index >= IDs.Count)
        {
            return GetAssetByID(IDs[IDs.Count - 1]);
        }
        else
        {
            return GetAssetByID(IDs[index]);
        }

        throw new System.NotImplementedException();
    }
}
