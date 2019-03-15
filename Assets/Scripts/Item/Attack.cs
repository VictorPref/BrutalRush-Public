using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Item
{

    public static int TIME_ITEM_DAMAGE = 5;

    // Update is called once per frame
    override
   public void UpdateItem()
    {

            RaycastHit2D hit = Physics2D.CircleCast(item.transform.position, CIRCLE_CAST, Vector2.zero, 0, layerMask);

            if (hit)
            {
                PlayerManager.Instance.getPlayerFromGameObject(hit.transform.parent.gameObject).character.GotItemDamage();
                Factory.Instance.pool.AddItemInPool(type, this);

            }
    }


}
