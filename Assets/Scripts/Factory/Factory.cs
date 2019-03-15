using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FactoryItem { Bomb,atk,Health,Energy,Special};
public enum FactorySkill{ ArticBlast, InfernalSpark, LightningStrike, ToxicFlash };

public class Factory  {

    private static Factory instance = null;

    private Factory()
    {
    }

    public static Factory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Factory();
            }
            return instance;
        }
    }

   public ObjectPool pool;

    public void Start () {
        pool = new ObjectPool();
        pool.Start();
	}

    public Item spawnItem(FactoryItem itemToSpawn,Vector2 pos)
    {
        Item item = null;
        if (!pool.isItemInPool(itemToSpawn))
        {
            
            switch (itemToSpawn)
            {
                case FactoryItem.Bomb:
                    item = new Bomb();
                   item.Start("Prefabs/Item/bomb", pos, FactoryItem.Bomb);
                    (item as Bomb).init();

                    break;
                case FactoryItem.atk:
                    item = new Attack();
                    item.Start("Prefabs/Item/atk", pos, FactoryItem.atk);              
                    break;
                case FactoryItem.Health:
                    item = new Health();
                    item.Start("Prefabs/Item/health", pos, FactoryItem.Health);
                    break;
                case FactoryItem.Energy:
                    item = new Energy();
                    item.Start("Prefabs/Item/energy", pos, FactoryItem.Energy);    
                    break;
                case FactoryItem.Special:
                    item = new Special();
                    item.Start("Prefabs/Item/special", pos, FactoryItem.Special);
                    break;
                default:
                    break;
            }
        }
        else
        {
            item = pool.RemoveItemFromPool(itemToSpawn);
            item.initialization(pos);
        }

        return item;
    }


    public Skill spawnSkill(FactorySkill skillToSpawn, Vector2 pos, float skillOverpower, int idCharacter)
    {
        Skill skill = null;
        if (!pool.isSkillInPool(skillToSpawn))
        {

            switch (skillToSpawn)
            {
                case FactorySkill.ArticBlast:
                    skill = new ArticBlast();
                    break;
                case FactorySkill.InfernalSpark:
                    skill = new InfernalSpark();
                    break;
                case FactorySkill.LightningStrike:
                    skill = new LightningStrike();
                    break;
                case FactorySkill.ToxicFlash:
                    skill = new ToxicFlash();
                    break;
                default:
                    break;
            }
            skill.Start("Prefabs/Skill/"+ skillToSpawn, pos, skillToSpawn, skillOverpower, idCharacter);
        }
        else
        {
            skill = pool.RemoveSkillFromPool(skillToSpawn);
            skill.initialization(pos, skillOverpower, idCharacter);
        }

        return skill;
    }


    public void Update()
    {
        pool.Update();    
    }
	
	
}
