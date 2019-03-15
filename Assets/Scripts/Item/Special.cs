using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Special : Item
{
    public static float TIME_ITEM_SPECIAL = 2;

    // Update is called once per frame
    override
   public void UpdateItem()
    {

        RaycastHit2D hit = Physics2D.CircleCast(item.transform.position, CIRCLE_CAST, Vector2.zero, 0, layerMask);

        if (hit)
        {
            PlayerManager.Instance.getPlayerFromGameObject(hit.transform.parent.gameObject).character.GotItemSpecial();
            Factory.Instance.pool.AddItemInPool(type, this);

        }
    }
}
