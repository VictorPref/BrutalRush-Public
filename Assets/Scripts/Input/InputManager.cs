using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class InputManager
{
    public static InputPkg GetKeysInput(int idPlayer)
    {
        //Create package
        InputPkg toRet = new InputPkg();

        if (ReInput.players.GetPlayer(idPlayer) != null)
        {
            bool up, down, right, left;

            //Left Joystick
            up = ReInput.players.GetPlayer(idPlayer).GetButton("JoystickLeft-Up");

            down = ReInput.players.GetPlayer(idPlayer).GetButton("JoystickLeft-Down");

            right = ReInput.players.GetPlayer(idPlayer).GetButton("JoystickLeft-Right");

            left = ReInput.players.GetPlayer(idPlayer).GetButton("JoystickLeft-Left");

            Vector2 dir = new Vector2(0, 0);

            if (up)
            {
                dir.y = 1;
            }
            if(down)
            {
                dir.y = -1;
            }
            if (right)
            {
                dir.x = -1;
            }
            if (left)
            {
                dir.x = 1;
            }

            toRet.LeftStick = dir;

            dir = new Vector2(0, 0);

            //Right Joystick
            up = ReInput.players.GetPlayer(idPlayer).GetButton("JoystickRight-Up");

            down = ReInput.players.GetPlayer(idPlayer).GetButton("JoystickRight-Down");

            right = ReInput.players.GetPlayer(idPlayer).GetButton("JoystickRight-Right");

            left = ReInput.players.GetPlayer(idPlayer).GetButton("JoystickRight-Left");



            if (up)
            {
                dir.y = 1;
            }
            if (down)
            {
                dir.y = -1;
            }
            if (right)
            {
                dir.x = -1;
            }
            if (left)
            {
                dir.x = 1;
            }

            toRet.RightStick = dir;

            //Buttons
            toRet.Up=  ReInput.players.GetPlayer(idPlayer).GetButton("UpButton");
            toRet.Down = ReInput.players.GetPlayer(idPlayer).GetButton("DownButton");
            toRet.Right = ReInput.players.GetPlayer(idPlayer).GetButton("RightButton");
            toRet.Left = ReInput.players.GetPlayer(idPlayer).GetButton("LeftButton");

            toRet.LT = ReInput.players.GetPlayer(idPlayer).GetButtonDown("LT/L2");
            toRet.RT = ReInput.players.GetPlayer(idPlayer).GetButtonDown("RT/R2");
            toRet.A = ReInput.players.GetPlayer(idPlayer).GetButtonDown("Cross/A");
            toRet.B = ReInput.players.GetPlayer(idPlayer).GetButtonDown("Circle/B");
            toRet.X = ReInput.players.GetPlayer(idPlayer).GetButtonDown("Square/X");
            toRet.Y = ReInput.players.GetPlayer(idPlayer).GetButtonDown("Triangle/Y");
            toRet.Start = ReInput.players.GetPlayer(idPlayer).GetButtonDown("Start");
            toRet.RB = ReInput.players.GetPlayer(idPlayer).GetButton("RB/R1");
            toRet.LB = ReInput.players.GetPlayer(idPlayer).GetButtonDown("LB/L1");


            toRet.idPlayer = idPlayer;
        }
        //return package
        return toRet;
    }

    public class InputPkg
    {
        public Vector2 RightStick;
        public Vector2 LeftStick;
        public bool Up;
        public bool Down;
        public bool Right;
        public bool Left;
        public bool A;
        public bool B;
        public bool X;
        public bool Y;
        public bool Start;
        public bool RB;
        public bool LB;
        public bool RT;
        public bool LT;
        public int idPlayer;

        public override string ToString()
        {
            return
                "Right stick values : " + RightStick+ " / " +
                "Left stick values : " + LeftStick + " / " +

                "Id player : " + idPlayer + " / " +
                "A Pressed : " + A + " / " +
                "Start Pressed : " + Start + " / " +
                "B Pressed : " + B + " / " +
                "X Pressed : " + X + " / " +
                "Y Pressed : " + Y + " / " +
                "Up Pressed : " + Up + " / " +
                "Down Pressed : " + Down + " / " +
                "Right Pressed : " + Right + " / " +
                "Left Pressed : " + Left + " / " +
                "RB Pressed : " + RB + " / " +
                "RT Pressed : " + RT + " / " +
                "LB Pressed : " + LB + " / " +
                "LT Pressed : " + LT;
        }
    }

}
