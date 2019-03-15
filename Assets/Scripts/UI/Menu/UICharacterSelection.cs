using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
public class UICharacterSelection
{

    GameObject selection;
    List<List<GameObject>> SelectionOfPlayer;
    List<Image> PlayersCharacter;
    List<GameObject> Players;
    List<Color> characterColor;
    List<GameObject> readyImage;

    List<int> choices;
    List<bool> joystick;
    bool startGame;
    UILevelSelection levelSelection;
    bool inLevelSelection;        

    List<bool> playerDone;

    int nbPlayers;
    int maxPlayer;
    // Start is called before the first frame update
    public void Start()
    {
        selection = GameObject.Instantiate(Resources.Load("Prefabs/UI/CharacterSelection/CharacterSelection", typeof(GameObject))) as GameObject;
        GameObject child = selection.transform.GetChild(0).gameObject;
        GameObject characterSelect = null;
        GameObject playerSelection = null;
        characterColor = new List<Color>();
        readyImage = new List<GameObject>();
        characterColor.Add(Color.yellow);
        characterColor.Add(Color.red);
        characterColor.Add(Color.blue);
        characterColor.Add(Color.green);
        levelSelection = new UILevelSelection();
        Players = new List<GameObject>();
        choices = new List<int>();
        maxPlayer = 4;
        startGame = false;

        inLevelSelection = false;

     

        for (int i = 0; i < child.transform.childCount; i++)
        {

            if(child.transform.GetChild(i).tag == "UICharacterSelection")
            {
                characterSelect = child.transform.GetChild(i).gameObject;
            }
            else if(child.transform.GetChild(i).tag == "UIPlayersSelection")
            {
                playerSelection = child.transform.GetChild(i).gameObject;
            }
        }

        SelectionOfPlayer = new List<List<GameObject>>();
        for (int i = 0; i < characterSelect.transform.childCount; i++)
        {
            List<GameObject> image = new List<GameObject>();

            Transform ch = characterSelect.transform.GetChild(i);

            for(int j = 0; j < ch.childCount; j++)
            {
                GameObject p = null;

                if(ch.GetChild(j).tag == "P1")
                {
                    p = ch.GetChild(j).gameObject;
                }
                else if (ch.GetChild(j).tag == "P2")
                {
                    p = ch.GetChild(j).gameObject;
                }
                else if (ch.GetChild(j).tag == "P3")
                {
                    p = ch.GetChild(j).gameObject;
                }
                else if (ch.GetChild(j).tag == "P4")
                {
                    p = ch.GetChild(j).gameObject;
                }

                if(p != null)
                {
                    
                    image.Add(p);
                }

            }
            SelectionOfPlayer.Add(image);

            
        }


        PlayersCharacter = new List<Image>();

        for (int i = 0; i < playerSelection.transform.childCount; i++)
        {
            Transform ch = playerSelection.transform.GetChild(i);

            GameObject p = null;
            if(ch.tag == "P1")
            {
                p = ch.gameObject;

            }
            else if(ch.tag == "P2")
            {
                p = ch.gameObject;
            }
            else if (ch.tag == "P3")
            {
                p = ch.gameObject;

            }
            else if (ch.tag == "P4")
            {
                p = ch.gameObject;
            }

            if(p!= null)
            {
                Players.Add(p);
                for (int j = 0; j < p.transform.childCount; j++)
                {
                    if(p.transform.GetChild(j).tag == "UIPlayerImage")
                    {
                        Image image = p.transform.GetChild(j).GetComponent<Image>();
                        PlayersCharacter.Add(image);
                    }
                    if(p.transform.GetChild(j).tag == "Ready")
                    {
                        GameObject image = p.transform.GetChild(j).gameObject;
                        readyImage.Add(image);
                        image.SetActive(false);
                    }
                }

            }
        }


        CheckNumberControler();

        joystick = new List<bool>();
        playerDone = new List<bool>();
        for (int i = 0; i < maxPlayer; i++)
        {
            choices.Add(0);
            joystick.Add(false);
            playerDone.Add(false);
        }


    }

