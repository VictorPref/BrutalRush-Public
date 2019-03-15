using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Item
{
    const float TIME_BEFORE_EXPLODE = 1f;

    Animator anim;
    AnimatorStateInfo animState;
    bool explosion;
    bool isPlaying = false;
    bool damageDone = false;
    int layerBomb;
    float timeExplode;
    bool hitGround;

    Rigidbody2D rd;

    public void init()
    {
        anim = item.GetComponent<Animator>();
        layerBomb = layerMask | 1 << Character.LEVEL_LAYER;
 
        explosion = false;
        isPlaying = false;
        damageDone = false;
        hitGround = false;
        timeExplode = 0;
        anim.Rebind();
        rd = item.GetComponent<Rigidbody2D>();
        rd.bodyType = RigidbodyType2D.Dynamic;
    }

    // Update is called once per frame
    override
   public void UpdateItem()
    {

        RaycastHit2D hit = Physics2D.CircleCast(item.transform.position, CIRCLE_CAST, Vector2.zero, 0, layerBomb);

        if(hit && !hitGround)
        {
            timeExplode = Time.time + TIME_BEFORE_EXPLODE;
            hitGround = true;
        }
        if (hitGround &&timeExplode < Time.time && !explosion)
        {
            explosion = true;
            anim.SetTrigger("Explode");
            rd.bodyType = RigidbodyType2D.Static;

        }
        
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Explosion"))
        {
            isPlaying = true;
        }

        if (isPlaying && !damageDone)
        {
            
            RaycastHit2D[] hit1 = Physics2D.CircleCastAll(item.transform.position, CIRCLE_CAST_BOMB, Vector2.zero, 0, layerMask);

            if (hit1.Length > 0)
            {
                damageDone = true;
                for (int i = 0; i < hit1.Length; i++)
                {
                    Player p = PlayerManager.Instance.getPlayerFromGameObject(hit1[i].transform.parent.gameObject);
                    Debug.Log(p);
                    p.character.TakeDirectHealthDamage(20);
                }
            }
        }

        if (isPlaying && anim.GetCurrentAnimatorStateInfo(0).IsName("Blank"))
        {
            Factory.Instance.pool.AddItemInPool(FactoryItem.Bomb, this);
        }
    }
}
