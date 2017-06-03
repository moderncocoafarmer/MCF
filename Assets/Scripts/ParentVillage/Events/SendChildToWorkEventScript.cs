﻿using System;
using UnityEngine;

public class SendChildToWorkEventScript : InteractableBuildingEventScript
{
    public override string Name
    {
        get { return "Cocoa Farm"; }
    }

    protected override string BuildingDescription
    {
        get { return "Your child goes to the Ivory Coast.  You won't believe what happens next..."; }
    }

    protected override string ChildSelectedDescription
    {
        get
        {
            return "Do you wish to send " + ChildManager.SelectedChild.Name + " to the Ivory Coast to earn vital money for the family?";
        }
    }

    // Child gets money at the end, but also always happiness cost, always health cost and slim chance of trafficking
    // Child earns $190 per year
    // 5% are actually paid - see how this fits with the game
    // Child locked in for a year

    private const int Salary = 116850;
    private static bool childPaid = true;

    protected override bool YesButtonEnabledImpl { get { return true; } }
    protected override string NoButtonTextImpl { get { return "No"; } }
    public override int CostToPerform { get { return 0; } }
    protected override float LockTime { get { return TimeManager.SecondsPerYear; } }
    protected override string OnShowAudioClipPath { get { return "Audio/Work"; } }

    public override BuildingType BuildingType { get { return BuildingType.Work; } }
    protected override Vector3 BuildingLocation { get { return GameObject.Find("Farm").transform.position; } }

    public override bool DataImplemented { get { return true; } }
    public override string HealthDeltaText { get { return "???"; } }
    public override string SafetyDeltaText { get { return "???"; } }
    public override string EducationDeltaText { get { return "???"; } }
    public override string HappinessDeltaText { get { return "???"; } }

    public override bool ConfirmEventQueued(Child selectedChild)
    {
        if (RandomEventGenerator.IsChildTrafficked(selectedChild))
        {
            return false;
        }

        return base.ConfirmEventQueued(selectedChild);
    }

    public override string GetOnCompleteDescription(Child child)
    {
        if (!childPaid)
        {
            return child.Name + " completes a hard year at the cocoa farm, but is not paid.";
        }

        return child.Name + " completes a hard year at the cocoa farm and is paid CFA " + ((int)(Salary * (1 + (child.Education * 0.01f)))).ToString() + ".  In real life, only 5% of children are paid for their work...";
    }

    protected override DataPacket GetDataPacketPerSecond(Child child)
    {
        return new DataPacket(
            -50 / LockTime, 
            -50 / LockTime, 
            0, 
            -50 / LockTime);
    }

    protected override void OnTimeComplete(Child child)
    {
        base.OnTimeComplete(child);

        if (UnityEngine.Random.Range(0.0f, 1.0f) >= 0.9f)
        {
            // Dont increment number of times sent, and don't pay the child
            childPaid = false;
            GameObject.Find(EventDialogScript.EventDialogName).GetComponent<EventDialogScript>().QueueEvent(new PlagueOfBlackPodEventScript(child));
            return;
        }

        childPaid = true;
        IncomeManager.AddMoney((int)(Salary * (1 + (child.Education * 0.01f))));
    }
}