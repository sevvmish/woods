using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class Inventory : MonoBehaviour
{
    [Inject] private ItemManager itemManager;

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
