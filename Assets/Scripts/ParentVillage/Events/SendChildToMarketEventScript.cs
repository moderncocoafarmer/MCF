﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendChildToMarketEventScript : InteractableBuildingEventScript
{
    public override string Name { get { return "Market  ( 3 Months )"; } }

    protected override string BuildingDescription
    {
        get
        {
            return "All manner of food can be bought here.";
        }
    }

    protected override string ChildSelectedDescription
    {
        get
        {
            if (IncomeManager.Money < CostToPerform)
            {
                return "Here you can buy food for your family.";
            }
            return "Do you wish to send " + ChildManager.SelectedChild.Name + " to the market to buy food for the family? ( $ " + CostToPerform.ToString() + " )";
        }
    }

    public override BuildingType BuildingType { get { return BuildingType.Market; } }
    protected override Vector3 BuildingLocation { get { return GameObject.Find("Market").transform.position; } }

    // $52 a year for food per person
    public override int CostToPerform { get { return 52 * ChildManager.ChildCount; } }
    protected override float LockTime { get { return TimeManager.SecondsPerYear * 0.25f; } }

    protected override bool ChoicesEnabledImpl { get { return IncomeManager.Money >= CostToPerform; } }

    protected override string OnShowAudioClipPath { get { return "Audio/Market"; } }

    public override bool DataImplemented { get { return true; } }
    public override string HealthDeltaText { get { return "+10% for all children"; } }
    public override string SafetyDeltaText { get { return "No change"; } }
    public override string EducationDeltaText { get { return "No change"; } }
    public override string HappinessDeltaText { get { return "+20% for all children"; } }

    public override bool ConfirmEventQueued(Child selectedChild)
    {
        if (RandomEventGenerator.IsChildTrafficked(selectedChild))
        {
            GameObject.Find(EventDialogScript.EventDialogName).GetComponent<EventDialogScript>().QueueEvent(new ChildTraffickedEventScript(selectedChild));
            return false;
        }

        return base.ConfirmEventQueued(selectedChild);
    }

    public override string GetOnCompleteDescription(Child child)
    {
        return child.Name + " returns home with food bought at the market.  Today is a good day.";
    }

    protected override DataPacket GetDataPacketPerSecond(Child child)
    {
        // No incremental change
        return new DataPacket(0, 0, 0, 0);
    }

    protected override void OnTimeComplete(Child child)
    {
        ChildManager.ApplyEventToAllChildren(new DataPacket(10, 0, 0, 20));
    }
}
