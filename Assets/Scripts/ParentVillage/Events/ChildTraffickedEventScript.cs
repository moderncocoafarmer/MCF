using System;
using UnityEngine;

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
            if (IncomeManager.Money < Cost)
            {
                return childThatWillBeTaken.Name + " your child has been taken by an illegal trafficker and smuggled across the boarder. You are unlikely to see him again.";
            }
            return childThatWillBeTaken.Name + " your child has been taken by an illegal trafficker and smuggled across the boarder.  Do you want to inform the Police? ( $ " + Cost.ToString() + " )";
        }
    }

    public override bool ChoicesEnabled { get { return IncomeManager.Money >= Cost; } }
    protected override string OnShowAudioClipPath { get { return "Audio/ChildTrafficked"; } }
    protected override string OnYesAudioClipPath { get { return "Audio/Money"; } }

    private const int Cost = 88;
    private Child childThatWillBeTaken;

    private static int childrenTrafficked = 0;

    // Yes = pay income for die-roll chance of recovering child; mention income cost in description
    // No = no-op

    public ChildTraffickedEventScript(Child child)
    {
        childThatWillBeTaken = child;
    }

    protected override void OnYes()
    {
        base.OnYes();

        IncomeManager.AddMoney(-Cost);

        if (UnityEngine.Random.Range(0.0f, 1.0f) >= 0.2)
        {
            TrafficChild(childThatWillBeTaken);
        }
    }

    protected override void OnNo()
    {
        base.OnNo();

        TrafficChild(childThatWillBeTaken);
    }

    private void TrafficChild(Child child)
    {
        childrenTrafficked++;
        ChildManager.KillChild(childThatWillBeTaken, Child.ChildState.kTrafficed);

        if (childrenTrafficked == 3)
        {
            GameObject.Find(EventDialogScript.EventDialogName).GetComponent<EventDialogScript>().QueueEvent(new UNPeaceKeepersArriveEventScript());
        }
    }
}