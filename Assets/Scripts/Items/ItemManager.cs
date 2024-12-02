using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private Item[] items;

    public Item GetItemByID(int ID)
    {        
        if (ID <= 0)
        {
            print("wrong ID number: " + ID);
            return null;
        }

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].ID == ID) return items[i];
        }
        print("no such itemID: " + ID);
        throw new System.NotImplementedException();
    }
}
