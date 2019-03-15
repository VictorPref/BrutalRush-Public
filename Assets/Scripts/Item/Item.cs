using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{

    public const float CIRCLE_CAST = 0.5f;
    public const float CIRCLE_CAST_BOMB = 1;

    public float timeDeleted;
    public bool deleted = false;
    public GameObject item;
    public int layerMask;
    public FactoryItem type;
    SpriteRenderer renderer;

    public virtual void Start(string path , Vector2 pos, FactoryItem itemType)
    {
        item = GameObject.Instantiate(Resources.Load(path)) as GameObject; ;
        initialization(pos);
        layerMask = 1 << Character.CHARACTER_LAYER;
        type = itemType;
        renderer = item.GetComponent<SpriteRenderer>();
    }

    public virtual void UpdateItem()
    {
       
    }
    public void UpdateInScreen()
    {
        if (!renderer.IsVisibleFrom(Camera.main))
        {
            Factory.Instance.pool.AddItemInPool(type, this);
        }
    }

    public void Update()
    {
        timeDeleted += Time.deltaTime;
    }

    public  void initialization(Vector2 pos)
    {
        deleted = false;
        item.SetActive(true);
        timeDeleted = 0;
        item.transform.position = pos;
    }

    public  void deleteGameobject()
    {
        GameObject.Destroy(item);
    }

    public  void delete()
    {
        item.SetActive(false);
        deleted = true;
    }
}
