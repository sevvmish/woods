using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalLoot : MonoBehaviour
{
    [SerializeField] private AdditionalLootdata[] lootData;

    private AssetManager assets;
    private Asset currentAsset;


    // Start is called before the first frame update
    void Start()
    {
        assets = GameObject.Find("AssetManager").GetComponent<AssetManager>();
        currentAsset = GetComponent<Asset>();
    }

    public void Giveloot()
    {
        if (lootData.Length == 0) return;

        for (int i = 0; i < lootData.Length; i++)
        {
            if (lootData[i].Chance < 100)
            {
                int rnd = UnityEngine.Random.Range(1, 101);
                if (rnd > lootData[i].Chance) continue;
            }
            
            assets.SpawnAssetGiverAtLocation(transform.position + Vector3.up * lootData[i].ForUpVector, lootData[i].ResourceID_for_visual, lootData[i].ItemID_for_inventory, lootData[i].Amount, currentAsset);
        }
    }

}

[Serializable]
public class AdditionalLootdata
{
    public int ResourceID_for_visual = -1;
    public int ItemID_for_inventory = -1;
    public int Amount = 1;
    public float ForUpVector = 1f;
    public int Chance = 100;

}
