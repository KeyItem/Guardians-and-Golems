﻿using System.Collections;
using UnityEngine;

public class GolemAbility : AbilityBase 
{
    private GolemResources golemResources;

    [Header("Ability Type")]
    public AbilityType abilityType;

    [Header("Ability Mask")]
    public LayerMask redMask;
    public LayerMask blueMask;

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        golemResources = transform.parent.parent.GetComponent<GolemResources>();
    }

    public override void CastAbility(Vector3 aimVec, PlayerTeam teamColor)
    {
        AbilityValues abilityValues;
        Vector3 spawnVec;
        Quaternion spawnRot;

        switch(abilityType)
        {
            case AbilityType.BUFF:
                break;

            case AbilityType.DEBUFF:
                break;

            case AbilityType.PROJECTILE:

                abilityValues = CreateAbilityStruct();

                if (golemResources.CanCast(abilityValues.manaCost) && golemResources)
                {
                    spawnRot = Quaternion.LookRotation(aimVec);

                    spawnVec = transform.position + new Vector3(0, 1, 0) + new Vector3(0, 0, abilityValues.spawnDistanceFromPlayer);

                    GameObject newProjectile = Instantiate(ability, spawnVec, spawnRot) as GameObject;
                    newProjectile.GetComponent<GolemAbilityBase>().abilityValues = abilityValues;
                    newProjectile.GetComponent<GolemAbilityBase>().InitializeAbility();

                    if (teamColor == PlayerTeam.RED)
                    {
                        newProjectile.layer = 8;
                    }
                    else if (teamColor == PlayerTeam.BLUE)
                    {
                        newProjectile.layer = 9;
                    }
                }              
                break;

            case AbilityType.STATIC:

                abilityValues = CreateAbilityStruct();

                if (golemResources.CanCast(abilityValues.manaCost))
                {
                    spawnRot = Quaternion.LookRotation(aimVec);

                    spawnVec = aimVec;
                    spawnVec.Normalize();
                    spawnVec = spawnVec * abilityValues.spawnDistanceFromPlayer;
                    spawnVec.y = -5f;

                    GameObject newStaticAbility = Instantiate(ability, transform.position + spawnVec, spawnRot) as GameObject;

                    if (teamColor == PlayerTeam.RED)
                    {
                        newStaticAbility.layer = 8;
                    }
                    else if (teamColor == PlayerTeam.BLUE)
                    {
                        newStaticAbility.layer = 9;
                    }
                    newStaticAbility.GetComponent<GolemAbilityBase>().abilityValues = abilityValues;
                    newStaticAbility.GetComponent<GolemAbilityBase>().InitializeAbility();            
                }
                break;

            case AbilityType.ZONE:

                abilityValues = CreateAbilityStruct();

                if (golemResources.CanCast(abilityValues.manaCost))
                {
                    if (aimVec != Vector3.zero)
                    {
                        spawnRot = Quaternion.LookRotation(aimVec);

                        Vector3 zoneSpawnVec = aimVec;
                        zoneSpawnVec.Normalize();
                        zoneSpawnVec = zoneSpawnVec * abilityValues.spawnDistanceFromPlayer;
                        zoneSpawnVec.y = 0f;

                        GameObject newZoneAbility = Instantiate(ability, transform.position + zoneSpawnVec, spawnRot) as GameObject;
                        newZoneAbility.GetComponent<GolemAbilityBase>().abilityValues = abilityValues;
                        newZoneAbility.GetComponent<GolemAbilityBase>().InitializeAbility();
                    }
                    else
                    {
                        Debug.Log("Should cast on myself");
                        GameObject newZoneAbility = Instantiate(ability, transform.position, Quaternion.identity) as GameObject;
                        newZoneAbility.GetComponent<GolemAbilityBase>().abilityValues = abilityValues;
                        newZoneAbility.GetComponent<GolemAbilityBase>().InitializeAbility();
                    }
                }         
                break;
        }       
    }

    public AbilityValues CreateAbilityStruct()
    {
        AbilityValues abilityInfo;

        abilityInfo.damageType = damageType;
        abilityInfo.damageAmount = damageAmount;
        abilityInfo.activeTime = activeTime;
        abilityInfo.projectileSpeed = projectileSpeed;
        abilityInfo.spawnDistanceFromPlayer = spawnDistanceFromPlayer;
        abilityInfo.raiseAmount = raiseAmount;
        abilityInfo.raiseSpeed = raiseSpeed;
        abilityInfo.zoneRadius = zoneRadius;
        abilityInfo.zoneHeight = zoneHeight;
        abilityInfo.zoneStrength = zoneStrength;
        abilityInfo.isMelee = isMelee;
        abilityInfo.isRanged = isRanged;
        abilityInfo.healthCost = healthCost;
        abilityInfo.manaCost = manaCost;
        abilityInfo.canStun = canStun;
        abilityInfo.canSlow = canSlow;
        abilityInfo.canDrainHealth = canDrainHealth;
        abilityInfo.canDrainMana = canDrainMana;
        abilityInfo.canBlind = canBlind;

        return abilityInfo;
    }
}
