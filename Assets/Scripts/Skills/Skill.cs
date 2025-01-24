using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "skill1", menuName = "Skills", order = 1)]
public class Skill : ScriptableObject
{
    public int ID;
    public SkillTypes CurrentSkill;
    public int CurrentSkillLevel;
}

public enum SkillTypes
{
    None,
    Axes,
    WoodCutting,
    Crafting,
    Cooking
}