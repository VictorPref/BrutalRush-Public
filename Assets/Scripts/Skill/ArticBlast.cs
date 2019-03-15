using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArticBlast : Skill
{
    const float SLOW_VELOCITY_VALUE = 150;
    const float SLOW_TIME = 3;
    const float DEFAULT_DAMAGE = 25;

    float damage;

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
                    damage = DEFAULT_DAMAGE + (DEFAULT_DAMAGE * overpower);

                    foreach (RaycastHit2D hit in hits)
                    {

                        Character c = PlayerManager.Instance.getPlayerFromGameObject(hit.transform.parent.gameObject).character;
                        if (c.id != idCharacter)
                        {
                            if (!c.gotSkillDamage)
                            {
                                Debug.Log("Player " + c.id + " get Ice damage");
                                c.TakeDirectHealthDamage(damage);
                                c.GetSlowEffect(SLOW_VELOCITY_VALUE, SLOW_TIME);
                                c.gotSkillDamage = true;
                                charactersHits.Add(c);
                            }
                        }
                    }
                }
            }

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Artic Blast"))
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
