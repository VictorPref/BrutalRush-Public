using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelSelection
{
    const float VISIBLE = 1;
    const float INVISIBLE = 0.5f;

    GameObject levelSelection;
    List<Image> levelChoice;
    int choice;

    bool joystick = false;
    // Start is called before the first frame update
    public void Start()
    {
        levelSelection = GameObject.Instantiate(Resources.Load("Prefabs/UI/LevelSelection/LevelSelection", typeof(GameObject))) as GameObject;

        Transform ch = levelSelection.transform.GetChild(0);

        levelChoice = new List<Image>();
        choice = 0;

        for (int i = 0; i < ch.childCount; i++)
        {
            if(ch.GetChild(i).tag == "UISelection")
            {
                for(int j = 0; j < ch.GetChild(i).childCount; j++)
                {
                    levelChoice.Add(ch.GetChild(i).GetChild(j).GetComponent<Image>());
                }
            }

        }
        UpdateUI();
    }

    // Update is called once per frame
   public void Update()
    {
        InputManager.InputPkg pkg = InputManager.GetKeysInput(0);

        if(pkg.LeftStick.x > 0 && !joystick)
        {
            SoundManager.Instance.Play("Button", SoundManager.SoundType.Menu);
            choice--;
            joystick = true;
            if (choice < 0)
            {
                choice = levelChoice.Count - 1;
            }

            UpdateUI();
        }
        else if (pkg.LeftStick.x < 0 && !joystick)
        {
            SoundManager.Instance.Play("Button", SoundManager.SoundType.Menu);
            choice++;
            joystick = true;

            if (choice > levelChoice.Count - 1)
            {
                choice = 0;
            }
            UpdateUI();

        }
        else if (pkg.LeftStick.x == 0)
        {
            joystick = false;
        }

        if (pkg.A)
        {
            SoundManager.Instance.Play("Button", SoundManager.SoundType.Menu);
            FlowManager.Instance.GetGamePackage().SetNameLevel(levelChoice[choice].sprite.name);
                FlowManager.Instance.ChangeFlows(FlowManager.SceneNames.Game);
        }
        if (pkg.B)
        {
            SoundManager.Instance.Play("Button", SoundManager.SoundType.Menu);
            UiManager.Instance.getMenu().LevelSelection();
            UiManager.Instance.getMenu().uICharacterSelection.RestartCharacterSelection();
        }

       
    }

    public void removeUI()
    {
        GameObject.Destroy(levelSelection);
    }


    void UpdateUI()
    {
        for (int i = 0; i < levelChoice.Count; i++)
        {
            Color c = levelChoice[i].color;
            if (choice != i)
                c.a = INVISIBLE;
            else
                c.a = VISIBLE;

            levelChoice[i].color = c;
        }
    }

}
