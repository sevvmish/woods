using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

public class Inventory : MonoBehaviour
{
    [Inject] private ItemManager itemManager;
    [Inject] private InventoryInformerUI inventoryInformerUI;

    public Action OnInventoryChanged;
    public Action RightHandEquipDurability;

    public Dictionary<int, InventoryPosition> MainInventory { get; private set; }
    private const int maxLimit = 32;
    private int rightHandIndex = -10;

    private void Awake()
    {
        MainInventory = new Dictionary<int, InventoryPosition>();
        RightHandEquipDurability = changeDurabilityRightHand;

        for (int i = 0; i < maxLimit; i++)
        {            
            MainInventory.Add(i, new InventoryPosition(Globals.MainPlayerData.Inv[i, 0], Globals.MainPlayerData.Inv[i, 1]));
        }
        /*
        if (Globals.MainPlayerData.Equip.Length > 0)
        {
            for (global::System.Int32 i = 0; i < Globals.MainPlayerData.Equip.Length; i++)
            {
                MainInventory[Globals.MainPlayerData.Equip[i]].SetInUse(true);
            }
        }
        */
        if (Globals.MainPlayerData.Dur.GetLength(0) > 0)
        {
            for (global::System.Int32 i = 0; i < Globals.MainPlayerData.Dur.GetLength(0); i++)
            {
                MainInventory[Globals.MainPlayerData.Dur[i, 0]].SetDurability(Globals.MainPlayerData.Dur[i, 1]);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            for (int i = 0; i < maxLimit; i++)
            {
                if (MainInventory[i].ItemID > 0) print(i + ": " + MainInventory[i].ItemID + ", amount - " + MainInventory[i].Amount);
            }
            print("=================");
        }
    }

    public void ReplaceIndex(int from, int to)
    {
        if (from == to) return;

        InventoryPosition fromItem = MainInventory[from];
        InventoryPosition toItem = MainInventory[to];

        //print("from: index - " + from + ", ID - " + fromItem.ItemID + ", amount - " + fromItem.Amount);
        //print("from: index - " + to + ", ID - " + toItem.ItemID + ", amount - " + toItem.Amount);

        if (fromItem.ItemID <= 0) return;
        
        if (toItem.ItemID <= 0)
        {
            MainInventory[to] = new InventoryPosition(fromItem.ItemID, fromItem.Amount, fromItem.IsEquiped, fromItem.Durability);
            MainInventory[from] = new InventoryPosition(0, 0, false, 0);
        }
        else if (toItem.ItemID > 0 && fromItem.ItemID > 0 )
        {
            if (toItem.ItemID != fromItem.ItemID || (toItem.ItemID == fromItem.ItemID && itemManager.GetItemByID(toItem.ItemID).MaxStack == 1))
            {
                MainInventory[to] = new InventoryPosition(fromItem.ItemID, fromItem.Amount, fromItem.IsEquiped, fromItem.Durability);
                MainInventory[from] = new InventoryPosition(toItem.ItemID, toItem.Amount, toItem.IsEquiped, toItem.Durability);
            }
            else if (toItem.ItemID == fromItem.ItemID && itemManager.GetItemByID(toItem.ItemID).MaxStack > 1 && toItem.Amount < itemManager.GetItemByID(toItem.ItemID).MaxStack)
            {
                int fromAmount = fromItem.Amount;
                int toAmount = toItem.Amount;

                int toAmountMaxCapacity = itemManager.GetItemByID(toItem.ItemID).MaxStack - toAmount;

                if (toAmountMaxCapacity >= fromAmount)
                {
                    MainInventory[to] = new InventoryPosition(toItem.ItemID, toAmount + fromAmount, toItem.IsEquiped, toItem.Durability);
                    MainInventory[from] = new InventoryPosition(0, 0, false, 0);
                }
                else
                {
                    MainInventory[to] = new InventoryPosition(toItem.ItemID, toAmount + toAmountMaxCapacity, toItem.IsEquiped, toItem.Durability);
                    MainInventory[from] = new InventoryPosition(fromItem.ItemID, fromAmount - toAmountMaxCapacity, fromItem.IsEquiped, fromItem.Durability);
                }
            }            
        }

        OnInventoryChanged?.Invoke();
    }

    public Item GetAnyAxeFromInventory()
    {
        HashSet<ItemTypes> axes = new HashSet<ItemTypes>() { ItemTypes.Axe1H, ItemTypes.Axe2H };

        for (int i = 0; i < maxLimit; i++)
        {
            if (MainInventory[i].ItemID <= 0) continue;

            Item item = itemManager.GetItemByID(MainInventory[i].ItemID);
            if (item != null && axes.Contains(item.ItemType))
            {
                return itemManager.GetItemByID(MainInventory[i].ItemID);
            }
        }

        return null;
    }

    public Item GetAnyPickaxeFromInventory()
    {
        HashSet<ItemTypes> pickaxes = new HashSet<ItemTypes>() { ItemTypes.Pickaxe1H, ItemTypes.Pickaxe2H };

        for (int i = 0; i < maxLimit; i++)
        {
            if (MainInventory[i].ItemID <= 0) continue;

            Item item = itemManager.GetItemByID(MainInventory[i].ItemID);
            if (item != null && pickaxes.Contains(item.ItemType))
            {
                return itemManager.GetItemByID(MainInventory[i].ItemID);
            }
        }

        return null;
    }

    public bool TryAddItem(int id, int amount)
    {
        if (id < 1) return false;

        Item item = itemManager.GetItemByID(id);

        if (item.MaxStack > 1)
        {
            int existingKey = getKeyOfSameStackableItem(id);
            if (existingKey != -1)
            {
                int diff = item.MaxStack - MainInventory[existingKey].Amount;
                if (diff >= amount)
                {
                    MainInventory[existingKey].AddAmount(amount);
                    inventoryInformerUI.AddItemInfo(id, amount);
                    OnInventoryChanged?.Invoke();
                    return true;
                }      
                else
                {
                    MainInventory[existingKey].AddAmount(diff);
                    return TryAddItem(id, amount - diff);
                }
            }
        }

        int index = getKeyWithEmpty();

        if (index == -1)
        {
            return false;
        }
        else
        {
            MainInventory[index] = new InventoryPosition(id, amount, false, item.MaxDurability);
            inventoryInformerUI.AddItemInfo(id, amount);
            OnInventoryChanged?.Invoke();
            return true;
        }
    }

    public void EquipItemRightHand(int index, Item item)
    {
        for (int i = 0; i < maxLimit; i++)
        {
            if (i == index) continue;

            if (MainInventory[i].ItemID > 0 && Item.IsEquipRightHand(itemManager.GetItemByID(MainInventory[i].ItemID).ItemType))
            {
                MainInventory[i].SetInUse(false);
            }
        }

        MainInventory[index].SetInUse(true);
        rightHandIndex = index;
        OnInventoryChanged?.Invoke();
    }

    public void EquipItemRightHand(Item item)
    {
        int index = -1;

        for (int i = 0; i < maxLimit; i++)
        {            
            if (MainInventory[i].ItemID > 0 && Item.IsEquipRightHand(itemManager.GetItemByID(MainInventory[i].ItemID).ItemType))
            {
                MainInventory[i].SetInUse(false);
            }

            if (MainInventory[i].ItemID > 0 && index <=0 && item.ID == MainInventory[i].ItemID)
            {
                index = i;
            }
        }

        MainInventory[index].SetInUse(true);
        rightHandIndex = index;
        OnInventoryChanged?.Invoke();
    }

    private void changeDurabilityRightHand()
    {
        if (rightHandIndex < 0) return;

        if (MainInventory[rightHandIndex].ItemID > 0 && Item.IsEquipRightHand(itemManager.GetItemByID(MainInventory[rightHandIndex].ItemID).ItemType))
        {
            int value = MainInventory[rightHandIndex].Durability - 1;
            MainInventory[rightHandIndex].SetDurability(value);
        }
    }

    private int getKeyWithEmpty()
    {
        for (int i = 0; i < maxLimit; i++)
        {
            if (MainInventory[i].ItemID < 1) return i;
        }

        return -1;
    }

    private int getKeyOfSameStackableItem(int id)
    {
        if (itemManager.GetItemByID(id).MaxStack <= 1) return -1;

        for (int i = 0; i < maxLimit; i++)
        {
            if (MainInventory[i].ItemID < 1) continue;
            Item item = itemManager.GetItemByID(MainInventory[i].ItemID);
            if (id == item.ID && item.MaxStack > MainInventory[i].Amount) return i;
        }

        return -1;
    }
}


public class InventoryPosition
{
    public int ItemID { get; private set; }
    public int Amount { get; private set; }
    public bool IsEquiped { get; private set; }
    public int Durability { get; private set; }

    public int SetDurability(int value) => Durability = value;

    public InventoryPosition(int itemID, int amount)
    {
        ItemID = itemID;
        Amount = amount;
        IsEquiped = false;
        Durability = 0;
    }
    public InventoryPosition(int itemID, int amount, bool isEquiped, int durability)
    {
        ItemID = itemID;
        Amount = amount;
        IsEquiped = isEquiped;
        Durability = durability;
    }

    public void AddAmount(int toAdd) => Amount += toAdd;
    public void SetInUse(bool isEquiped) => IsEquiped = isEquiped;
}
