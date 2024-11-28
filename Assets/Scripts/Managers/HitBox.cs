using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    private EffectsManager effects;
    private ShowDPSUI showDPS;
    private HashSet<Collider> colliders = new HashSet<Collider>();

    public void SetHitBox(EffectsManager effects, ShowDPSUI showDPS)
    {
        this.effects = effects;
        this.showDPS = showDPS;
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (!colliders.Contains(other) && other.TryGetComponent(out Asset asset))
        {            
            colliders.Add(other);
            if (Asset.IsChop(asset.AssetType))
            {
                if (asset.TryGetComponent(out Interactable i) && i.CurrentHP > 0)
                {
                    i.GetHit(2);
                }
                    
                showDPS.ShowDPS(2, other.gameObject.transform, Vector3.up * 1.2f);
                effects.PlayEffectAtLocation(effects.PunchImpactBluntPool, other.gameObject.transform.position + Vector3.up * 1.2f, 0.5f);
            }
            else if (Asset.IsCollect(asset.AssetType))
            {
                if (asset.TryGetComponent(out Interactable i) && i.CurrentHP > 0)
                {
                    i.GetHit(2);
                }

                //showDPS.ShowDPS(2, other.gameObject.transform, Vector3.up * 1.2f);
                //effects.PlayEffectAtLocation(effects.PunchImpactBluntPool, other.gameObject.transform.position + Vector3.up * 1.2f, 0.5f);
            }
        }
    }
}
