using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager  {

    private static GameManager instance = null;

    private GameManager()
    {

    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    Game currentGame;
    int idJoystickPause;
    bool joystick;
    int choicePause;

    UIPause uIPause;

    // Use this for initialization
    public void Start (string levelName) {

        currentGame = new Game();
        currentGame.Start(levelName);
        uIPause = new UIPause();
    }

    // Update is called once per frame
    public void Update () {
        currentGame.Update();
	}


    public void PauseUpdate()
    {
        InputManager.InputPkg pkg = InputManager.GetKeysInput(idJoystickPause);

        if (pkg.LeftStick.y > 0 && !joystick) {

            choicePause++;
            joystick = true;
            if (choicePause > uIPause.getNBChoices() - 1)
            {
                choicePause = 0;
            }
            uIPause.UpdateUI(choicePause);
        }
        else if (pkg.LeftStick.y < 0 && !joystick)
        {
            choicePause--;
            joystick = true;
            if (choicePause < 0)
            {
                choicePause = uIPause.getNBChoices() - 1;
            }

            uIPause.UpdateUI(choicePause);
        }
        else if (pkg.LeftStick.y == 0)
        {
            joystick = false;
        }


        if (pkg.A)
        {
            switch (choicePause)
            {
                case 0:
                    RemovePause();
                    break;
                case 1:
                    RemovePause();
                    FlowManager.Instance.ChangeFlows(FlowManager.SceneNames.MainMenu);
                    break;
                default:
                    break;
            }
        }

    }


    public void SetPause(int id)
    {
        idJoystickPause = id;
        Debug.Log(idJoystickPause);
        (FlowManager.Instance.getCurrentFlow() as GameFlow).SetPause();
        choicePause = 0;
        joystick = false;
        uIPause.Start();
        Time.timeScale = 0f;
    }

    void RemovePause()
    {
        uIPause.HideUI();
        (FlowManager.Instance.getCurrentFlow() as GameFlow).SetPause();
        Time.timeScale = 1f;
    }

    public Vector2 getRandomPosSpawn()
    {
        return currentGame.getRandomSpawnPos();
    }
}
