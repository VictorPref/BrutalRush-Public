using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class GameFlow : Flow
{
    readonly int mapSize = 10;

    GameObject gO;
    bool pause;
    bool endGame;
    bool loadWinnerUI;
    UIWinner uIWinner;

    override
    public void InitializeFlow()
    {
        GamePackage pkg = FlowManager.Instance.GetGamePackage();
        PlayerManager.Instance.Start(pkg.GetCharacterNames(), pkg.GetNbLives());
        UiManager.Instance.StartUIGame();
        GameManager.Instance.Start(FlowManager.Instance.GetGamePackage().GetNameLevel());
        Factory.Instance.Start();
        pause = false;
        endGame = false;
        loadWinnerUI = false;
        uIWinner = new UIWinner();
    }

    override
    public void UpdateFlow(float dt)
    {
        if (!pause && !endGame)
        {
            PlayerManager.Instance.Update();
            GameManager.Instance.Update();
            Factory.Instance.Update();
            SkillManager.Instance.Update();
        }
        else if(!endGame)
        {
            GameManager.Instance.PauseUpdate();
        }
        else
        {
            if (!loadWinnerUI)
            {
                loadWinnerUI = true;
                uIWinner.Start(PlayerManager.Instance.getLastPlayerAlive());
            }
            UIEndGame();
        }


       
    }


    public void UIEndGame()
    {
        uIWinner.Update(PlayerManager.Instance.getPlayer(0).idJoystick);
    }

    override
    public void FixedUpdateFlow(float dt)
    {
        if (!pause && !endGame)
        {
            PlayerManager.Instance.FixedUpdate();
        }
    }

    override
    public void CloseFlow()
    {
        PlayerManager.ClosePlayerManager();
    }

    public void SetPause()
    {
        pause = !pause;
    }

    public void EndGame()
    {
        endGame = true;
    }

    public void Restart()
    {

        FlowManager.Instance.ChangeFlows(FlowManager.SceneNames.Game);

    }
}
