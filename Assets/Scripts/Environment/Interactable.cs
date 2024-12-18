using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private float HPamount = 10;
    [SerializeField] private int dropsBeforeEnd = 2;

    private bool isBefore;
    
    [SerializeField] private int dropsAtEnd = 3;

    [SerializeField] private int resourceID;
    [SerializeField] private int itemID;

    [SerializeField] private float forUpVector = 1f;


    private AssetManager assets;
    private EffectsManager effects;

    public float CurrentHP { get; private set; }

    private void OnEnable()
    {
        assets = GameObject.Find("AssetManager").GetComponent<AssetManager>();
        effects = GameObject.Find("EffectsManager").GetComponent<EffectsManager>();
        CurrentHP = HPamount;
        isBefore = false;
    }

    public void GetHit(float damage)
    {
        CurrentHP -= damage;

        if (CurrentHP <= 0)
        {
            assets.SpawnAssetGiverAtLocation(transform.position + Vector3.up * forUpVector, resourceID, itemID, dropsAtEnd);

            if (!isBefore && dropsBeforeEnd > 0)
            {
                assets.SpawnAssetGiverAtLocation(transform.position + Vector3.up * forUpVector, resourceID, itemID, dropsBeforeEnd);
            }


            AssetTypes _type = GetComponent<Asset>().AssetType;

            if (Asset.IsChop(_type))
            {
                effects.PlayEffectAtLocation(effects.TreeDestroyedPool, transform.position, 2f);
            }
            else if (Asset.IsMine(_type))
            {
                effects.PlayEffectAtLocation(effects.StoneDestroyedPool, transform.position, 2f);
            }

            
            assets.ReturnAsset(gameObject);
        }
        else if (dropsBeforeEnd > 0 && !isBefore && (HPamount/2f) >= CurrentHP)
        {
            isBefore = true;
            assets.SpawnAssetGiverAtLocation(transform.position + Vector3.up * forUpVector, resourceID, itemID, dropsBeforeEnd);
        }
    }


    
}
