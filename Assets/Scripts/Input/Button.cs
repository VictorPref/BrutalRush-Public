using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button
{
    public enum KeyInputs { A, B, Y, X, LT, LB, RT, RB, StickUp, StickDown, StickToRight, StickToLeft, Up, Down, Right, Left, Start}

    public KeyInputs value;
    public bool pushed;
    public float timePushed;

    public Button(KeyInputs _value, bool _pushed, float _timePushed)
    {
        value = _value;
        pushed = _pushed;
        timePushed = _timePushed;
    }
}
