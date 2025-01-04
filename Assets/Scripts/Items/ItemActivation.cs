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
    [Inject] private GameplayInformerUI gameplayInformerUI;

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

    public bool TryPutAnythingToChop()
    {
        if (equip.RightHandItem == null || (equip.RightHandItem.ItemType != ItemTypes.Axe1H && equip.RightHandItem.ItemType != ItemTypes.Axe2H))
        {
            Item item = inventory.GetAnyAxeFromInventory();
            if (item != null)
            {
                ActivateItem(getIndexByItem(item));
                return true;
            }
            else
            {
                gameplayInformerUI.YouNeedAxe();
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public bool TryPutAnythingToMine()
    {
        if (equip.RightHandItem == null || (equip.RightHandItem.ItemType != ItemTypes.Pickaxe1H && equip.RightHandItem.ItemType != ItemTypes.Pickaxe2H))
        {
            Item item = inventory.GetAnyPickaxeFromInventory();
            if (item != null)
            {
                ActivateItem(getIndexByItem(item));
                return true;
            }
            else
            {
                gameplayInformerUI.YouNeedPickaxe();
                return false;
            }
        }
        else
        {
            return false;
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

    public void ActivateItem(Item item)
    {
        InventoryPosition[] items = inventory.MainInventory.Values.ToArray();

        if (item == null) return;
                
        if (Item.IsEquipRightHand(item.ItemType))
        {
            equip.EquipRightHand(item);
            sounds.PlayGrabSound();
            inventory.EquipItemRightHand(item);
        }
    }

    private int getIndexByItem(Item item)
    {
        InventoryPosition[] items = inventory.MainInventory.Values.ToArray();

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].ItemID > 0 && itemManager.GetItemByID(items[i].ItemID).ItemType == item.ItemType)
            {
                return i;
            }
        }

        return -1;
    }
}
