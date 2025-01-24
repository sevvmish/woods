using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    [SerializeReference] private Recipe[] recipes;

    private Dictionary<SkillTypes, List<Recipe>> recipesBySkill = new Dictionary<SkillTypes, List<Recipe>>();
    private Dictionary<int, Recipe> recipeByID = new Dictionary<int, Recipe>();

    private void Awake()
    {
        for (int i = 0; i < recipes.Length; i++)
        {
            if (recipes[i] == null) continue;

            if (recipesBySkill.ContainsKey(recipes[i].SkillNeeded))
            {
                recipesBySkill[recipes[i].SkillNeeded].Add(recipes[i]);
            }
            else
            {
                recipesBySkill.Add(recipes[i].SkillNeeded, new List<Recipe>() { recipes[i] });
            }

            if (recipeByID.ContainsKey(recipes[i].ID))
            {
                Debug.LogError("too many such recipe ID as: " + recipes[i].ID);
            }
            else
            {
                recipeByID.Add(recipes[i].ID, recipes[i]);
            }
        }
    }

    public List<Recipe> GetAllRecipesBySkill(SkillTypes _type)
    {
        if (recipesBySkill.ContainsKey(_type))
        {
            return recipesBySkill[_type];
        }
        else
        {
            Debug.LogError("no such skill in recipes: " + _type);
            return null;
        }
    }

    public Recipe GetRecipe(int id)
    {
        if (recipeByID.ContainsKey(id))
        {
            return recipeByID[id];
        }
        else
        {
            Debug.LogError("no such recipe ID: " + id);
            return null;
        }
    }
}
