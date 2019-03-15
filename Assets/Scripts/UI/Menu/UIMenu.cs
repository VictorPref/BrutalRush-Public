using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu 
{
    const float VISIBLE = 1;
    const float INVISIBLE = 0.5f;

    GameObject menu;
    // Start is called before the first frame update
    List<Image> choices;
    int choiceNb = 0;
    bool changeChoice;

    bool characterSelection = false;
    bool optionSelection = false;
  public  UICharacterSelection uICharacterSelection;
    UIOption uIOption;
   

    public void Start()
    {
        menu = GameObject.Instantiate(Resources.Load("Prefabs/UI/Menu/Menu", typeof(GameObject))) as GameObject;
        int cpt = menu.transform.GetChild(0).GetChild(1).childCount;
        choices = new List<Image>();
        uICharacterSelection = new UICharacterSelection();
        uIOption = new UIOption();

        for (int i = 0; i < cpt; i++)
        {
            Image choice = menu.transform.GetChild(0).GetChild(1).GetChild(i).gameObject.GetComponent<Image>();
            choices.Add(choice);
        }
    }

    // Update is called once per frame
    public void Update(InputManager.InputPkg pkg)
    {
        if (!characterSelection && !optionSelection)
        {
            if (pkg.LeftStick.x == 1 && !changeChoice)
            {
                SoundManager.Instance.Play("Button", SoundManager.SoundType.Menu);
                choiceNb--;
                changeChoice = true;
                if (choiceNb < 0)
                    choiceNb = choices.Count - 1;
            }
            if (pkg.LeftStick.x == -1 && !changeChoice)
            {
                SoundManager.Instance.Play("Button", SoundManager.SoundType.Menu);
                choiceNb++;
                changeChoice = true;
                if (choiceNb > choices.Count - 1)
                    choiceNb = 0;
            }

            if (pkg.LeftStick.x == 0)
            {
                changeChoice = false;
            }
            UpdateImage();

            if (pkg.A)
            {
                SoundManager.Instance.Play("Button", SoundManager.SoundType.Menu);
                switch (choiceNb)
                {
                    case 0:
                        characterSelection = true;
                        uICharacterSelection.Start();
                        break;
                    case 1:
                        optionSelection = true;
                        uIOption.Start();
                        break;
                    case 2:
                        Application.Quit(0);
                        break;
                    default:
                        break;
                }

            }
        }
        else if (characterSelection)
        {
            uICharacterSelection.Update();
        }
        else if (optionSelection)
        {
            uIOption.Update();
        }
    }

    public void GoBackMenu()
    {
        if (characterSelection)
        {
            uICharacterSelection.removeUI();
            characterSelection = false;
        }

        if (optionSelection)
        {
            uIOption.removeUI();
            optionSelection = false;
        }

    }

    public void LevelSelection()
    {
        uICharacterSelection.RemoveLevelSelection();
    }

    public void UpdateImage()
    {

        for(int i = 0; i < choices.Count; i++)
        {
            Color co = choices[i].color;
            co.a = INVISIBLE;
            choices[i].color = co;
        }

        Color c = choices[choiceNb].color;
        c.a = VISIBLE;
        choices[choiceNb].color = c;
    }

  
}
