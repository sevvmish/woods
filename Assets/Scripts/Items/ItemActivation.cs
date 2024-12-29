using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

public class ItemActivation : MonoBehaviour
{
    [Inject] private ItemManager itemManager;
    [Inject] private Inventory inventory;
    [Inject] private EquipControl equip;
    [Inject] private Sounds sounds;

    private void Start()
    {
        if (Globals.MainPlayerData.Equip.Length > 0)
        {
            for (global::System.Int32 i = 0; i < Globals.MainPlayerData.Equip.Length; i++)
            {
                int id = inventory.MainInventory[Globals.MainPlayerData.Equip[i]].ItemID;

                if (id > 0 && Item.IsEquipRightHand(itemManager.GetItemByID(id).ItemType))
                {
                    ActivateItem(Globals.MainPlayerData.Equip[i]);
                }
            }
        }
    }

    public void ActivateItem(int index)
    {
        InventoryPosition[] items = inventory.MainInventory.Values.ToArray();

        if (items[index].ItemID < 1) return;

        Item item = itemManager.GetItemByID(items[index].ItemID);
        if (Item.IsEquipRightHand(item.ItemType))
        {
            equip.EquipRightHand(item);
            sounds.PlayGrabSound();
            inventory.EquipItemRightHand(index, item);            
        }
    }
}
