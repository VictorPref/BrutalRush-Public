using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    #region Const
    const int DEFAULT_TIME_SPAWN_BOMB = 6;
    const int QUICK_TIME_SPAWN_BOMB = 3;
    const int DEFAULT_TIME_BOMB = 600;
    const int QUICK_TIME_BOMB = 120;
    const int ITEM_ATTACK_RANDOM_VALUE = 25;
    const int ITEM_ENERGY_RANDOM_VALUE = 50;
    const int ITEM_HEALTH_RANDOM_VALUE = 75;
    const int ITEM_SPECIAL_RANDOM_VALUE = 100;
    const int RANDOM_MIN_RANGE = 1;
    const int RANDOM_MAX_RANGE = 100;
    const int RANDOM_TIME_MIN_RANGE = 2;
    const int RANDOM_TIME_MAX_RANGE = 5;
    #endregion

    private static ItemManager instance = null;

    private ItemManager()
    {
    }

    public static ItemManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ItemManager();
            }
            return instance;
        }
    }

    float timeBetweenSpawn;
    float timeBomb;
    float timeSpawnBomb;
    Vector2 limit1;
    Vector2 limit2;
    List<Item> items;
    List<bool> active;

    // Start is called before the first frame update
    public void Start(Vector2 pos1, Vector2 pos2)
    {

        limit1 = pos1;
        limit2 = pos2;
        items = new List<Item>();
        RandomTime();
        timeSpawnBomb = Time.time + DEFAULT_TIME_SPAWN_BOMB;
        initActive();

        timeBomb = 0;
    }

    // Update is called once per frame
    public void Update()
    {
        timeBomb += Time.deltaTime;
        if (timeBetweenSpawn < Time.time)
        {
            RandomItem();
            RandomTime();
        }

        if (timeSpawnBomb < Time.time)
        {
            spawnBomb();
        }


        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].deleted)
            {
                items[i].Update();
            }
            else
            {
                items[i].UpdateItem();
                items[i].UpdateInScreen();

            }
        }




        if (Input.GetKeyDown(KeyCode.F))
        {
            timeBomb = 99999;
            timeSpawnBomb = 0;
        }

    }

    public void spawnBomb()
    {
        Debug.Log(active[4]);
        if (active[4])
        {
            if (timeBomb < QUICK_TIME_BOMB)
            {
                timeSpawnBomb = Time.time + QUICK_TIME_SPAWN_BOMB;
            }
            else if (timeBomb < DEFAULT_TIME_BOMB)
            {
                timeSpawnBomb = Time.time + DEFAULT_TIME_SPAWN_BOMB;
            }
            Vector2 pos = new Vector2(Random.Range(limit1.x, limit2.x), Random.Range(limit1.y, limit2.y));
            items.Add(Factory.Instance.spawnItem(FactoryItem.Bomb, pos));
        }
    }

    public void RandomItem()
    {
        int random = Random.Range(RANDOM_MIN_RANGE, RANDOM_MAX_RANGE);
        Vector2 pos = new Vector2(Random.Range(limit1.x, limit2.x), Random.Range(limit1.y, limit2.y));

        if (random < ITEM_ATTACK_RANDOM_VALUE && active[0])
        {
            items.Add(Factory.Instance.spawnItem(FactoryItem.atk, pos));
        }
        else if (random < ITEM_ENERGY_RANDOM_VALUE && active[1])
        {
            items.Add(Factory.Instance.spawnItem(FactoryItem.Energy, pos));
        }
        else if (random < ITEM_HEALTH_RANDOM_VALUE && active[2])
        {
            items.Add(Factory.Instance.spawnItem(FactoryItem.Health, pos));
        }
        else if (random < ITEM_SPECIAL_RANDOM_VALUE && active[3])
        {
            items.Add(Factory.Instance.spawnItem(FactoryItem.Special, pos));
        }

    }

    public void RandomTime()
    {
        timeBetweenSpawn = Random.Range(RANDOM_TIME_MIN_RANGE, RANDOM_TIME_MAX_RANGE);
        timeBetweenSpawn += Time.time;
    }

    public void initActive()
    {
        if (active == null)
        {
            active = new List<bool>();
            for (int i = 0; i < 5; i++)
            {
                active.Add(true);
            }
        }
    }

    public List<bool> GetItemActive()
    {
        return active;
    }
    public void setItemActive(List<bool> ac)
    {
        active = ac;
    }

    public void DeleteAllItems()
    {
        for(int i = 0; i < items.Count; i++)
        {
            items[i].delete();
        }
    }
}
