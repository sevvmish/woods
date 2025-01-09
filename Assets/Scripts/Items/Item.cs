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
    public float Damage;
    public int MaxDurability;

    public static bool IsItemWeapon(int ID)
    {
        HashSet<int> IDsForWeapons = new HashSet<int>() { 2 };

        return IDsForWeapons.Contains(ID);
    }

    public static string QualityText(ItemsQuality quality)
    {
        switch(quality)
        {
            case ItemsQuality.common:
                return Globals.Language.Common;

            case ItemsQuality.good:
                return Globals.Language.Good;

            case ItemsQuality.perfect:
                return Globals.Language.Perfect;

            case ItemsQuality.super:
                return Globals.Language.Super;

            case ItemsQuality.legendary:
                return Globals.Language.Legendary;

            default:
                return "";
        }
    }

    public static bool IsItemForChop(Item item)
    {
        if (item.ItemType == ItemTypes.Axe1H || item.ItemType == ItemTypes.Axe2H)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsItemForMine(Item item)
    {
        if (item.ItemType == ItemTypes.Pickaxe1H || item.ItemType == ItemTypes.Pickaxe2H)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsEquipRightHand(ItemTypes _type)
    {
        HashSet<ItemTypes> set = new HashSet<ItemTypes>() {ItemTypes.Axe1H, ItemTypes.Axe2H, ItemTypes.Pickaxe1H, ItemTypes.Pickaxe2H, ItemTypes.Flare1H };

        return set.Contains(_type);
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
    Flare1H,
    Fibres,
    Cabbage,
    Raspberry,
    Carrot
}

public enum ItemsQuality
{
    common,//обычный
    good,//хороший
    perfect,//отличный
    super,//супер
    legendary//легендарный
}
