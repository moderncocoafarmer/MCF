﻿using System;

public class ChildTraffickedEventScript : EventScript
{
    public override string Name
    {
        get { return "Child Trafficked"; }
    }

    public override string Description
    {
        get
        {
            return childThatWillBeTaken.Name + " has been taken by an illegal trafficker.  Do you want to inform the Police? ( CFA " + Cost.ToString() + " )";
        }
    }

    public override string YesButtonText { get { return "Yes"; } }
    public override bool NoButtonEnabled { get { return true; } }

    private const float Cost = 615000;
    private Child childThatWillBeTaken;

    // Yes = pay income for die-roll chance of recovering child; mention income cost in description
    // No = no-op

    public ChildTraffickedEventScript()
    {
        Random random = new Random();
        childThatWillBeTaken = ChildManager.GetChild(random.Next(0, ChildManager.ChildCount));
    }

    protected override void OnYes()
    {
        base.OnYes();

        IncomeManager.AddMoney(-Cost);

        Random random = new Random();
        if (random.NextDouble() >= 0.2)
        {
            ChildManager.RemoveChild(childThatWillBeTaken);
        }
    }

    protected override void OnNo()
    {
        base.OnNo();

        ChildManager.RemoveChild(childThatWillBeTaken);
    }
}