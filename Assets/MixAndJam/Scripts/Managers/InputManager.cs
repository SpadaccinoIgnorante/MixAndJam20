using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public static float hRightAxis;
    public static float vRightAxis;
    public static float hLeftAxis;
    public static float vLeftAxis;
    public static bool lTrigger;
    public static bool rTrigger;
    public static bool interact;
    public static bool jump;

    private const string INTERACT_ACTION = "Interact";
    private const string JUMP_ACTION = "Jump";
    private const string HORIZONTAL_LEFT_AXIS = "HorizontalMove";
    private const string VERTICAL_LEFT_AXIS = "VerticalMove";
    private const string HORIZONTAL_RIGHT_AXIS = "HorizontalLook";
    private const string VERTICAL_RIGHT_AXIS = "VerticalLook";
    private const string LEFT_TRIGGER = "Absorb";
    private const string RIGHT_TRIGGER = "Shoot";

    private Player playerControls;

    public void Awake()
    {
        if (instance == null) { instance = this; }
        else if(instance != null) { Destroy(gameObject); }

        DontDestroyOnLoad(gameObject);

        playerControls = ReInput.players.GetPlayer(0);
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
    }

}
