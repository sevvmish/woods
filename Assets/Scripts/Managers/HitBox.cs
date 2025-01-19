using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    private EffectsManager effects;
    private ShowDPSUI showDPS;
    private HashSet<IInteractable> interactables = new HashSet<IInteractable>();
    private Item weapon;
    private Transform hitterTransform;
    private IInteractable source;

    public void SetHitBox(IInteractable source, EffectsManager effects, ShowDPSUI showDPS, Item weapon, Transform pc)
    {
        this.effects = effects;
        this.showDPS = showDPS;
        this.weapon = weapon;
        hitterTransform = pc;
        this.source = source;
    }

    private void OnTriggerEnter(Collider other)
    {           
        if (other.TryGetComponent(out IInteractable i) && !interactables.Contains(i) && !i.Equals(source) && i.CurrentHP > 0)
        {            
            interactables.Add(i);
            Asset asset = i.CurrentAsset;

            if (asset != null && Asset.IsChop(asset.AssetType))
            {
                float damage = 0;

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
                showDPS.ShowDPS(damage, other.gameObject.transform, Vector3.up * 1.2f, ShowDPSUI.AimTypes.None);
            }
            else if (asset != null && Asset.IsMine(asset.AssetType))
            {
                float damage = 0;

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
                showDPS.ShowDPS(damage, other.gameObject.transform, Vector3.up * 1.2f, ShowDPSUI.AimTypes.None);
            }
            else if (asset != null && (Asset.IsCollect(asset.AssetType) || Asset.IsPickUp(asset.AssetType)))
            {
                i.GetHit(2);
            }            
            else
            {                
                i.GetHit(4);
                
                if (other.gameObject.layer == 3)
                {
                    showDPS.ShowDPS(4, other.gameObject.transform, Vector3.up * 1.2f, ShowDPSUI.AimTypes.Player);                    
                }
                else if (other.TryGetComponent(out NPCstats npc))
                {
                    showDPS.ShowDPS(4, other.gameObject.transform, Vector3.up * 1.2f, ShowDPSUI.AimTypes.NPCs);                    
                }
                else if (asset != null && asset.AssetType == AssetTypes.bush)
                {
                    effects.PlayEffectAtLocation(effects.BushHitPool, other.gameObject.transform.position, 1.5f);
                    showDPS.ShowDPS(4, other.gameObject.transform, Vector3.up * 1.2f, ShowDPSUI.AimTypes.None);
                }
            }
        }
    }
}
