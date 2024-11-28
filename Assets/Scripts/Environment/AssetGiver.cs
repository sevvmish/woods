using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AssetGiver : MonoBehaviour
{
    [SerializeField] private Transform locationForResource;
    [SerializeField] private GameObject VFX;
    [SerializeField] private AudioSource _audio;
    [SerializeField] private BoxCollider boxC;

    private GameObject resource;
    private bool isGiven;
    private AssetManager assets;
    private ObjectPool returnPool;

    private int itemID;
    private int amountOfItem;

    public void SetData(AssetManager a, int resourceID, int itemID, int amount, ObjectPool rp)
    {
        assets = a;
        returnPool = rp;

        amountOfItem = amount;
        this.itemID = itemID;

        VFX.SetActive(true);
        isGiven = false;
        boxC.enabled = false;

        resource = assets.GetAssetByID(resourceID);
        resource.SetActive(true);
        resource.transform.parent = locationForResource;
        resource.transform.localPosition = new Vector3(0, 0.5f, 0);
        resource.transform.localEulerAngles = Vector3.zero;
        locationForResource.DOKill();
        locationForResource.DOLocalRotate(new Vector3(0, 359, 0), 3, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);

        respawnDelay().Forget();
    }
    private async UniTaskVoid respawnDelay()
    {
        await UniTask.Delay(500);
        boxC.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isGiven && other.gameObject.layer == 3)
        {
            if (other.GetComponent<Inventory>().TryAddItem(itemID, amountOfItem))
            {
                isGiven = true;
                giveToPlayer(other.gameObject.transform).Forget();
            }
            else
            {
                print("ERROR trying to take resources");
            }
        }
    }

    private async UniTaskVoid giveToPlayer(Transform t)
    {
        boxC.enabled = false;

        transform.parent = t;
        transform.DOLocalMove(new Vector3(0, 2f, 0), 0.3f).SetEase(Ease.Linear);

        _audio.Play();        

        await UniTask.Delay(300);
        
        transform.DOLocalMove(new Vector3(0, 0, 0), 0.2f).SetEase(Ease.Linear);
        
        await UniTask.Delay(200);
        
        assets.ReturnAsset(resource);
        returnPool.ReturnObject(this.gameObject);
    }
}