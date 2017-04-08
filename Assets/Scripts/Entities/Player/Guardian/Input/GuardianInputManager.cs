﻿using System.Collections;
using UnityEngine;

public class GuardianInputManager : MonoBehaviour 
{
    private GuardianPlayerController guardianController;

    [Header("Guardian Player Values")]
    public PlayerNum playerNum;
    public PlayerTeam playerTeam;

    public string PlayerName;
    public int PlayerNumber;

    [Header("Guardian Input Values")]
    [HideInInspector]
    public float xAxis;
    [HideInInspector]
    public float zAxis;
    public float holdTime;
    public float holdTimeMultiplier = 2;

    [HideInInspector]
    public float aimXAxis;
    [HideInInspector]
    public float aimZAxis;

    [HideInInspector]
    public float leftTriggerAxis;
    [HideInInspector]
    public float rightTriggerAxis;

    //private float modifierAxis;
    //private float multicastAxis;

    [HideInInspector]
    public Vector3 aimVec;

    private string inputAxisX = "";
    private string inputAxisZ = "";
    private string inputAimAxisX = "";
    private string inputAimAxisZ = "";
    //private string inputModifierAxis = "";
    //private string inputMulticastAxis = "";
    private string inputLeftTriggerAxis = "";
    private string inputRightTriggerAxis = "";
    private string inputCaptureButton = "";
    private string inputAbility1Button = "";
    private string inputAbility2Button = "";

    private bool isHoldingAbility = false;
    private bool isLeftTriggerPressed = false;
    private bool isRightTriggerPressed = false;

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
        guardianController = GetComponent<GuardianPlayerController>();

        switch (playerNum)
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
        //inputMulticastAxis = "BlockAxisPlayer" + PlayerNumber + "Win";

        inputLeftTriggerAxis = "LeftTriggerAxisPlayer" + PlayerNumber + "Win";
        inputRightTriggerAxis = "RightTriggerAxisPlayer" + PlayerNumber + "Win";

        inputCaptureButton = "joystick " + PlayerNumber + " button 2";

        inputAbility1Button = "joystick " + PlayerNumber + " button 4";
        inputAbility2Button = "joystick " + PlayerNumber + " button 5";
#endif

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX

        inputAxisX = "HorizontalPlayer" + PlayerNumber + "OSX";
        inputAxisZ = "VerticalPlayer" + PlayerNumber + "OSX";

        inputAimAxisX = "HorizontalAimPlayer" + PlayerNumber + "OSX";
        inputAimAxisZ = "VerticalAimPlayer" + PlayerNumber + "OSX";

        //inputModifierAxis = "ModifierAxisPlayer" + PlayerNumber + "OSX";
        //inputMulticastAxis = "BlockAxisPlayer" + PlayerNumber + "OSX";

        inputLeftTriggerAxis = "LeftTriggerAxisPlayer" + PlayerNumber + "OSX";
        inputRightTriggerAxis = "RightTriggerAxisPlayer" + PlayerNumber + "OSX";

        inputCaptureButton = "joystick " + PlayerNumber + " button 18";

        inputAbility1Button = "joystick " + PlayerNumber + " button 13";
        inputAbility2Button = "joystick " + PlayerNumber + " button 14";

#endif

    }

    void GetInput()
    {
        xAxis = Input.GetAxis(inputAxisX);
        zAxis = Input.GetAxis(inputAxisZ);

        aimXAxis = Input.GetAxis(inputAimAxisX);
        aimZAxis = Input.GetAxis(inputAimAxisZ);

        //modifierAxis = Input.GetAxis(inputModifierAxis);
        //multicastAxis = Input.GetAxis(inputMulticastAxis);
        leftTriggerAxis = Input.GetAxis(inputLeftTriggerAxis);
        rightTriggerAxis = Input.GetAxis(inputRightTriggerAxis);

        if (Input.GetKeyDown(inputCaptureButton))
        {
            guardianController.CaptureOrb();
        }

        if (!isLeftTriggerPressed && leftTriggerAxis != 0)
        {
            isLeftTriggerPressed = true;
        }
        else if (isLeftTriggerPressed && leftTriggerAxis != 0)
        {
            holdTime += Time.deltaTime * holdTimeMultiplier;
            isHoldingAbility = true;
        }
        else if (isLeftTriggerPressed && leftTriggerAxis == 0)
        {
            isLeftTriggerPressed = false;
            guardianController.UseAbility(2, playerTeam, holdTime);
            holdTime = 0;
        }

        if (!isRightTriggerPressed && rightTriggerAxis != 0)
        {
            isRightTriggerPressed = true;
        }
        else if (isRightTriggerPressed && rightTriggerAxis != 0)
        {
            holdTime += Time.deltaTime * holdTimeMultiplier;
            isHoldingAbility = true;
        }
        else if (isRightTriggerPressed && rightTriggerAxis == 0)
        {
            isRightTriggerPressed = false;
            guardianController.UseAbility(3, playerTeam, holdTime);
            holdTime = 0;
        }

        if (Input.GetKeyUp(inputAbility1Button))
        {
            guardianController.UseAbility(0, playerTeam, holdTime);
            holdTime = 0;
        }
      
        if (Input.GetKeyUp(inputAbility2Button))
        {
            guardianController.UseAbility(1, playerTeam, holdTime);
            holdTime = 0;
        }

        if (Input.GetKey(inputAbility1Button))
        {
            holdTime += Time.deltaTime * holdTimeMultiplier;
            isHoldingAbility = true;
        }
        
        if (Input.GetKey(inputAbility2Button))
        {
            holdTime += Time.deltaTime * holdTimeMultiplier;
            isHoldingAbility = true;
        }
    }

    void CalculateAimVec()
    {
        if (aimXAxis != 0 || aimZAxis != 0)
        {
            aimVec = new Vector3(aimXAxis, 0, aimZAxis);
            aimVec.Normalize();
        }
        else
        {
            aimVec = Vector3.zero;
        }
    }
}
