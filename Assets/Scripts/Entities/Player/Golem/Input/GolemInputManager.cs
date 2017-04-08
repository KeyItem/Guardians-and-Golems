﻿using System.Collections;
using UnityEngine;

public enum PlayerNum
{
    PLAYER_1,
    PLAYER_2,
    PLAYER_3,
    PLAYER_4
}

public enum PlayerTeam
{
    RED,
    BLUE,
    NONE
}

public class GolemInputManager : MonoBehaviour 
{
    private GolemPlayerController golemPlayerController;

    [Header("Golem Player Values")]
    public PlayerNum playerNum;
    public PlayerTeam playerTeam;

    public string PlayerName;

    public int PlayerNumber;

    [Header("Golem Input Values")]
    [HideInInspector]
    public float xAxis;
    [HideInInspector]
    public float zAxis;
    [HideInInspector]
    public float aimZAxis;
    //private float modifierAxis;
    [HideInInspector]
    public float leftTriggerAxis;
    [HideInInspector]
    public float rightTriggerAxis;

    [HideInInspector]
    public Vector2 moveVec;
    [HideInInspector]
    public Vector2 moveDirection;

    [HideInInspector]
    public Vector3 aimVec;

    [HideInInspector]
    public float aimXAxis;

    [Space(10)]
    public float holdTime;
    private float holdMultiplier = 2f;

    [Header("Debug Values")]
    public bool isHoldingAbility = false;

    private string inputAxisX = "";
    private string inputAxisZ = "";
    private string inputAimAxisX = "";
    private string inputAimAxisZ = "";
    //private string inputModifierAxis = "";
    private string inputLeftTriggerAxis = "";
    private string inputRightTriggerAxis = "";
    private string inputAttackButton = "";
    private string inputDodgeButton = "";
    private string inputAbility1Button = "";
    private string inputAbility2Button = "";

    public bool isLeftTriggerPressed = false;
    public bool isRightTriggerPressed = false;

    private void Start()
    {
        PlayerSetup();
    }

    private void Update()
    {
        GetInput();

        CalculateAimVec();
    }

    void PlayerSetup()
    {
        golemPlayerController = GetComponent<GolemPlayerController>();    

        switch(playerNum)
        {
            case PlayerNum.PLAYER_1:
                PlayerNumber = 1;
                break;
            case PlayerNum.PLAYER_2:
                PlayerNumber = 2;
                break;
            case PlayerNum.PLAYER_3:
                PlayerNumber = 3;
                break;
            case PlayerNum.PLAYER_4:
                PlayerNumber = 4;
                break;
        }

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

        inputAxisX = "HorizontalPlayer" + PlayerNumber + "Win";
        inputAxisZ = "VerticalPlayer" + PlayerNumber + "Win";

        inputAimAxisX = "HorizontalAimPlayer" + PlayerNumber + "Win";
        inputAimAxisZ = "VerticalAimPlayer" + PlayerNumber + "Win";

        //inputModifierAxis = "ModifierAxisPlayer" + PlayerNumber + "Win";
        inputLeftTriggerAxis = "LeftTriggerAxisPlayer" + PlayerNumber + "Win";
        inputRightTriggerAxis = "RightTriggerAxisPlayer" + PlayerNumber + "Win";

        inputAttackButton = "joystick " + PlayerNumber + " button 2";
        inputDodgeButton = "joystick " + PlayerNumber + " button 1";

        inputAbility1Button = "joystick " + PlayerNumber + " button 4";
        inputAbility2Button = "joystick " + PlayerNumber + " button 5";
#endif

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX

        inputAxisX = "HorizontalPlayer" + PlayerNumber + "OSX";
        inputAxisZ = "VerticalPlayer" + PlayerNumber + "OSX";

        inputAimAxisX = "HorizontalAimPlayer" + PlayerNumber + "OSX";
        inputAimAxisZ = "VerticalAimPlayer" + PlayerNumber + "OSX";

        //inputModifierAxis = "ModifierAxisPlayer" + PlayerNumber + "OSX";
        inputLeftTriggerAxis = "LeftTriggerAxisPlayer" + PlayerNumber + "OSX";
        inputRightTriggerAxis = "RightTriggerAxisPlayer" + PlayerNumber + "OSX";
            
        inputAttackButton = "joystick " + PlayerNumber + " button 18";
        inputDodgeButton = "joystick " + PlayerNumber + " button 17";

        inputAbility1Button = "joystick " + PlayerNumber + " button 13";
        inputAbility2Button = "joystick " + PlayerNumber + " button 14";
#endif

    }

    void GetInput()
    {
        xAxis = Input.GetAxis(inputAxisX);
        zAxis = Input.GetAxis(inputAxisZ);

        moveVec = new Vector2 (xAxis, zAxis);
        moveDirection = moveVec.normalized;

        aimXAxis = Input.GetAxis(inputAimAxisX);
        aimZAxis = Input.GetAxis(inputAimAxisZ);

        //modifierAxis = Input.GetAxis(inputModifierAxis);
        leftTriggerAxis = Input.GetAxis(inputLeftTriggerAxis);
        rightTriggerAxis = Input.GetAxis(inputRightTriggerAxis);

        //Use Ability
        if (!isLeftTriggerPressed && leftTriggerAxis != 0)
        {
            isLeftTriggerPressed = true;
        }
        else if (isLeftTriggerPressed && leftTriggerAxis != 0)
        {
            holdTime += Time.deltaTime * holdMultiplier;
            isHoldingAbility = true;
        }
        else if (isLeftTriggerPressed && leftTriggerAxis == 0)
        {
            isLeftTriggerPressed = false;
            golemPlayerController.UseAbility(2, playerTeam, holdTime);
            holdTime = 0;
        }

        if (!isRightTriggerPressed && rightTriggerAxis != 0)
        {
            isRightTriggerPressed = true;
        }
        else if (isRightTriggerPressed && rightTriggerAxis != 0)
        {
            holdTime += Time.deltaTime * holdMultiplier;
            isHoldingAbility = true;
        }
        else if (isRightTriggerPressed && rightTriggerAxis == 0)
        {
            isRightTriggerPressed = false;
            golemPlayerController.UseAbility(3, playerTeam, holdTime);
            holdTime = 0;
        }

        if (Input.GetKeyUp(inputAbility1Button))
        {
            golemPlayerController.UseAbility(0, playerTeam, holdTime);
            holdTime = 0;
        }
        
        if (Input.GetKeyUp(inputAbility2Button))
        {
            golemPlayerController.UseAbility(1, playerTeam, holdTime);
            holdTime = 0;
        }

        if (Input.GetKey(inputAbility1Button))
        {
            holdTime += Time.deltaTime * holdMultiplier;
            isHoldingAbility = true;
        }

        if (Input.GetKey(inputAbility2Button))
        {
            holdTime += Time.deltaTime * holdMultiplier;
            isHoldingAbility = true;
        }
        else
        {
            isHoldingAbility = false;
        }

        if (Input.GetKeyDown(inputDodgeButton))
        {
            golemPlayerController.DodgeSetup();
        }

        if (Input.GetKeyDown(inputAttackButton))
        {
            golemPlayerController.UseAttack();
        }
    }

    void CalculateAimVec()
    {
        if (aimXAxis != 0 || aimZAxis != 0)
        {
            aimVec = new Vector3(aimXAxis, 0, aimZAxis).normalized;
        }
        else
        {
            aimVec = Vector3.zero;
        }     
    }
}
