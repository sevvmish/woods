using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class ActionControl : MonoBehaviour
{    
    private GameObject currentAim;
    private PlayerControl playerControl;
    private Inventory inventory;
    private InputControl inputControl;
    

    public void SetInput(InputControl i) => inputControl = i;
    
    private void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        inventory = GetComponent<Inventory>();
    }

    public void SetAim(GameObject a)
    {        
        currentAim = a;

        if (Globals.IsMobile)
        {
            if (currentAim == null)
            {
                inputControl.ShowHitButton();
            }
            else
            {
                float distance = (transform.position - currentAim.transform.position).magnitude;

                if (distance < Globals.DISTANCE_FOR_USE)
                {
                    inputControl.ShowUseButton();
                }
                else
                {
                    inputControl.ShowHitButton();
                }
            }            
        }
    }
        

    public void UsePressed()
    {
        if (currentAim == null) return;
        

        if (currentAim.TryGetComponent(out Asset asset))
        {
            float distance = (transform.position - currentAim.transform.position).magnitude;

            if (distance <= Globals.DISTANCE_FOR_USE)
            {
                if (Asset.IsChop(asset.AssetType))
                {
                    lookAt(asset.gameObject.transform.position);
                    UseHit(HitType.Chop);
                }
                else if (Asset.IsCollect(asset.AssetType))
                {
                    lookAt(asset.gameObject.transform.position);
                    UseCollect(asset);
                }
                else if (Asset.IsMine(asset.AssetType))
                {
                    lookAt(asset.gameObject.transform.position);
                    UseHit(HitType.Mine);
                }
                else if (Asset.IsPickUp(asset.AssetType))
                {
                    lookAt(asset.gameObject.transform.position);
                    UseCollect(asset);
                }
                else if (Asset.IsAnyhit(asset.AssetType))
                {
                    lookAt(asset.gameObject.transform.position);
                    UseHit(HitType.Any);
                }
            }

            
        }
        

    }

    private void lookAt(Vector3 point)
    {
        Vector3 toLookAt = new Vector3(point.x, playerControl.transform.position.y, point.z);
        playerControl.transform.DOLookAt(toLookAt, 0.2f).SetEase(Ease.Linear);
    }

    public void UseHit(HitType _type)
    {
        playerControl.SetHit(_type);
    }

    public void UseCollect(Asset asset)
    {        
        playerControl.ActivateCollect?.Invoke(asset);
    }
}

public enum BodyLevel
{
    None,
    Low,
    Medium,
    High
}