    // Update is called once per frame
    public void Update()
    {

        if (nbPlayers != ReInput.controllers.joystickCount && ReInput.controllers.joystickCount <= maxPlayer)
        {
            CheckNumberControler();
        }
        else
        {
            for (int i = 0; i < ReInput.controllers.joystickCount; i++)
            {
               
                CheckInputForPlayers(ReInput.controllers.Joysticks[i].id,i);
                UpdateUI(i);
            }
        }
        if (nbPlayers > 1) {
            int playerReady = 0;
            for (int i = 0; i < nbPlayers; i++)
            {
                if (playerDone[i])
                {
                    playerReady++;
                }
                
             }
            if(playerReady == nbPlayers)
            {
                startGame = true;
            }
        }


        if (startGame && !inLevelSelection)
        {
            Debug.Log("START CHARACTERSELECTION");
            FlowManager.Instance.GetGamePackage().SetCharacterList(choices);

            
            inLevelSelection = true;
            levelSelection.Start();
            selection.SetActive(false);
        }

        if (inLevelSelection)
        {
            levelSelection.Update();
        }
    }

    public void CheckNumberControler()
    {
        nbPlayers = ReInput.controllers.joystickCount;


        ReInput.controllers.AutoAssignJoysticks();

        for (int i = 0; i < maxPlayer; i++)
        {
            if(i < nbPlayers)
            {
                Players[i].SetActive(true);
            }
            else
            {
                Players[i].SetActive(false);
            }

        }

        for(int i = 0; i < SelectionOfPlayer.Count; i++)
        {
            for (int j = 0; j < SelectionOfPlayer[i].Count; j++)
            {
                SelectionOfPlayer[i][j].SetActive(false);
            }
        }
    }

    public void UpdateUI(int id)
    {
        for(int i = 0; i < SelectionOfPlayer.Count; i++)
        {
            for (int j = 0; j < SelectionOfPlayer[i].Count; j++)
            {
                SelectionOfPlayer[i][j].SetActive(false);
            }
        }


        for (int i = 0; i < nbPlayers; i++)
        {
            SelectionOfPlayer[choices[i]][i].SetActive(true);
            PlayersCharacter[i].color = characterColor[choices[i]];

            if (playerDone[i])
            {
                readyImage[i].SetActive(true);
            }
            else
            {
                readyImage[i].SetActive(false);
            }
        }
    }

    public void removeUI()
    {
        GameObject.Destroy(selection);
    }

    public void RemoveLevelSelection()
    {
        inLevelSelection = false;
        levelSelection.removeUI();
        selection.SetActive(true);
    }

    void CheckInputForPlayers(int id,int i)
    {
        InputManager.InputPkg pkg = InputManager.GetKeysInput(id);

        if (!playerDone[i])
        {
            if (pkg.LeftStick.x < 0 && !joystick[i])
            {
                SoundManager.Instance.Play("Button", SoundManager.SoundType.Menu);
                joystick[i] = true;
                choices[i] += 1;
                if (choices[i] > characterColor.Count - 1)
                {
                    choices[i] = 0;
                }

            }
            else if (pkg.LeftStick.x > 0 && !joystick[i])
            {
                SoundManager.Instance.Play("Button", SoundManager.SoundType.Menu);
                joystick[i] = true;
                choices[i] -= 1;
                if (choices[i] < 0)
                {
                    choices[i] = characterColor.Count - 1;
                }
            }
            else if (pkg.LeftStick.x == 0)
            {
                joystick[i] = false;

            }

            if (pkg.Start)
            {
                SoundManager.Instance.Play("Button", SoundManager.SoundType.Menu);
                playerDone[i] = true;
            }
            if (pkg.B && !inLevelSelection)
            {
                SoundManager.Instance.Play("Button", SoundManager.SoundType.Menu);
                UiManager.Instance.getMenu().GoBackMenu();
            }
        }

        if (pkg.B)
        {
            SoundManager.Instance.Play("Button", SoundManager.SoundType.Menu);
            playerDone[i] = false;
        }


    }

    public void RestartCharacterSelection()
    {
        inLevelSelection = false;
        startGame = false;
        for(int i = 0; i < playerDone.Count; i++)
        {
            playerDone[i] = false;
        }
    }
}
