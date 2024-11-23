using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asset : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private AssetTypes assetType;

    public int ID { get => id; }
    public AssetTypes AssetType { get => assetType; }

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
    wood_log

}
