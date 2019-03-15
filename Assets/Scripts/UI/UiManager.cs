using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager  {

    private static UiManager instance = null;

    private UiManager()
    {
    }

    public static UiManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UiManager();
            }
            return instance;
        }
    }

    UIMenuTitle MenuTitle;
    UIMenu menuUI;
    bool menu;
    
    // Use this for initialization
    public void StartMenu () {
        MenuTitle = new UIMenuTitle();
        MenuTitle.Start();
        menu = false;

    }


    // Update is called once per frame
    public void UpdateMenu () {
        InputManager.InputPkg pkg = InputManager.GetKeysInput(0);

        if (!menu)
        {
            MenuTitle.Update();
        }
        if (pkg.Start && !menu)
        {
            menu = true;
            menuUI = new UIMenu();
           
            menuUI.Start();
            MenuTitle.DestroyGO();
        }

        if (menu)
        {
            menuUI.Update(pkg);
            
        }
    }

    public UIMenu getMenu()
    {
        return menuUI;
    }

    UIGame uIGame;

    public void StartUIGame()
    {
        uIGame = new UIGame();
        uIGame.Start();
        PlayerManager.Instance.InitUIPlayer();

    }

    public void ActiveDesactiveSpecialBar(int id,bool state)
    {
        uIGame.ActiveDesactiveBar(id, state);
    }

    public void ChangeColorUI(Color color, int id)
    {
        uIGame.ChangeImageSource(color, id);
    }
    public void UpdateUIGame(int id, float health, float energy, int nbLives)
    {
        uIGame.UpdateUiPlayer(id, health, energy, nbLives);
    }

    public UIGame GetUiGame()
    {
        return uIGame;
    }
}
