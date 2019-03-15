using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class PlayerManager {

    private static PlayerManager instance = null;
    List<Player> listePlayers;
    List<CharacterName> playerColors;
    public int maxPlayer = 4;

    private PlayerManager()
    {
    }

    public static PlayerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerManager();
            }
            return instance;
        }
    }

    // Use this for initialization
    public void Start(List<CharacterName> characters, int lives)
    {
        playerColors = characters;
        listePlayers = new List<Player>();
        for(int i = 0; i < ReInput.controllers.joystickCount; i++)//ReInput.controllers.joystickCount
        {
            Player p = new Player();
            p.Start(i, lives, playerColors[i],ReInput.controllers.Joysticks[i].id);
            listePlayers.Add(p);
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (listePlayers != null)
        {
            foreach (Player player in listePlayers)
            {
                player.Update();
            }
        }
    }

    public void FixedUpdate()
    {
        if (listePlayers != null)
        {
            foreach (Player player in listePlayers)
            {
                player.FixedUpdate();
            }
        }
    }

    public Player getPlayer(int index)
    {

        if(listePlayers.Count > index)
        {
            return listePlayers[index];
        }
        return null ;
    }

    public List<Player> getListPlayers()
    {
        return listePlayers;
    }

    static public void ClosePlayerManager()
    {
        instance = null;
    }

    public Player getPlayerFromGameObject(GameObject go)
    {
        for(int i = 0; i < listePlayers.Count; i++)
        {
            if(listePlayers[i].character.character == go)
            {
                return listePlayers[i];
            }
        }
        return null;
    }

    public void ChangeColorUI()
    {
        for (int i = 0; i < listePlayers.Count; i++)
        {
            listePlayers[i].character.InitUIColor();
        }
    }

    public void InitUIPlayer()
    {
        ChangeColorUI();
        for (int i = 0; i < listePlayers.Count; i++)
        {
            UpdateUIPlayer(i);
        }
    }

    public void UpdateUIPlayer(int id)
    {
        listePlayers[id].character.UpdateUIPlayer();
    }

    public int HowManyPlayersAlive()
    {
        int nb = 0; 

        for(int i = 0; i < listePlayers.Count; i++)
        {
            if (listePlayers[i].character.isAlive())
                nb++;
        }

        return nb;
    }

    public void SetUpClampNamePlayer(Text txt, int id)
    {
        listePlayers[id].character.SetUpClampName(txt);
    }


    public void DeleteAllPlayers()
    {
        for(int i = 0; i < listePlayers.Count; i++)
        {
            listePlayers[i].DeleteSelf();
        }
    }

    public Player getLastPlayerAlive()
    {
        Player p = null;

        for(int i = 0; i < listePlayers.Count; i++)
        {
            if (listePlayers[i].character.isAlive())
            {
                p = listePlayers[i];
            }
        }
        return p;
    }
}
