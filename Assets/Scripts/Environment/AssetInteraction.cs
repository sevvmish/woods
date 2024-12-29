using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class AssetInteraction : MonoBehaviour
{
    [SerializeField] private AssetManager assets;
    [SerializeField] private GameObject assetGiver;
    private ObjectPool assetGiverPool;
    

    // Start is called before the first frame update
    void Start()
    {        
        assetGiverPool = new ObjectPool(10, assetGiver, transform);
    }

    public void SpawnAssetGiverAtLocation(Vector3 pos, int resourceID, int itemID, int amountOfItem, Asset asset)
    {
        GameObject g = assetGiverPool.GetObject();
        g.GetComponent<AssetGiver>().SetData(assets, resourceID, itemID, amountOfItem, assetGiverPool);
        g.transform.position = pos + Vector3.down;
        g.SetActive(true);

        int x = 0;
        int z = 0;

        for (int i = 0; i < 100; i++)
        {
            x = UnityEngine.Random.Range(-1, 2);
            z = UnityEngine.Random.Range(-1, 2);

            if (asset.AssetType == AssetTypes.tree_hard || asset.AssetType == AssetTypes.medium_stone || asset.AssetType == AssetTypes.wood_log)
            {
                x = UnityEngine.Random.Range(-2, 3);
                z = UnityEngine.Random.Range(-2, 3);
            }

            if (new Vector2(x,z) != Globals.LastRandomThrownAsset)
            {
                Globals.LastRandomThrownAsset = new Vector2(x,z);
                break;
            }
        }
        

        Vector3 lastPos = pos + new Vector3(x, 0, z);
        g.transform.DOMove(g.transform.position + new Vector3(x, 0, z) + Vector3.up * 2, 0.3f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            g.transform.DOMove(lastPos, 0.5f).SetEase(Ease.OutBounce);
        });
        
    }
    
}
