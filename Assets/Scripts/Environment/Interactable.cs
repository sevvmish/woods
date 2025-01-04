using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private float HPamount = 10;
    [SerializeField] private int dropsBeforeEnd = 2;

    private bool isBefore;
    
    [SerializeField] private int dropsAtEnd = 3;

    [SerializeField] private int resourceID_for_visual;
    [SerializeField] private int itemID_for_inventory;

    [SerializeField] private float forUpVector = 1f;

    private Asset currentAsset;
    private AssetManager assets;
    private EffectsManager effects;
    

    public float CurrentHP { get; private set; }

    private void Start()
    {
        currentAsset = GetComponent<Asset>();
        assets = GameObject.Find("AssetManager").GetComponent<AssetManager>();
        effects = GameObject.Find("EffectsManager").GetComponent<EffectsManager>();
    }

    private void OnEnable()
    {        
        CurrentHP = HPamount;
        isBefore = false;
    }

    public void GetHit(float damage)
    {
        CurrentHP -= damage;
        AssetTypes _type = currentAsset.AssetType;

        if (CurrentHP <= 0)
        {
            //loot at end
            assets.SpawnAssetGiverAtLocation(transform.position + Vector3.up * forUpVector, resourceID_for_visual, itemID_for_inventory, dropsAtEnd, currentAsset);

            //loot at middle
            if (!isBefore && dropsBeforeEnd > 0)
            {
                assets.SpawnAssetGiverAtLocation(transform.position + Vector3.up * forUpVector, resourceID_for_visual, itemID_for_inventory, dropsBeforeEnd, currentAsset);
            }

            //additional loot
            if (gameObject.TryGetComponent(out AdditionalLoot loot))
            {
                loot.Giveloot();
            }
            
            //end action
            if (Asset.IsChop(_type))
            {
                effects.PlayEffectAtLocation(effects.TreeDestroyedPool, transform.position, 2f);
                
            }
            else if (Asset.IsMine(_type))
            {
                effects.PlayEffectAtLocation(effects.StoneDestroyedPool, transform.position, 2f);
                
            }
            else if (Asset.IsCollect(_type))
            {
                effects.PlayEffectAtLocation(effects.CollectResourcePool, transform.position, 1f);
                
            }
            else if (Asset.IsPickUp(_type))
            {
                effects.PlayEffectAtLocation(effects.BushHitPool, transform.position, 1.5f);                
            }
            else if (currentAsset.AssetType == AssetTypes.bush)
            {
                effects.PlayEffectAtLocation(effects.BushBreakPool, transform.position, 1.5f);
                
            }

            assets.ReturnAsset(gameObject);

        }
        else
        {
            if (dropsBeforeEnd > 0 && !isBefore && (HPamount / 2f) >= CurrentHP)
            {
                isBefore = true;
                assets.SpawnAssetGiverAtLocation(transform.position + Vector3.up * forUpVector, resourceID_for_visual, itemID_for_inventory, dropsBeforeEnd, currentAsset);
            }

            if (Asset.IsChop(_type))
            {
                if (currentAsset.AssetType == AssetTypes.tree_small)
                {
                    transform.DOKill();
                    Vector3 rot = transform.eulerAngles;
                    transform.eulerAngles = rot + new Vector3(UnityEngine.Random.Range(-3, 3), 0, UnityEngine.Random.Range(-3, 3));
                    transform.DORotate(rot, 0.3f).SetEase(Ease.OutBounce);//.OnComplete(() => { transform.eulerAngles = rot; });
                }
                else
                {
                    effects.GetCameraControl.ShakeCameraRot(Ease.OutBounce, 5, 0.3f);
                }
            }
            else if (Asset.IsMine(_type))
            {
                effects.GetCameraControl.ShakeCameraPos(Ease.OutBounce, 0.2f, 0.3f);
            }
            else if (Asset.IsCollect(_type))
            {
                
            }                        
        }
        
    }


    
}
