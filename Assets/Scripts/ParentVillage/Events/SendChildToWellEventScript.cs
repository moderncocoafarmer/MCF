﻿using System;
using UnityEngine;

public class SendChildToWellEventScript : InteractableBuildingEventScript
{
    public override string Name
    {
        get { return "Well  ( 2 Months )"; }
    }

    protected override string BuildingDescription
    {
        get { return "A rare source of water."; }
    }

    protected override string ChildSelectedDescription
    {
        get
        {
            return "Do you wish to send " + ChildManager.SelectedChild.Name + " to the well to collect water?";
        }
    }
    
    protected override bool ChoicesEnabledImpl { get { return true; } }
    public override int CostToPerform { get { return 0; } }
    protected override float LockTime { get { return TimeManager.SecondsPerYear / 6.0f; } }

    public override BuildingType BuildingType { get { return BuildingType.Well; } }
    protected override Vector3 BuildingLocation { get { return GameObject.Find("Well").transform.position; } }
    protected override string OnShowAudioClipPath { get { return "Audio/Well"; } }

    public override bool DataImplemented { get { return true; } }
    public override string HealthDeltaText { get { return "+10% for all children"; } }
    public override string SafetyDeltaText { get { return "No change"; } }
    public override string EducationDeltaText { get { return "No change"; } }
    public override string HappinessDeltaText { get { return "+10% for all children"; } }

    public override bool ConfirmEventQueued(Child child)
    {
        if (RandomEventGenerator.IsChildTrafficked(child))
        {
            return false;
        }

        return base.ConfirmEventQueued(child);
    }

    public override string GetOnCompleteDescription(Child child)
    {
        return child.Name + " collects water for your family.";
    }

    protected override DataPacket GetDataPacketPerSecond(Child child)
    {
        return new DataPacket(0, 0, 0, 0);
    }

    protected override void OnTimeComplete(Child child)
    {
        base.OnTimeComplete(child);

        ChildManager.ApplyEventToAllChildren(new DataPacket(10, 0, 0, 10));
    }
}