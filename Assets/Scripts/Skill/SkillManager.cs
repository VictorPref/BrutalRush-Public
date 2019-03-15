using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class SkillManager
{

    private static SkillManager instance = null;

    private SkillManager()
    {
        skills = new List<Skill>();

    }

    public static SkillManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SkillManager();
            }
            return instance;
        }
    }

    List<Skill> skills;

    public void spawnSkill(FactorySkill skill, Vector2 pos, float skillOverpower, int idCharacter)
    {
        skills.Add(Factory.Instance.spawnSkill(skill, pos, skillOverpower, idCharacter));
    }

    public void Update()
    {
        foreach (Skill s in skills)
        {
            s.UpdateSkill();
        }
    }
}
