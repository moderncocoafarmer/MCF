﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SendChildToSchoolEventScript : InteractableBuildingEventScript
{
    public override string Name
    {
        get { return "School  ( 1 Month )"; }
    }

    protected override string BuildingDescription
    {
        get { return "1 x 5 is 5.  2 x 5 is 10.  3 x 5 is..."; }
    }

    protected override string ChildSelectedDescription
    {
        get
        {
            if (IncomeManager.Money < CostToPerform)
            {
                return "You do not have enough money to send " + ChildManager.SelectedChild.Name + " to school.";
            }

            return "Do you wish to send " + ChildManager.SelectedChild.Name + " to school so they will be more likely to earn money in the future? ( $ " + CostToPerform.ToString() + " for books, equipment and uniform )";
        }
    }

    // $50 per child per year for equipment. ~ 7% of income of parent
    // Paid at beginning of year
    // Child locked in for an entire year
    // 70 children in class per average

    protected override bool ChoicesEnabledImpl { get { return IncomeManager.Money >= CostToPerform; } }

    public override int CostToPerform { get { return 4; } }
    protected override float LockTime { get { return TimeManager.SecondsPerMonth; } }
    protected override string OnShowAudioClipPath { get { return "Audio/School"; } }

    public override BuildingType BuildingType { get { return BuildingType.School; } }
    protected override Vector3 BuildingLocation { get { return GameObject.Find("SchoolDestination").transform.position; } }

    public override bool DataImplemented { get { return true; } }
    public override string HealthDeltaText { get { return "No change"; } }
    public override string SafetyDeltaText { get { return "No change"; } }
    public override string EducationDeltaText { get { return "+5%"; } }
    public override string HappinessDeltaText { get { return "+3%"; } }

    public override string GetOnCompleteDescription(Child child)
    {
        if (child.Education == Child.MaxEducation)
        {
            return child.Name + " has completed education and left for a new a job in the city.  Money will be sent back to help out your family.";
        }
        return child.Name + " has studied hard all year and is closer towards a full education.";
    }

    protected override DataPacket GetDataPacketPerSecond(Child child)
    {
        return new DataPacket(
            0 / LockTime,
            0 / LockTime,
            5 / LockTime,
            3 / LockTime);
    }

    protected override void OnAliveChildTimeComplete(Child child)
    {
        base.OnAliveChildTimeComplete(child);

        if (child.Happiness <= 20 && UnityEngine.Random.Range(0, 1) > 0.75f)
        {
             GameObject.Find(EventDialogScript.EventDialogName).GetComponent<EventDialogScript>().QueueEvent(new ChildExpelledEventScript(child));
        }
    }
}