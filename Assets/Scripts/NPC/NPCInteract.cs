using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteract : MonoBehaviour, IInteractable
{
    public Asset CurrentAsset { get; private set; }
    public float CurrentHP { get; private set; }

    private float HPamount = 10;

    [SerializeField] private int dropsAtEnd = 3;
    [SerializeField] private int resourceID_for_visual;
    [SerializeField] private int itemID_for_inventory;
    [SerializeField] private float forUpVector = 1f;
    [SerializeField] private AdditionalLootdata[] lootData;


    private AssetManager assets;
    private EffectsManager effects;
    private NPCManager npc;

    public void SetHP(float hp)
    {
        HPamount = hp;
        CurrentHP = hp;
    }


    private void Start()
    {
        if (TryGetComponent(out Asset a))
        {
            CurrentAsset = a;
        }
        else if (transform.parent.TryGetComponent(out Asset a1))
        {
            CurrentAsset = a1;
        }

        assets = GameObject.Find("AssetManager").GetComponent<AssetManager>();
        effects = GameObject.Find("EffectsManager").GetComponent<EffectsManager>();
        npc = GetComponent<NPCManager>();

        CurrentHP = HPamount;
    }


    public void GetHit(float damage)
    {
        CurrentHP -= damage;
        AssetTypes _type = CurrentAsset.AssetType;

        if (CurrentHP <= 0)
        {            
            assets.SpawnAssetGiverAtLocation(transform.position + Vector3.up * forUpVector, resourceID_for_visual, itemID_for_inventory, dropsAtEnd, CurrentAsset);
            if (lootData.Length > 0)
            {
                for (int i = 0; i < lootData.Length; i++)
                {
                    if (lootData[i].Chance < 100)
                    {
                        int rnd = UnityEngine.Random.Range(1, 101);
                        if (rnd > lootData[i].Chance) continue;
                    }

                    assets.SpawnAssetGiverAtLocation(transform.position + Vector3.up * lootData[i].ForUpVector, lootData[i].ResourceID_for_visual, lootData[i].ItemID_for_inventory, lootData[i].Amount, CurrentAsset);
                }
            }

            
            end().Forget();
        }
        else
        {
            npc.GetHit();
        }
    }

    private async UniTaskVoid end()
    {
        npc.MakeDead();

        await UniTask.Delay(3000);

        assets.ReturnAsset(CurrentAsset.gameObject);
    }
}
