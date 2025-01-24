using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "recipe1", menuName = "Recipes", order = 1)]
public class Recipe : ScriptableObject
{
    public int ID;

    public RecipeGroup[] Ingredients;
    public RecipeGroup[] Results;

    public SkillTypes SkillNeeded;
    public int LevelOfSkillNeeded;
}