using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class ActionControl : MonoBehaviour
{    
    private GameObject currentAim;
    private PlayerControl playerControl;

    private void Start()
    {
        playerControl = GetComponent<PlayerControl>();
    }

    public void SetAim(GameObject a) => currentAim = a;

    public void UsePressed()
    {
        if (currentAim == null) return;

        if (currentAim.TryGetComponent(out Asset asset))
        {
            float distance = (transform.position - currentAim.transform.position).magnitude;

            if (distance > 10) return;

            if (ScreenAimNamer.IsChop(asset.AssetType))
            {
                print(currentAim.name + " chop");
            }
            else if (ScreenAimNamer.IsCollect(asset.AssetType))
            {
                print(currentAim.name + " collect");
            }
        }
        else
        {
            return;
        }
    }

    public void UseHit()
    {
        playerControl.SetHit();
    }
}
