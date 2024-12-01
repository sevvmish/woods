using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class EquipControl : MonoBehaviour
{
    [Inject] private AssetManager assetManager;
    [Inject] private ItemManager itemManager;

    private PlaceForWeapon placeForWeapon;

    public void SetPlaceForWeapon(PlaceForWeapon p) => placeForWeapon = p;

    public void EquipRightHand(Item item)
    {
        GameObject g = assetManager.GetAssetByID(item.AssetID);
        g.transform.parent = placeForWeapon.RightHand;
        g.transform.localPosition = Vector3.zero;
        g.transform.localEulerAngles = Vector3.zero;
        g.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            EquipRightHand(itemManager.GetItemByID(2));
        }
    }
}
