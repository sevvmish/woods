using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Translations", menuName = "Languages", order = 1)]
public class Translation : ScriptableObject
{   
    public string W;
    public string A;
    public string S;
    public string D;

    public string Attack;
    public string Block;
    public string Jump;
    public string Inventory;
    public string Menu;
    public string Crafting;
    public string Map;

    [Header("UIs")]
    public string QuickBar;

    [Header("Qualities")]
    public string Common;
    public string Good;
    public string Perfect;
    public string Super;
    public string Legendary;


    public string Collect;
    public string Chop;
    public string Mine;
    public string E;

    [Header("Base informers")]
    public string YouNeedAxe;
    public string YouNeedBetterAxe;
    public string YouNeedPickaxe;
    public string YouNeedBetterPickaxe;


    [Header("Assets")]
    public string TreeSmall;
    public string TreeHard;
    public string Brunches;
    public string Bushes;
    public string Stone;
    public string Mushroom;
    public string Flower;
    public string WoodLog;
    public string Campfire;
    public string Hen;
    public string Raspberry;
    public string Cabbage;
    public string Carrot;
    public string Boar;


    [Header("Items")]
    public ItemTranslation[] ItemsTranslation;

    [Header("Skills")]
    public ItemTranslation[] SkillsTranslation;

    [Header("Recipes")]
    public ItemTranslation[] RecipesTranslation;



    public Translation() { }
}

[Serializable]
public struct ItemTranslation
{
    public int ID;
    [TextArea]
    public string Name;
    [TextArea]
    public string Description;
}
