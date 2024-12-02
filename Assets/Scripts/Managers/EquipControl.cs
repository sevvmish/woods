using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class EquipControl : MonoBehaviour
{
    [Inject] private AssetManager assetManager;
    [Inject] private ItemManager itemManager;

    private PlaceForWeapon placeForWeapon;

    public Item RightHandItem { get; private set; }
    private GameObject rightHandObject;
    public Item LeftHandItem { get; private set; }
    private GameObject leftHandObject;

    public void SetPlaceForWeapon(PlaceForWeapon p) => placeForWeapon = p;

    public void EquipRightHand(Item item)
    {
        if (RightHandItem != null)
        {
            assetManager.ReturnAsset(rightHandObject);
        }

        GameObject g = assetManager.GetAssetByID(item.AssetID);
        g.transform.parent = placeForWeapon.RightHand;
        g.transform.localPosition = Vector3.zero;
        g.transform.localEulerAngles = Vector3.zero;
        g.SetActive(true);
        rightHandObject = g;

        RightHandItem = item;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            EquipRightHand(itemManager.GetItemByID(6));
        }
    }
}
