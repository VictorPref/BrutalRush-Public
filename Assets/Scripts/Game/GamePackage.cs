using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GamePackage
{
    const int MAXVIE = 10;
    const int MINVIE = 1;
    const int DEFAULTLIVES = 4;

    int nbLives;
    List<CharacterName> characterNames;
    string levelName;


    public GamePackage()
    {
        nbLives = DEFAULTLIVES;
    }

    public void AddLives()
    {
        if(nbLives < MAXVIE)
            nbLives++;
    }

    public void RemoveLives()
    {
        if (nbLives > MINVIE)
               nbLives--;
    }


    public void DesactiveActiveItem(List<bool> active)
    {
        ItemManager.Instance.setItemActive(active);
    }


    public List<bool> GetActiveItem()
    {
        return ItemManager.Instance.GetItemActive();
    }
    public void SetCharacterList(List<int> characters)
    {
        characterNames = new List<CharacterName>();
        for (int i = 0; i < characters.Count; i++)
        {
            characterNames.Add((CharacterName)Enum.ToObject(typeof(CharacterName), characters[i]));
        }
    }

    public List<CharacterName> GetCharacterNames()
    {
        return characterNames;
    }

    public int GetNbLives()
    {
        return nbLives;
    }

    public void SetNameLevel(string lvl)
    {
        levelName = lvl;
    }

    public string GetNameLevel()
    {
        return levelName;
    }
}
