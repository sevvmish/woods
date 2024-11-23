using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item1", menuName = "Items", order = 1)]
public class Item : ScriptableObject
{
    public int ID;
    public string NameRus;
    public string NameEng;
    public string DescriptionRus;
    public string DescriptionEng;
    public ItemTypes ItemType;
    public Sprite UISprite;

    public static bool IsItemWeapon(int ID)
    {
        HashSet<int> IDsForWeapons = new HashSet<int>() { 2 };

        return IDsForWeapons.Contains(ID);
    }

    
}

public enum ItemTypes
{
    None,
    Wood_simple,
    Axe
}
