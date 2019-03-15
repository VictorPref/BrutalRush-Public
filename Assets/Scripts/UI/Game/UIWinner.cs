using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWinner
{
    const float TIME = 0.5f;
    const float VISIBLE = 1;
    const float INVISIBLE = 0.5f;


    GameObject uiWinner;
    List<Image> choices;
    Image playerImage;
    Text playerText;
    int choice;
    bool joystick;
    Player p;
    bool boutton;
    float timer;
    // Start is called before the first frame update
    public void Start(Player player)
    {
        uiWinner = GameObject.Instantiate(Resources.Load("Prefabs/UI/Game/UIWinner", typeof(GameObject))) as GameObject;
        choices = new List<Image>();
        p = player;

        Transform gameChoice = null;
        Transform infoPlayer = null;
        choice = 0;
        joystick = false;
        boutton = false;
        timer = TIME + Time.time;
        for (int i = 0;i < uiWinner.transform.childCount; i++)
        {
            if(uiWinner.transform.GetChild(i).tag == "UISelection")
            {
                gameChoice = uiWinner.transform.GetChild(i);
            }
            else if(uiWinner.transform.GetChild(i).tag == "UIInfoPlayer")
            {
                infoPlayer = uiWinner.transform.GetChild(i);
            }
        }


        for(int i = 0; i < infoPlayer.childCount; i++)
        {
            if(infoPlayer.GetChild(i).tag == "Text")
            {
                playerText = infoPlayer.GetChild(i).GetComponent<Text>();
            }
            else if(infoPlayer.GetChild(i).tag == "Image")
            {
                playerImage = infoPlayer.GetChild(i).GetComponent<Image>();
            }
        }


        for(int i = 0; i < gameChoice.childCount; i++)
        {
            choices.Add(gameChoice.GetChild(i).GetComponent<Image>());
        }

        playerImage.color = p.character.sr.color;
        playerText.text = "P" + (p.id+1);

        UpdateUI();
    }

    public void Update(int id)
    {
        InputManager.InputPkg pkg = InputManager.GetKeysInput(id);


        if(pkg.LeftStick.x > 0 && !joystick)
        {
            joystick = true;

            choice--;

            if(choice < 0)
            {
                choice = choices.Count - 1;
            }
            UpdateUI();

        }
        else if (pkg.LeftStick.x < 0 && !joystick)
        {
            joystick = true;
            choice++;

            if(choice > choices.Count - 1)
            {
                choice = 0;
            }
            UpdateUI();
        }
        else if(pkg.LeftStick.x == 0)
        {
            joystick = false;
        }


        if (pkg.A && boutton)
        {

            switch (choice)
            {
                case 0:
                    (FlowManager.Instance.getCurrentFlow() as GameFlow).Restart();
                    HideUI();
                    break;
                case 1:
                    FlowManager.Instance.ChangeFlows(FlowManager.SceneNames.MainMenu);
                    break;
                default:
                    break;
            }
        }
        if(timer < Time.time)
        {
            boutton =true;
        }

    }

    // Update is called once per frame
    public void UpdateUI()
    {
        for(int i = 0; i < choices.Count; i++)
        {
            Color c = choices[i].color;
            if (i == choice)
                c.a = INVISIBLE;
            else
                c.a = VISIBLE;

            choices[i].color = c;
        }   
    }
    public void HideUI()
    {
        GameObject.Destroy(uiWinner);
    }

    
}
