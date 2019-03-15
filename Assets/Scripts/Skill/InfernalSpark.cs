﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfernalSpark : Skill
{
    const float TIME_FIRE = 3;
    const float DEFAULT_FIRE_DAMAGE_OVER_TIME = 15;

    public float fireDamageOverTime;


    override
    public void UpdateSkill()
    {
        RaycastHit2D[] hits;
        if (skill)
        {
            if (skill.activeSelf)
            {
                hits = Physics2D.CircleCastAll(skill.transform.position, CIRCLE_CAST, Vector2.zero, 0, layerMask);

                if (hits.Length > 0)
                {
                    fireDamageOverTime = DEFAULT_FIRE_DAMAGE_OVER_TIME + (DEFAULT_FIRE_DAMAGE_OVER_TIME * overpower);
                    foreach (RaycastHit2D hit in hits)
                    {
                        Character c = PlayerManager.Instance.getPlayerFromGameObject(hit.transform.parent.gameObject).character;
                        if (c.id != idCharacter)
                        {
                            if (!c.gotSkillDamage)
                            {
                                Debug.Log("Player " + c.id + " get Fire damage");
                                c.TakeHealthDamageOverTime(fireDamageOverTime, TIME_FIRE);
                                c.gotSkillDamage = true;
                                charactersHits.Add(c);
                            }
                        }
                    }

                }
            }

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Infernal Spark"))
            {
                isPlaying = true;
            }

            if (isPlaying && anim.GetCurrentAnimatorStateInfo(0).IsName("Blank"))
            {
                Factory.Instance.pool.AddSkillInPool(type, this);
                foreach (Character c in charactersHits)
                {
                    c.gotSkillDamage = false;
                }
                charactersHits.Clear();
            }
        }
    }
}