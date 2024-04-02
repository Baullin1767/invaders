using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : IButtonsHandler
{
    public bool GetButtonLeft()
    {
        return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
    }

    public bool GetButtonRight()
    {
        return Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
    }

    public bool GetButtonShoot()
    {
        return Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
    }
}
