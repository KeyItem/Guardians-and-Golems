﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GuardianInputManager))]
[System.Serializable]
public class GuardianPlayerController : GuardianStats 
{
    private GuardianInputManager guardianInputManager;
    private OrbController orbController;
    private TimerClass selectionTimer;

    private float xAxis;
    private float zAxis;

    [Header("Guardian Orb Attributes")]
    public List<GameObject> orbList;

    public PlayerTeam playerTeam;

    public bool isCapturingOrb = false;

    [Header("Selection Values")]
    public GameObject attachedOrb;
    private GameObject selectedOrb;

    public float selectionDistance;

    public float selectionDelay;

    private RaycastHit rayHit;

    public LayerMask orbMask;

    [Header("CoolDowns")]
    private float globalCooldownTime;

    void Start()
    {
        PlayerSetup();
    }

    private void Update()
    {
        GatherInput();
        ManageAttack();
    }

    void PlayerSetup()
    {

        globalCooldownTime = GameObject.FindObjectOfType<GlobalVariables>().guardianGlobalCooldown;

        guardianInputManager = GetComponent<GuardianInputManager>();

        selectionTimer = new TimerClass();

        selectionTimer.ResetTimer(selectionDelay);

        playerTeam = guardianInputManager.playerTeam;
    }

    void GatherInput()
    {
        xAxis = guardianInputManager.xAxis;
        zAxis = guardianInputManager.zAxis;

        SearchForOrb(xAxis, zAxis);
    }

    void ManageAttack()
    {
        if (orbList.Contains(attachedOrb))
        {
            if (orbController.orbState != OrbState.DISABLED)
            {
                canAttack = true;
            }
        }
        else
        {
            canAttack = false;
        }
    }

    void SearchForOrb(float xAxis, float zAxis)
    {
        if (xAxis != 0 || zAxis != 0)
        {
            Vector3 lookVec = new Vector3(xAxis, 0, zAxis);

            if (Physics.Raycast(transform.position, lookVec, out rayHit, selectionDistance, orbMask))
            {
                selectedOrb = rayHit.collider.gameObject;

                if (selectionTimer.TimerIsDone())
                {
                    if (selectedOrb != attachedOrb && attachedOrb != null)
                    {
                        DeattachFromOrb();
                    }

                    AttachToOrb(selectedOrb);
                }
            }
        }
    }

    void AttachToOrb(GameObject orb)
    {
        transform.position = orb.transform.position;
        attachedOrb = orb;
        orbController = attachedOrb.GetComponent<OrbController>();
        selectionTimer.ResetTimer(selectionDelay);
        orbController.attachedGuardianColor.Add(playerTeam);
    }

    void DeattachFromOrb()
    {
        orbController.DeselectOrb(playerTeam);
        orbController = null;
        attachedOrb = null;
    }

    public void CaptureOrb()
    {
        if (attachedOrb != null && !isCapturingOrb)
        {
            orbController.StartOrbCapture(playerTeam, gameObject);
        }
    }

    public void UseAbility(int abilityNumber, Vector3 aimVec, PlayerTeam teamColor, float holdTime)
    {
        if (canAttack && attachedOrb != null)
        {
            if (aimVec != null)
            {
                GameObject spawnObj = orbController.orbObjectBase;

                if (aimVec != Vector3.zero)
                {           
                    guardianAbilites[abilityNumber].CastGuardianAbility(aimVec, spawnObj, teamColor, holdTime);
                }
                else
                {
                    guardianAbilites[abilityNumber].CastGuardianAbility(transform.forward, spawnObj, teamColor, holdTime);
                }
            }
        }     
    }

    public void UseAbilityFromAllOrbs(int abilityNumber, Vector3 aimVec, PlayerTeam teamColor, float holdTime)
    {
        if (canAttack && attachedOrb != null)
        {
            if (aimVec != null)
            {
                for (int i = 0; i < orbList.Count; i++)
                {
                    GameObject spawnObj = orbList[i].GetComponent<OrbController>().orbObjectBase;

                    if (aimVec != Vector3.zero)
                    {
                        guardianAbilites[abilityNumber].CastGuardianAbility(aimVec, spawnObj, teamColor, holdTime);
                    }
                    else
                    {
                        guardianAbilites[abilityNumber].CastGuardianAbility(transform.forward, spawnObj, teamColor, holdTime);
                    }
                }            
            }
        }
    }
}
