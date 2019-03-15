using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    private static SoundManager instance = null;

    public enum SoundType { Character, Skill, Menu }

    public AudioSource AttackEffectSource = null;
    public AudioSource SkillEffectSource = null;
    public AudioSource MenuEffectSource = null;

    Dictionary<string, AudioClip> Effects;

    private SoundManager()
    {
        Init();

        Effects = new Dictionary<string, AudioClip>();
        Effects.Add("Button", Resources.Load("Sound/Button") as AudioClip);
        Effects.Add("Hit", Resources.Load("Sound/Hit") as AudioClip);
        Effects.Add("Jump", Resources.Load("Sound/Jump") as AudioClip);
        Effects.Add("Punch", Resources.Load("Sound/Punch") as AudioClip);
        Effects.Add("Thunder", Resources.Load("Sound/Thunder") as AudioClip);
        Effects.Add("Explode", Resources.Load("Sound/Explode") as AudioClip);
        Effects.Add("Poison", Resources.Load("Sound/Poison") as AudioClip);
        Effects.Add("Ice", Resources.Load("Sound/Ice") as AudioClip);
    }

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SoundManager();
            }
            return instance;
        }
    }

    public void Init()
    {
        GameObject src = GameObject.Find("Attack");
        AttackEffectSource = src ? src.GetComponent<AudioSource>() : null;

        src = GameObject.Find("Skill");
        SkillEffectSource = src ? src.GetComponent<AudioSource>() : null;

        src = GameObject.Find("Menu");
        MenuEffectSource = src ? src.GetComponent<AudioSource>() : null;
    }

    public void Play(string soundName, SoundType soundType)
    {
        if (Effects[soundName])
        {
            AudioSource src = null;

            switch (soundType)
            {
                case SoundType.Character:
                    src = AttackEffectSource;
                    break;
                case SoundType.Skill:
                    src = SkillEffectSource;
                    break;

                case SoundType.Menu:
                    src = MenuEffectSource;
                    break;
                default:
                    break;
            }

            if (src)
            {
                if (src.isPlaying)
                    src.Stop();

                src.clip = Effects[soundName];
                src.Play();
            }
        }
    }
}
