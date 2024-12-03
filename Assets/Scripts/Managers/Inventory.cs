using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

public class Inventory : MonoBehaviour
{
    [Inject] private ItemManager itemManager;

    public InventoryPosition[] GetAllInventory => inventory.Values.ToArray();

    private Dictionary<int, InventoryPosition> inventory = new Dictionary<int, InventoryPosition>();
    private const int maxLimit = 32;

    private void Awake()
    {
        for (int i = 0; i < maxLimit; i++)
        {
            inventory.Add(i, new InventoryPosition(Globals.MainPlayerData.Inv[i, 1], Globals.MainPlayerData.Inv[i, 2]));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            for (int i = 0; i < maxLimit; i++)
            {
                if (inventory[i].ItemID > 0) print(i + ": " + inventory[i].ItemID + ", amount - " + inventory[i].Amount);
            }
            print("=================");
        }
    }

    public void ReplaceIndex(int from, int to)
    {
        if (from == to) return;

        InventoryPosition fromItem = inventory[from];
        InventoryPosition toItem = inventory[to];

        //print("from: index - " + from + ", ID - " + fromItem.ItemID + ", amount - " + fromItem.Amount);
        //print("from: index - " + to + ", ID - " + toItem.ItemID + ", amount - " + toItem.Amount);

        if (fromItem.ItemID <= 0) return;
        
        if (toItem.ItemID <= 0)
        {
            inventory[to] = new InventoryPosition(fromItem.ItemID, fromItem.Amount);
            inventory[from] = new InventoryPosition(0, 0);
        }
        else if (toItem.ItemID > 0 && fromItem.ItemID > 0 && toItem.ItemID != fromItem.ItemID)
        {
            inventory[to] = new InventoryPosition(fromItem.ItemID, fromItem.Amount);
            inventory[from] = new InventoryPosition(toItem.ItemID, toItem.Amount);
        }
    }

    public Item GetAnyAxeFromInventory()
    {
        HashSet<ItemTypes> axes = new HashSet<ItemTypes>() { ItemTypes.Axe1H, ItemTypes.Axe2H };

        for (int i = 0; i < maxLimit; i++)
        {
            Item item = itemManager.GetItemByID(inventory[i].ItemID);
            if (item != null && axes.Contains(item.ItemType))
            {
                return itemManager.GetItemByID(inventory[i].ItemID);
            }
        }

        return null;
    }

    public Item GetAnyPickaxeFromInventory()
    {
        HashSet<ItemTypes> pickaxes = new HashSet<ItemTypes>() { ItemTypes.Pickaxe1H, ItemTypes.Pickaxe2H };

        for (int i = 0; i < maxLimit; i++)
        {
            Item item = itemManager.GetItemByID(inventory[i].ItemID);
            if (item != null && pickaxes.Contains(item.ItemType))
            {
                return itemManager.GetItemByID(inventory[i].ItemID);
            }
        }

        return null;
    }

    public bool TryAddItem(int id, int amount)
    {
        if (id < 1) return true;

        Item item = itemManager.GetItemByID(id);

        if (item.MaxStack > 1)
        {
            int existingKey = getKeyOfSameStackableItem(id);
            if (existingKey != -1)
            {
                int diff = item.MaxStack - inventory[existingKey].Amount;
                if (diff >= amount)
                {
                    inventory[existingKey].AddAmount(amount);
                    return true;
                }      
                else
                {
                    inventory[existingKey].AddAmount(diff);
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
            inventory[index] = new InventoryPosition(id, amount);
            return true;
        }
    }

    private int getKeyWithEmpty()
    {
        for (int i = 0; i < maxLimit; i++)
        {
            if (inventory[i].ItemID < 1) return i;
        }

        return -1;
    }

    private int getKeyOfSameStackableItem(int id)
    {
        if (itemManager.GetItemByID(id).MaxStack <= 1) return -1;

        for (int i = 0; i < maxLimit; i++)
        {
            if (inventory[i].ItemID < 1) continue;
            Item item = itemManager.GetItemByID(inventory[i].ItemID);
            if (id == item.ID && item.MaxStack > inventory[i].Amount) return i;
        }

        return -1;
    }
}

public class InventoryPosition
{
    public int ItemID { get; private set; }
    public int Amount { get; private set; }

    public InventoryPosition(int itemID, int amount)
    {
        ItemID = itemID;
        Amount = amount;
    }

    public void AddAmount(int toAdd) => Amount += toAdd;
}
