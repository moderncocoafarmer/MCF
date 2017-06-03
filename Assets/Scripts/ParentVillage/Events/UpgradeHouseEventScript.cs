﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UpgradeHouseEventScript : InteractableBuildingEventScript
{
    public override string Name
    {
        get { return "Home  ( 4 Months )"; }
    }

    protected override string BuildingDescription
    {
        get { return "No place like it."; }
    }

    protected override string ChildSelectedDescription
    {
        get
        {
            if (IncomeManager.Money >= CostToPerform)
            {
                return "Would you like to upgrade your house? ( CFA " + Math.Abs(CostToPerform).ToString() + " )";
            }
            return "You do not have enough money to perform an upgrade to your house.";
        }
    }

    // Different upgrades for your house in unlockable tiers - electricity, well, toilet
    // Child health deteriorates every year based on current health
    // These upgrades provide a passive health bonus (and maybe safety) every year and will eventually negate deterioration and even surpass it

    // House upgrade = $75?

    public override float TimeOut { get { return IncomeManager.Money >= CostToPerform ? float.MaxValue : 4; } }
    protected override bool ChoicesEnabledImpl { get { return IncomeManager.Money >= CostToPerform; } }
    public override int CostToPerform { get { return 46125; } }
    protected override float LockTime { get { return TimeManager.SecondsPerYear / 3; } }
    protected override string OnShowAudioClipPath { get { return "Audio/Home"; } }

    public override BuildingType BuildingType { get { return BuildingType.Home; } }
    protected override Vector3 BuildingLocation { get { return GameObject.Find("Home").transform.position; } }

    public override bool DataImplemented { get { return true; } }
    public override string HealthDeltaText { get { return "+5% per year for all children"; } }
    public override string SafetyDeltaText { get { return "+5% per year for all children"; } }
    public override string EducationDeltaText { get { return "+5% per year for all children"; } }
    public override string HappinessDeltaText { get { return "+5% per year for all children"; } }

    public override string GetOnCompleteDescription(Child child)
    {
        return "The upgrade to your house has been completed.";
    }

    protected override DataPacket GetDataPacketPerSecond(Child child)
    {
        return new DataPacket(0, 0, 0, 0);
    }

    protected override void OnTimeComplete(Child child)
    {
        // Remove degredation
        ChildManager.ChildDegredation -= 5;
    }
}