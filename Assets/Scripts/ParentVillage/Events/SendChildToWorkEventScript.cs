using System;
using System.Collections.Generic;
using UnityEngine;

public class SendChildToWorkEventScript : InteractableBuildingEventScript
{
    public override string Name
    {
        get { return "Cocoa Farm  ( 1 Month )"; }
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

    private const int Salary = 18;
    private Queue<bool> ChildCanLeaveAtEndOfYearRecords = new Queue<bool>();

    protected override bool ChoicesEnabledImpl { get { return true; } }
    public override int CostToPerform { get { return 0; } }
    protected override float LockTime { get { return TimeManager.SecondsPerMonth; } }
    protected override string OnShowAudioClipPath { get { return "Audio/Work"; } }
    
    public override BuildingType BuildingType { get { return BuildingType.Farm; } }
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
            GameObject.Find(EventDialogScript.EventDialogName).GetComponent<EventDialogScript>().QueueEvent(new ChildTraffickedEventScript(selectedChild));
            return false;
        }

        // Decide when child is locked in whether they will be paid or not
        // This is so we can know whether to create the child going home
        ChildCanLeaveAtEndOfYearRecords.Enqueue(UnityEngine.Random.Range(0.0f, 1.0f) < 0.9f);
        return true;
    }

    public override string GetOnCompleteDescription(Child child)
    {
        Debug.Assert(ChildCanLeaveAtEndOfYearRecords.Count > 0);

        if (!ChildCanLeaveAtEndOfYearRecords.Peek())
        {
            return child.Name + " completes a hard month at the cocoa farm, but is not paid.";
        }

        return child.Name + " completes a hard month at the cocoa farm and is paid $ " + ((int)(Salary * (1 + (child.Education * 0.01f)))).ToString() + ".  In real life, only 5% of children are paid for their work...";
    }

    protected override DataPacket GetDataPacketPerSecond(Child child)
    {
        return new DataPacket(
            -10 / LockTime, 
            -10 / LockTime, 
            0, 
            -10 / LockTime);
    }

    protected override void OnAliveChildTimeComplete(Child child)
    {
        base.OnAliveChildTimeComplete(child);

        Debug.Assert(ChildCanLeaveAtEndOfYearRecords.Count > 0);

        if (!ChildCanLeaveAtEndOfYearRecords.Peek())
        {
            GameObject.Find(EventDialogScript.EventDialogName).GetComponent<EventDialogScript>().QueueEvent(new PlagueOfBlackPodEventScript(child));
            return;
        }

        // Only pay child if they leave at end of year
        IncomeManager.AddMoney((int)(Salary * (1 + (child.Education * 0.01f))));
    }

    protected override void OnChildTimeComplete(Child child)
    {
        base.OnChildTimeComplete(child);

        Debug.Assert(ChildCanLeaveAtEndOfYearRecords.Count > 0);
        ChildCanLeaveAtEndOfYearRecords.Dequeue();
    }
}