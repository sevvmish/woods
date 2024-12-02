using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item1", menuName = "Items", order = 1)]
public class Item : ScriptableObject
{
    public int ID;
    public ItemsQuality Quality;
    public string NameRus;
    public string NameEng;
    public string DescriptionRus;
    public string DescriptionEng;
    public ItemTypes ItemType;
    public Sprite UISprite;
    public int MaxStack;
    public int AssetID;

    public static bool IsItemWeapon(int ID)
    {
        HashSet<int> IDsForWeapons = new HashSet<int>() { 2 };

        return IDsForWeapons.Contains(ID);
    }

    public override bool Equals(object obj)
    {
        return obj is Item item &&
               base.Equals(obj) &&
               ID == item.ID;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), ID);
    }
}

public enum ItemTypes
{
    None,
    Wood_simple,
    Axe1H,
    Stone,
    Mushroom,
    Axe2H,
    Pickaxe1H,
    Pickaxe2H,
    Flare1H
}

public enum ItemsQuality
{
    common,//обычный
    good,//хороший
    perfect,//отличный
    super,//супер
    legendary//легендарный
}
