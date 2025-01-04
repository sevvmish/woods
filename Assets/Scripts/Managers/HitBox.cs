using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    private EffectsManager effects;
    private ShowDPSUI showDPS;
    private HashSet<Collider> colliders = new HashSet<Collider>();
    private Item weapon;
    private Transform hitterTransform;

    public void SetHitBox(EffectsManager effects, ShowDPSUI showDPS, Item weapon, Transform pc)
    {
        this.effects = effects;
        this.showDPS = showDPS;
        this.weapon = weapon;
        hitterTransform = pc;
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (!colliders.Contains(other) && other.TryGetComponent(out Asset asset))
        {            
            colliders.Add(other);

            if (Asset.IsChop(asset.AssetType))
            {
                float damage = 0;

                if (asset.TryGetComponent(out Interactable i) && i.CurrentHP > 0)
                {                    
                    if (weapon != null && Item.IsItemForChop(weapon))
                    {
                        damage = weapon.Damage / 2f;
                        effects.PlayEffectAtLocation(0, effects.ChopWoodEffectPool, other.gameObject.transform.position + Vector3.up * 1.2f, hitterTransform.eulerAngles, Vector3.one, 1f);
                    }
                    else
                    {
                        effects.PlayEffectAtLocation(effects.PunchImpactBluntPool, other.gameObject.transform.position + Vector3.up * 1.2f, 0.5f);
                    }
                                        
                    i.GetHit(damage);
                    showDPS.ShowDPS(damage, other.gameObject.transform, Vector3.up * 1.2f);
                }
            }
            else  if (Asset.IsMine(asset.AssetType))
            {
                float damage = 0;

                if (asset.TryGetComponent(out Interactable i) && i.CurrentHP > 0)
                {
                    if (weapon != null && Item.IsItemForMine(weapon))
                    {
                        damage = weapon.Damage / 2f;
                        effects.PlayEffectAtLocation(effects.MineStoneEffectPool, other.gameObject.transform.position + Vector3.up * 1.2f, 1f);
                    }
                    else
                    {
                        effects.PlayEffectAtLocation(effects.PunchImpactBluntPool, other.gameObject.transform.position + Vector3.up * 1.2f, 0.5f);
                    }

                    i.GetHit(damage);
                    showDPS.ShowDPS(damage, other.gameObject.transform, Vector3.up * 1.2f);
                }
            }
            else if (Asset.IsCollect(asset.AssetType) || Asset.IsPickUp(asset.AssetType))
            {
                if (asset.TryGetComponent(out Interactable i) && i.CurrentHP > 0)
                {
                    i.GetHit(2);
                }

            }
            else
            {
                if (asset.TryGetComponent(out Interactable i) && i.CurrentHP > 0)
                {
                    i.GetHit(4);
                    showDPS.ShowDPS(4, other.gameObject.transform, Vector3.up * 1.2f);

                    if (asset.AssetType == AssetTypes.bush)
                    {
                        effects.PlayEffectAtLocation(effects.BushHitPool, other.gameObject.transform.position, 1.5f);
                    }
                }
            }
        }
    }
}
