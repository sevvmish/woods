using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenAimNamer : MonoBehaviour
{
    private static GameObject lastObject;
    private static Asset lastAsset;
    private static string lastName = "";
        
    public static string GetNameByAsset(GameObject g, out bool isCollectable, out bool isChopable, out bool isMinable)
    {
        string result = "";
        isCollectable = false;
        isChopable = false;
        isMinable = false;
                
        if (lastObject!= null && lastObject.Equals(g))
        {
            if (isCollect(lastAsset.AssetType))
            {
                isCollectable = true;
            }
            else if (isChop(lastAsset.AssetType))
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

            if (isCollect(asset.AssetType))
            {
                isCollectable = true;
            }
            else if (isChop(asset.AssetType))
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

    private static bool isCollect(AssetTypes _type)
    {
        switch (_type)
        {
            case AssetTypes.tree_small:
                return false;

            case AssetTypes.tree_hard:
                return false;

            default:
                return true;

        }
    }

    private static bool isChop(AssetTypes _type)
    {
        switch (_type)
        {
            case AssetTypes.tree_small:
                return true;

            case AssetTypes.tree_hard:
                return true;

            default:
                return false;

        }
    }

    private static string getName(AssetTypes _type)
    {
        Translation lang = Globals.Language;

        switch (_type)
        {
            case AssetTypes.tree_small:
                return lang.TreeSmall;

            case AssetTypes.tree_hard:
                return lang.TreeHard;                

            default:
                return "";

        }

    }
}
