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
    public float holdTime;
    public Vector2 moveVec;
    [HideInInspector]
    public Vector2 moveDirection;

    [HideInInspector]
    public float aimXAxis;
    [HideInInspector]
    public float aimZAxis;

    private float modifierAxis;

    [HideInInspector]
    public float blockAxis;

    [Header("Debug Values")]
    private bool isBlockAxisActive = false;
    public bool isHoldingAbility = false;

    private Vector3 aimVec;

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
    }

    void GetInput()
    {

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

        xAxis = Input.GetAxis("HorizontalPlayer" + PlayerNumber + "Win");
        zAxis = Input.GetAxis("VerticalPlayer" + PlayerNumber + "Win");

        moveVec = new Vector2 (xAxis, zAxis);
        moveDirection = moveVec.normalized;

        aimXAxis = Input.GetAxis("HorizontalAimPlayer" + PlayerNumber + "Win");
        aimZAxis = Input.GetAxis("VerticalAimPlayer" + PlayerNumber + "Win");

        modifierAxis = Input.GetAxis("ModifierAxisPlayer" + PlayerNumber + "Win");
        blockAxis = Input.GetAxis("BlockAxisPlayer" + PlayerNumber + "Win");

        //Use Ability
        if (modifierAxis != 0 && Input.GetKeyUp("joystick " + PlayerNumber + " button 4"))
        {
            golemPlayerController.UseAbility(2, aimVec, playerTeam, holdTime);
            holdTime = 0;
        }
        else if (Input.GetKeyUp("joystick " + PlayerNumber + " button 4"))
        {
            golemPlayerController.UseAbility(0, aimVec, playerTeam, holdTime);
            holdTime = 0;
        }

        if (modifierAxis != 0 && Input.GetKeyUp("joystick " + PlayerNumber + " button 5"))
        {
            golemPlayerController.UseAbility(3, aimVec, playerTeam, holdTime);
            holdTime = 0;
        }
        else if (Input.GetKeyUp("joystick " + PlayerNumber + " button 5"))
        {
            golemPlayerController.UseAbility(1, aimVec, playerTeam, holdTime);
            holdTime = 0;
        }

        //Hold Down
        if (modifierAxis != 0 && Input.GetKey("joystick " + PlayerNumber + " button 4"))
        {
            holdTime += Time.fixedDeltaTime;
            isHoldingAbility = true;
        }
        else if (Input.GetKey("joystick " + PlayerNumber + " button 4"))
        {
            holdTime += Time.fixedDeltaTime;
            isHoldingAbility = true;
        }

        if (modifierAxis != 0 && Input.GetKey("joystick " + PlayerNumber + " button 5"))
        {
            holdTime += Time.fixedDeltaTime;
            isHoldingAbility = true;
        }
        else if (Input.GetKey("joystick " + PlayerNumber + " button 5"))
        {
            holdTime += Time.fixedDeltaTime;
            isHoldingAbility = true;
        }
        else
        {
            isHoldingAbility = false;
        }

        if (Input.GetKeyDown("joystick " + PlayerNumber + " button 1"))
        {
            golemPlayerController.DodgeSetup();
        }

        if (Input.GetKeyDown("joystick " + PlayerNumber + " button 2"))
        {
            golemPlayerController.UseAttack();
        }

        if (blockAxis != 0)
        {
            if (isBlockAxisActive == false)
            {
                isBlockAxisActive = true;
            }
            else if (isBlockAxisActive)
            {
                golemPlayerController.Block();
            }           
        }
        else if (blockAxis == 0)
        {
            if (isBlockAxisActive)
            {
                golemPlayerController.Unblock();
                isBlockAxisActive = false;
            } 
        }

#endif

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX

          xAxis = Input.GetAxis("HorizontalPlayer" + PlayerNumber + "OSX");
        zAxis = Input.GetAxis("VerticalPlayer" + PlayerNumber + "OSX");

        moveVec = new Vector2 (xAxis, zAxis);
        moveDirection = moveVec.normalized;

        aimXAxis = Input.GetAxis("HorizontalAimPlayer" + PlayerNumber + "OSX");
        aimZAxis = Input.GetAxis("VerticalAimPlayer" + PlayerNumber + "OSX");

        modifierAxis = Input.GetAxis("ModifierAxisPlayer" + PlayerNumber + "OSX");
        blockAxis = Input.GetAxis("BlockAxisPlayer" + PlayerNumber + "OSX");

        //Use Ability
        if (modifierAxis != 0 && Input.GetKeyUp("joystick " + PlayerNumber + " button 13"))
        {
            golemPlayerController.UseAbility(2, aimVec, playerTeam, holdTime);
            holdTime = 0;
        }
        else if (Input.GetKeyUp("joystick " + PlayerNumber + " button 13"))
        {
            golemPlayerController.UseAbility(0, aimVec, playerTeam, holdTime);
            holdTime = 0;
        }

        if (modifierAxis != 0 && Input.GetKeyUp("joystick " + PlayerNumber + " button 14"))
        {
            golemPlayerController.UseAbility(3, aimVec, playerTeam, holdTime);
            holdTime = 0;
        }
        else if (Input.GetKeyUp("joystick " + PlayerNumber + " button 14"))
        {
            golemPlayerController.UseAbility(1, aimVec, playerTeam, holdTime);
            holdTime = 0;
        }

        //Hold Down
        if (modifierAxis != 0 && Input.GetKey("joystick " + PlayerNumber + " button 13"))
        {
            holdTime += Time.fixedDeltaTime;
            isHoldingAbility = true;
        }
        else if (Input.GetKey("joystick " + PlayerNumber + " button 13"))
        {
            holdTime += Time.fixedDeltaTime;
            isHoldingAbility = true;
        }

        if (modifierAxis != 0 && Input.GetKey("joystick " + PlayerNumber + " button 14"))
        {
            holdTime += Time.fixedDeltaTime;
            isHoldingAbility = true;
        }
        else if (Input.GetKey("joystick " + PlayerNumber + " button 14"))
        {
            holdTime += Time.fixedDeltaTime;
            isHoldingAbility = true;
        }
        else
        {
            isHoldingAbility = false;
        }

        if (Input.GetKeyDown("joystick " + PlayerNumber + " button 17"))
        {
            golemPlayerController.DodgeSetup();
        }

        if (Input.GetKeyDown("joystick " + PlayerNumber + " button 18"))
        {
            golemPlayerController.UseAttack();
        }

        if (blockAxis != 0)
        {
            if (isBlockAxisActive == false)
            {
                isBlockAxisActive = true;
            }
            else if (isBlockAxisActive)
            {
                golemPlayerController.Block();
            }           
        }
        else if (blockAxis == 0)
        {
            if (isBlockAxisActive)
            {
                golemPlayerController.Unblock();
                isBlockAxisActive = false;
            } 
        }

#endif
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
