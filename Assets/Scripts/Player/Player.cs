using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{

    public int id;
    float time;
    public int idJoystick;

    public Character character;

    // Use this for initialization

    public void Start(int idd, int lives, CharacterName chName,int joystick)
    {
        id = idd;
        character = new Character();
        character.Start(lives, chName, id);
        idJoystick = joystick;
    }

    // Update is called once per frame
    public void Update()
    {

        InputManager.InputPkg pkg = InputManager.GetKeysInput(idJoystick);
        character.Update(pkg);

        if (pkg.Start)
        {
            GameManager.Instance.SetPause(idJoystick);
        }
    }

    public void FixedUpdate()
    {
        InputManager.InputPkg pkg = InputManager.GetKeysInput(idJoystick);
        character.FixedUpdate(pkg);
    }

    public void DeleteSelf()
    {
        GameObject.Destroy(character.character);
    }
}