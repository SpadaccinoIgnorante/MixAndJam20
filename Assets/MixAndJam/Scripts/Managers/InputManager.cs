﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class InputManager : Singleton<InputManager>
{
    public static float hRightAxis;
    public static float vRightAxis;
    public static float hLeftAxis;
    public static float vLeftAxis;
    public static bool lTrigger;
    public static bool rTrigger;
    public static bool interact;
    public static bool jump;
    public static bool pause;

    private const string INTERACT_ACTION = "Interact";
    private const string JUMP_ACTION = "Jump";
    private const string HORIZONTAL_LEFT_AXIS = "HorizontalMove";
    private const string VERTICAL_LEFT_AXIS = "VerticalMove";
    private const string HORIZONTAL_RIGHT_AXIS = "HorizontalLook";
    private const string VERTICAL_RIGHT_AXIS = "VerticalLook";
    private const string LEFT_TRIGGER = "Absorb";
    private const string RIGHT_TRIGGER = "Shoot";
    private const string PAUSE = "Pause";

    private Player playerControls;
    private static Player playerControlsStatic;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);

        playerControls = ReInput.players.GetPlayer(0);
        playerControlsStatic = ReInput.players.GetPlayer(0);
    }

    private void Update()
    {
        interact = playerControls.GetButtonDown(INTERACT_ACTION);
        hRightAxis = playerControls.GetAxis(HORIZONTAL_RIGHT_AXIS);
        vRightAxis = playerControls.GetAxis(VERTICAL_RIGHT_AXIS);
        hLeftAxis = playerControls.GetAxis(HORIZONTAL_LEFT_AXIS);
        vLeftAxis = playerControls.GetAxis(VERTICAL_LEFT_AXIS);
        rTrigger = playerControls.GetButton(RIGHT_TRIGGER);
        lTrigger = playerControls.GetButton(LEFT_TRIGGER);
        jump = playerControls.GetButtonDown(JUMP_ACTION);
        pause = playerControls.GetButtonDown(PAUSE);
    }

    public static bool isUsingController()
    {
        bool result = false;
        // Get last controller from a Player and the determine the type of controller being used
        Controller controller = playerControlsStatic.controllers.GetLastActiveController();
        if (controller != null)
        {
            switch (controller.type)
            {
                case ControllerType.Keyboard:
                    // Do something for keyboard
                    result = false;
                    break;
                case ControllerType.Joystick:
                    // Do something for joystick
                    result = true;
                    break;
                case ControllerType.Mouse:
                    // Do something for mouse
                    result = false;
                    break;
                case ControllerType.Custom:
                    // Do something custom controller
                    result = true;
                    break;
            }
        }

        return result;
    }

}
