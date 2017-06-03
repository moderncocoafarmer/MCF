﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PayBillsEventScript : EventScript
{
    public override string Name
    {
        get { return "Bills"; }
    }

    public override string Description
    {
        get
        {
            if (IncomeManager.Money < Cost)
            {
                return "Your bills are due ( CFA " + Cost.ToString() + " ).  You do not have enough money to pay them.";
            }

            return "Your bills are due ( CFA " + Cost.ToString() + " ).  Do you wish to pay them?";
        }
    }

    // Average house size is 32 m2
    // $700 a year for 32 m2 apartment
    // $52 a year for food per person
    // $170 a month for 85 m2 apartment

    public override float TimeOut { get { return IncomeManager.Money < Cost ? 6 : float.MaxValue; } }
    public override bool ChoicesEnabled { get { return IncomeManager.Money >= Cost; } }
    protected override string OnYesAudioClipPath { get { return "Audio/Money"; } }

    public int Cost { get { return 430500 + 31980 * ChildManager.ChildCount; } }

    public override bool DataImplemented { get { return true; } }
    public override DataType EventDataType { get { return DataType.kNo; } }
    public override string HealthDeltaText { get { return ChoicesEnabled ? "No change" : "-30% for all children"; } }
    public override string SafetyDeltaText { get { return ChoicesEnabled ? "No change" : "-20% for all children"; } }
    public override string EducationDeltaText { get { return "No change"; } }
    public override string HappinessDeltaText { get { return ChoicesEnabled ? "No change" : "-50% for all children"; } }

    protected override void OnYes()
    {
        base.OnYes();

        if (IncomeManager.Money >= Cost)
        {
            IncomeManager.AddMoney(-Cost);
        }
    }

    protected override void OnNo()
    {
        base.OnNo();

        ChildManager.ApplyEventToAllChildren(new DataPacket(-30, -20, 0, -50));
    }
}