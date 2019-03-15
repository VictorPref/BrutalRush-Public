using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick
{
    public Button Up;
    public Button Down;
    public Button Right;
    public Button Left;

    public Stick(Button _up, Button _down, Button _right, Button _left)
    {
        Up = _up;
        Down = _down;
        Right = _right;
        Left = _left;
    }
}
