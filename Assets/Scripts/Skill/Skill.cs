using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Skill {

    public const float CIRCLE_CAST = 1.5f;

    public float timeDeleted;
    public bool deleted = false;
    public GameObject skill;
    public int layerMask;
    public FactorySkill type;
    public Animator anim;
    public bool isPlaying = false;
    public List<Character> charactersHits;
    public float overpower;
    public AudioSource soundEffect;
    public int idCharacter;

    public virtual void Start(string path, Vector2 pos, FactorySkill skillType, float _overpower, int _idCharacter)
    {
        skill = GameObject.Instantiate(Resources.Load(path)) as GameObject;
        anim = skill.GetComponent<Animator>();
        layerMask = 1 << Character.CHARACTER_LAYER;
        type = skillType;
        charactersHits = new List<Character>();
        initialization(pos, _overpower, _idCharacter);
    }

    public virtual void UpdateSkill()
    {
 
    }

    public void LaunchSkill()
    {
        anim.SetTrigger("Launch");

        switch (type)
        {
            case FactorySkill.ArticBlast:
                SoundManager.Instance.Play("Ice", SoundManager.SoundType.Skill);
                break;
            case FactorySkill.InfernalSpark:
                SoundManager.Instance.Play("Explode", SoundManager.SoundType.Skill);
                break;
            case FactorySkill.LightningStrike:
                SoundManager.Instance.Play("Thunder", SoundManager.SoundType.Skill);
                break;
            case FactorySkill.ToxicFlash:
                SoundManager.Instance.Play("Poison", SoundManager.SoundType.Skill);
                break;
            default:
                break;
        }
    }

    public void Update()
    {
        timeDeleted += Time.deltaTime;
    }

    public void initialization(Vector2 pos, float _overpower, int _idCharacter)
    {
        isPlaying = false;
        deleted = false;
        
        skill.SetActive(true);
        timeDeleted = 0;
        skill.transform.position = pos;
        overpower = _overpower;
        idCharacter = _idCharacter;
        LaunchSkill();
    }

    public void deleteGameobject()
    {
        GameObject.Destroy(skill);
    }

    public void delete()
    {
        skill.SetActive(false);
        deleted = true;
    }
}
