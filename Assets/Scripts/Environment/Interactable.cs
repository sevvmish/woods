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

    public float CurrentHP { get; private set; }

    private void OnEnable()
    {
        assets = GameObject.Find("AssetManager").GetComponent<AssetManager>();
        CurrentHP = HPamount;
        isBefore = false;
    }

    public void GetHit(float damage)
    {
        CurrentHP -= damage;

        if (CurrentHP <= 0)
        {
            assets.SpawnAssetGiverAtLocation(transform.position + Vector3.up * forUpVector, resourceID, itemID, dropsAtEnd);

            if (!isBefore)
            {
                assets.SpawnAssetGiverAtLocation(transform.position + Vector3.up * forUpVector, resourceID, itemID, dropsBeforeEnd);
            }

            assets.ReturnAsset(gameObject);
        }
        else if (!isBefore && (HPamount/2f) >= CurrentHP)
        {
            isBefore = true;
            assets.SpawnAssetGiverAtLocation(transform.position + Vector3.up * forUpVector, resourceID, itemID, dropsBeforeEnd);
        }
    }


    
}
