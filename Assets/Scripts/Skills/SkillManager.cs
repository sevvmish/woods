using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeReference] private Skill[] skills;

    private Dictionary<SkillTypes, Skill> skillsInGroup = new Dictionary<SkillTypes, Skill>();
    private Dictionary<int, Skill> skillsByID = new Dictionary<int, Skill>();

    private void Awake()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i] == null) continue;

            if (skillsInGroup.ContainsKey(skills[i].CurrentSkill))
            {
                Debug.LogError("too many such skill types as: " + skills[i].CurrentSkill);
            }
            else
            {
                skillsInGroup.Add(skills[i].CurrentSkill, skills[i]);
            }

            if (skillsByID.ContainsKey(skills[i].ID))
            {
                Debug.LogError("too many such skill ID as: " + skills[i].ID);
            }
            else
            {
                skillsByID.Add(skills[i].ID, skills[i]);
            }
        }
    }

    public Skill GetSkill(SkillTypes _type)
    {
        if (skillsInGroup.ContainsKey(_type))
        {
            return skillsInGroup[_type];
        }
        else
        {
            Debug.LogError("no such skill: " + _type);
            return null;
        }
    }

    public Skill GetSkill(int id)
    {
        if (skillsByID.ContainsKey(id))
        {
            return skillsByID[id];
        }
        else
        {
            Debug.LogError("no such skill ID: " + id);
            return null;
        }
    }
}
