﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum OrbState
{
    EMPTY,
    IN_PROGRESS,
    CONTROLLED,
    CONTESTED,
    HOMEBASE
}

public class OrbController : MonoBehaviour 
{
    private GuardianPlayerController guardianPlayerController;

    [Header("Orb Attributes")]
    public OrbState orbState;
    public PlayerTeam orbColor;

    public List<PlayerTeam> attachedGuardianColor;

    private float captureSpeed;
    [Space(10)]
    public float decaySpeed;
    public float totalCaptureAmount;
    public float redTeamCaptureAmount;
    public float blueTeamCaptureAmount;

    public bool isBeingAssistedByRedGolem;
    public bool isBeingAssistedByBlueGolem;

    [Header("Orb ObjectBase")]
    public GameObject orbObjectBase;

    [Header("Orb UI Attributes")]
    public Image orbCaptureIndicator;

    [Header("Orb Renderer Attributes")]
    public Renderer orbRenderer;
    public Material orbBaseMaterial;
    public Material redOrbMaterial;
    public Material blueOrbMaterial;

    private void Start()
    {
        InitializeOrb();
    }

    private void Update()
    {
        CheckForOrbCapture();
        ManageOrbOutline();
        ManageOrbUI();
    }

    void InitializeOrb()
    {
        orbState = OrbState.EMPTY;
        orbColor = PlayerTeam.NONE;
    }

    void ManageOrbUI()
    {
        if (orbState == OrbState.IN_PROGRESS)
        {
           if (redTeamCaptureAmount > 0)
            {
                orbCaptureIndicator.fillAmount = redTeamCaptureAmount / totalCaptureAmount;
            }
            else if (blueTeamCaptureAmount > 0)
            {
                orbCaptureIndicator.fillAmount = blueTeamCaptureAmount / totalCaptureAmount;
            }
        }
    }

    public void ManageOrbOutline()
    {
       if (attachedGuardianColor.Contains(PlayerTeam.RED) && attachedGuardianColor.Contains(PlayerTeam.BLUE))
        {
            Debug.Log("Both are attached");
        }
       else if (attachedGuardianColor.Contains(PlayerTeam.RED))
        {
            orbRenderer.material.SetColor("_BaseColor", Color.red);
            orbRenderer.material.SetFloat("_Outline", 30);
        }
       else if (attachedGuardianColor.Contains(PlayerTeam.BLUE))
        {
            orbRenderer.material.SetColor("_BaseColor", Color.blue);
            orbRenderer.material.SetFloat("_Outline", 30);
        }
    }

    public void StartOrbCapture(PlayerTeam teamColor, GameObject Guardian)
    {
        if (orbState == OrbState.EMPTY)
        {
            guardianPlayerController = Guardian.GetComponent<GuardianPlayerController>();
            guardianPlayerController.isCapturingOrb = true;
            captureSpeed = guardianPlayerController.captureSpeed;

            orbCaptureIndicator.enabled = true;

            orbState = OrbState.IN_PROGRESS;
            orbColor = teamColor;
        }
        else if (orbState == OrbState.IN_PROGRESS)
        {
            if (teamColor != orbColor)
            {
                if (!attachedGuardianColor.Contains(orbColor))
                {
                    guardianPlayerController.isCapturingOrb = false;

                    guardianPlayerController = Guardian.GetComponent<GuardianPlayerController>();
                    guardianPlayerController.isCapturingOrb = true;
                    captureSpeed = guardianPlayerController.captureSpeed;

                    orbCaptureIndicator.enabled = true;

                    orbColor = teamColor;
                }
            }
        }
    }

    public void CheckForOrbCapture()
    {
        if (orbState == OrbState.IN_PROGRESS)
        {
            switch(orbColor)
            {
                case PlayerTeam.RED:
                    if (attachedGuardianColor.Contains(PlayerTeam.RED) && !attachedGuardianColor.Contains(PlayerTeam.BLUE))
                    {
                        if (blueTeamCaptureAmount > 0)
                        {
                            blueTeamCaptureAmount -= captureSpeed * Time.deltaTime;
                        }
                        else
                        {
                            orbCaptureIndicator.color = Color.red;

                            redTeamCaptureAmount += captureSpeed * Time.deltaTime;
                            blueTeamCaptureAmount = 0;

                            if (redTeamCaptureAmount > totalCaptureAmount)
                            {
                                redTeamCaptureAmount = totalCaptureAmount;

                                CompleteCapture(PlayerTeam.RED);
                            }
                        }
                    }
                    else if (attachedGuardianColor.Contains(PlayerTeam.RED) && attachedGuardianColor.Contains(PlayerTeam.BLUE))
                    {
                        
                    }
                    else
                    {
                        redTeamCaptureAmount -= decaySpeed * Time.deltaTime;

                        if (redTeamCaptureAmount < 0)
                        {
                            ResetOrb();
                        }
                    }         
                    break;

                case PlayerTeam.BLUE:
                    if (attachedGuardianColor.Contains(PlayerTeam.BLUE) && !attachedGuardianColor.Contains(PlayerTeam.RED))
                    {
                        if (redTeamCaptureAmount > 0)
                        {
                            redTeamCaptureAmount -= captureSpeed * Time.deltaTime;
                        }
                        else
                        {
                            orbCaptureIndicator.color = Color.blue;

                            blueTeamCaptureAmount += captureSpeed * Time.deltaTime;
                            redTeamCaptureAmount = 0;

                            if (blueTeamCaptureAmount > totalCaptureAmount)
                            {
                                blueTeamCaptureAmount = totalCaptureAmount;

                                CompleteCapture(PlayerTeam.BLUE);
                            }
                        }
                    }
                    else if (attachedGuardianColor.Contains(PlayerTeam.RED) && attachedGuardianColor.Contains(PlayerTeam.BLUE))
                    {
                       
                    }
                    else
                    {
                        blueTeamCaptureAmount -= decaySpeed * Time.deltaTime;

                        if (blueTeamCaptureAmount < 0)
                        {
                            ResetOrb();
                        }
                    }                 
                    break;
                default:
                    Debug.Log("Something wrong passed through CheckForOrbCapture, was " + orbColor);
                    break;
            }
        }
    }

    void CompleteCapture(PlayerTeam teamColor)
    {
        switch(teamColor)
        {
            case PlayerTeam.RED:
                orbRenderer.material = redOrbMaterial; 
                break;
            case PlayerTeam.BLUE:
                orbRenderer.material = blueOrbMaterial;
                break;
        }

        orbCaptureIndicator.enabled = false;
        captureSpeed = 0;
        guardianPlayerController.isCapturingOrb = false;
        guardianPlayerController.orbList.Add(gameObject);
        orbState = OrbState.CONTROLLED;
    }

    public void ResetOrb()
    {
        guardianPlayerController.isCapturingOrb = false;
        redTeamCaptureAmount = 0;
        blueTeamCaptureAmount = 0;
        orbRenderer.material.SetFloat("_Outline", 0);
        orbState = OrbState.EMPTY;
        orbColor = PlayerTeam.NONE;
    }

    public void DeselectOrb(PlayerTeam guardianColor)
    {
        attachedGuardianColor.Remove(guardianColor);
        orbRenderer.material.SetFloat("_Outline", 0);
    }
}
