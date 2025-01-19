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
            else if (IsMine(lastAsset.AssetType))
            {
                isMinable = true;
            }
                        
            return lastName;
        }

        if (g.TryGetComponent(out Asset asset))
        {
            lastObject = g;
            lastAsset = asset;
            lastName = GetName(asset.AssetType);
            

            if (IsCollect(asset.AssetType))
            {
                isCollectable = true;
            }
            else if (IsChop(asset.AssetType))
            {
                isChopable = true;
            }
            else if (IsMine(asset.AssetType))
            {
                isMinable = true;
            }

            return lastName;
        }
        else if (g.transform.parent.TryGetComponent(out Asset asset1))
        {
            lastObject = g;
            lastAsset = asset1;
            lastName = GetName(asset1.AssetType);


            if (IsCollect(asset1.AssetType))
            {
                isCollectable = true;
            }
            else if (IsChop(asset1.AssetType))
            {
                isChopable = true;
            }
            else if (IsMine(asset1.AssetType))
            {
                isMinable = true;
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
            lastName = GetName(asset.AssetType);

            return lastName;
        }
        else if (g.transform.parent.TryGetComponent(out Asset asset1))
        {
            lastObject = g;
            lastAsset = asset1;
            lastName = GetName(asset1.AssetType);

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
                            
            case AssetTypes.mushroom:
                return true;

            case AssetTypes.cabbage:
                return true;

            case AssetTypes.carrot:
                return true;


            default:
                return false;

        }
    }

    public static bool IsPickUp(AssetTypes _type)
    {
        switch (_type)
        {
            case AssetTypes.raspberry:
                return true;
          
            default:
                return false;

        }
    }

    public static BodyLevel GetBodyLevelByAsset(AssetTypes _type)
    {
        HashSet<AssetTypes> lows = new HashSet<AssetTypes>() { AssetTypes.mushroom, AssetTypes.flowers, AssetTypes.grass, AssetTypes.branch_on_ground, AssetTypes.small_stone };
        HashSet<AssetTypes> mediums = new HashSet<AssetTypes>() {AssetTypes.raspberry };
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

    public static bool IsAnyhit(AssetTypes _type)
    {
        switch (_type)
        {
            case AssetTypes.bush:
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

    public static string GetName(AssetTypes _type)
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

            case AssetTypes.hen_simple:
                return lang.Hen;

            case AssetTypes.raspberry:
                return lang.Raspberry;

            case AssetTypes.cabbage:
                return lang.Cabbage;

            case AssetTypes.carrot:
                return lang.Carrot;

            case AssetTypes.boar_simple:
                return lang.Boar;

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
    flare_1h,
    hen_simple,
    resource_fibres,
    resource_berries,
    resource_cabbage,
    raspberry,
    resource_raspberry,
    bush_for_raspberry,
    cabbage,
    carrot,
    resource_carrot,
    boar_simple

}
