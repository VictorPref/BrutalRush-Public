using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Item
{

    public static float HEALTH_ITEM_VALUE = 15;

    // Update is called once per frame
    override
   public void UpdateItem()
    {

        RaycastHit2D hit = Physics2D.CircleCast(item.transform.position, CIRCLE_CAST, Vector2.zero, 0, layerMask);

        if (hit)
        {
            PlayerManager.Instance.getPlayerFromGameObject(hit.transform.parent.gameObject).character.GotItemHealth() ;
            Factory.Instance.pool.AddItemInPool(type, this);

        }
    }
}
