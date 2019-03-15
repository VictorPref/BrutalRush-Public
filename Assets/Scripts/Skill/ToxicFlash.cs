using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicFlash : Skill
{
    public float poisonHealthDamageOverTime;
    public float poisonEnergieDamageOverTime;

    const float DEFAULT_POISON_HEALT_DAMAGE_OVER_TIME = 8;
    const float DEFAULT_POISON_ENERGY_DAMAGE_OVER_TIME = 1;
    const float TIME_POISON = 3;

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
                    poisonHealthDamageOverTime = DEFAULT_POISON_HEALT_DAMAGE_OVER_TIME + (DEFAULT_POISON_HEALT_DAMAGE_OVER_TIME * overpower);
                    poisonEnergieDamageOverTime = DEFAULT_POISON_ENERGY_DAMAGE_OVER_TIME + (DEFAULT_POISON_ENERGY_DAMAGE_OVER_TIME * overpower);
                    foreach (RaycastHit2D hit in hits)
                    {
                        Character c = PlayerManager.Instance.getPlayerFromGameObject(hit.transform.parent.gameObject).character;
                        if (c.id != idCharacter)
                        {
                            if (!c.gotSkillDamage)
                            {
                                Debug.Log("Player " + c.id + " get Poison damage");
                                c.TakeHealthDamageOverTime(poisonHealthDamageOverTime, TIME_POISON);
                                c.TakeEnergieDamageOverTime(poisonEnergieDamageOverTime, TIME_POISON);
                                c.gotSkillDamage = true;
                                charactersHits.Add(c);
                            }
                        }
                    }
                }
            }

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Toxic Flash"))
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
