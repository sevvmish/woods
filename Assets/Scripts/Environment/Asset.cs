using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asset : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private AssetTypes assetType;

    public int ID { get => id; }
    public AssetTypes AssetType { get => assetType; }


    private static GameObject lastObject;
    private static Asset lastAsset;
    private static string lastName = "";

    public static string GetNameByAsset(GameObject g, out bool isCollectable, out bool isChopable, out bool isMinable)
    {
        string result = "";
        isCollectable = false;
        isChopable = false;
        isMinable = false;

        if (lastObject != null && lastObject.Equals(g))
        {
            if (IsCollect(lastAsset.AssetType))
            {
                isCollectable = true;
            }
            else if (IsChop(lastAsset.AssetType))
            {
                isChopable = true;
            }
            return lastName;
        }

        if (g.TryGetComponent(out Asset asset))
        {
            lastObject = g;
            lastAsset = asset;
            lastName = getName(asset.AssetType);

            if (IsCollect(asset.AssetType))
            {
                isCollectable = true;
            }
            else if (IsChop(asset.AssetType))
            {
                isChopable = true;
            }

            return lastName;
        }

        return result;
    }


    public static string GetNameByAsset(GameObject g)
    {
        string result = "";

        if (lastObject != null && lastObject.Equals(g))
        {
            return lastName;
        }

        if (g.TryGetComponent(out Asset asset))
        {
            lastObject = g;
            lastAsset = asset;
            lastName = getName(asset.AssetType);

            return lastName;
        }

        return result;
    }

    public static bool IsCollect(AssetTypes _type)
    {
        switch (_type)
        {
            case AssetTypes.grass:
                return true;

            case AssetTypes.small_stone:
                return true;

            case AssetTypes.flowers:
                return true;

            case AssetTypes.branch_on_ground:
                return true;

            case AssetTypes.bush:
                return true;

            case AssetTypes.mushroom:
                return true;

            
            default:
                return false;

        }
    }

    public static BodyLevel GetBodyLevelByAsset(AssetTypes _type)
    {
        HashSet<AssetTypes> lows = new HashSet<AssetTypes>() { AssetTypes.mushroom, AssetTypes.flowers, AssetTypes.grass, AssetTypes.branch_on_ground, AssetTypes.small_stone };
        HashSet<AssetTypes> mediums = new HashSet<AssetTypes>();
        HashSet<AssetTypes> high = new HashSet<AssetTypes>();

        if (lows.Contains(_type))
        {
            return BodyLevel.Low;
        }
        else if (mediums.Contains(_type))
        {
            return BodyLevel.Medium;
        }
        else if (high.Contains(_type))
        {
            return BodyLevel.High;
        }
        else
        {
            return BodyLevel.None;
        }
    }

    public static bool IsChop(AssetTypes _type)
    {
        switch (_type)
        {
            case AssetTypes.tree_small:
                return true;

            case AssetTypes.tree_hard:
                return true;

            case AssetTypes.wood_log:
                return true;

            
            default:
                return false;

        }
    }

    public static bool IsMine(AssetTypes _type)
    {
        switch (_type)
        {
            case AssetTypes.medium_stone:
                return true;

     
            default:
                return false;

        }
    }

    public static string getName(AssetTypes _type)
    {
        Translation lang = Globals.Language;

        switch (_type)
        {
            case AssetTypes.tree_small:
                return lang.TreeSmall;

            case AssetTypes.tree_hard:
                return lang.TreeHard;

            case AssetTypes.branch_on_ground:
                return lang.Brunches;

            case AssetTypes.bush:
                return lang.Bushes;

            case AssetTypes.small_stone:
                return lang.Stone;

            case AssetTypes.stone:
                return lang.Stone;

            case AssetTypes.medium_stone:
                return lang.Stone;

            case AssetTypes.mushroom:
                return lang.Mushroom;

            case AssetTypes.campfire:
                return lang.Campfire;

            case AssetTypes.flowers:
                return lang.Flower;

            case AssetTypes.wood_log:
                return lang.WoodLog;

            default:
                return "";

        }

    }
}

public enum AssetTypes
{
    none,
    terrain_flat,
    tree_small,
    tree_hard,
    stone,
    rock,
    bush,
    grass,
    flowers,
    campfire,
    branch_on_ground,
    resource_wood_simple,
    small_stone,
    resource_stone_simple,
    resource_wood_hard,
    wood_log,
    mushroom,
    resource_mushroom,
    medium_stone,
    axe_1h,
    hammer_1h,
    pickaxe_1h,
    flare_1h

}
