using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool  {

    Dictionary<FactoryItem, List<Item>> poolItem;
    Dictionary<FactorySkill, List<Skill>> poolSkill;
    float timeForDelete = 60;
    List<Item> itemToDelete;
    List<Skill> skillToDelete;

    // Use this for initialization
    public void Start()
    {
        poolItem = new Dictionary<FactoryItem, List<Item>>();
        poolSkill = new Dictionary<FactorySkill, List<Skill>>();
        itemToDelete = new List<Item>();
        skillToDelete = new List<Skill>();

    }

    public void Update()
    {
        //ITEM DELETE
        for (int i = 0; i < System.Enum.GetNames(typeof(FactoryItem)).Length; i++)
        {
            FactoryItem e = (FactoryItem)i;
            if (poolItem.ContainsKey(e))
            {
                List<Item> items = poolItem[e];

                if (items.Count > 0)
                {
                    for (int j = 0; j < items.Count; j++)
                    {
                        items[j].Update();
                        if (items[j].timeDeleted > timeForDelete)
                        {
                            items[j].deleteGameobject();
                            itemToDelete.Add(items[j]);
                        }
                    }
                }
            }
        }
        ItemstoDelete();

        //SKILL DELETE
        for (int i = 0; i < System.Enum.GetNames(typeof(FactorySkill)).Length; i++)
        {
            FactorySkill e = (FactorySkill)i;
            if (poolSkill.ContainsKey(e))
            {
                List<Skill> skills = poolSkill[e];

                if (skills.Count > 0)
                {
                    for (int j = 0; j < skills.Count; j++)
                    {
                        skills[j].Update();
                        if (skills[j].timeDeleted > timeForDelete)
                        {
                            skills[j].deleteGameobject();
                            skillToDelete.Add(skills[j]);
                        }
                    }
                }
            }
        }
        SkillstoDelete();
    }

    //ITEM POOL
    public bool isItemInPool(FactoryItem itemInPool)
    {
        //Debug.Log(itemInPool+"  :  "+ poolItem.ContainsKey(itemInPool));
        if (poolItem.ContainsKey(itemInPool)) {
            List<Item> items = poolItem[itemInPool];

            if (items != null && items.Count > 0)
            {

               
                return true;
            }
           }

        return false;
    }

    public void AddItemInPool(FactoryItem factoryItem,Item item)
    {
        if (!poolItem.ContainsKey(factoryItem))
        {
            List<Item> items = new List<Item>();
            poolItem.Add(factoryItem, items);
        }
        item.delete();
        poolItem[factoryItem].Add(item);

    }

    void ItemstoDelete()
    {
        for (int i = 0; i < System.Enum.GetNames(typeof(FactoryItem)).Length; i++)
        {
            FactoryItem e = (FactoryItem)i;
            if (poolItem.ContainsKey(e))
            {
                List<Item> items = poolItem[e];
                for (int j = 0; j < itemToDelete.Count; j++)
                {
                    items.Remove(itemToDelete[j]);
                }
            }
        }

        itemToDelete = new List<Item>();
    }

    public Item RemoveItemFromPool(FactoryItem factoryItem)
    {
        Item item= null;
        if (poolItem.ContainsKey(factoryItem))
        {
            List<Item> items = poolItem[factoryItem];
            if (items.Count > 0)
            {
                item = items[0];
                items.Remove(item);

                if(item.type == FactoryItem.Bomb)
                {
                    (item as Bomb).init();
                }
            }
        }


        return item;
    }

    //SKILL POOL
    public bool isSkillInPool(FactorySkill skillInPool)
    {
        if (poolSkill.ContainsKey(skillInPool))
        {
            List<Skill> skills = poolSkill[skillInPool];


            if (skills != null && skills.Count > 0)
            {


                return true;
            }
        }

        return false;
    }

    public void AddSkillInPool(FactorySkill factorySkill, Skill skill)
    {
        if (!poolSkill.ContainsKey(factorySkill))
        {
            List<Skill> skills = new List<Skill>();
            poolSkill.Add(factorySkill, skills);
        }
        skill.delete();
        poolSkill[factorySkill].Add(skill);

    }

    void SkillstoDelete()
    {
        for (int i = 0; i < System.Enum.GetNames(typeof(FactorySkill)).Length; i++)
        {
            FactorySkill e = (FactorySkill)i;
            if (poolSkill.ContainsKey(e))
            {
                List<Skill> skills = poolSkill[e];
                for (int j = 0; j < skillToDelete.Count; j++)
                {
                    skills.Remove(skillToDelete[j]);
                }
            }
        }

        skillToDelete = new List<Skill>();
    }

    public Skill RemoveSkillFromPool(FactorySkill factorySkill)
    {
        Skill skill = null;
        if (poolSkill.ContainsKey(factorySkill))
        {
            List<Skill> skills = poolSkill[factorySkill];
            if (skills.Count > 0)
            {
                skill = skills[0];
                skills.Remove(skill);
            }
        }
        return skill;
    }
}
