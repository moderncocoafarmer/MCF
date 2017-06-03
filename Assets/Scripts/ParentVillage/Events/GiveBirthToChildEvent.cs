using UnityEngine;

public class GiveBirthToChildEvent : EventScript
{
    private const int Threshold = 3;

    public override string Name
    {
        get { return ""; }
    }

    public override string Description
    {
        get
        {
            if (ChildManager.Instance.ChildCount <= Threshold)
            {
                return "You've had a baby.";
            }

            return "You have become pregnant again, do you wish to keep the child?";
        }
    }

    public override float TimeOut { get { return ChildManager.Instance.ChildCount > Threshold ? float.MaxValue : 4; } }
    public override bool ChoicesEnabled { get { return ChildManager.Instance.ChildCount > Threshold; } }
    protected override string OnShowAudioClipPath { get { return ChildManager.Instance.ChildCount <= Threshold ? "Audio/Birth" : null; } }
    protected override string OnYesAudioClipPath { get { return "Audio/Birth"; } }
    protected override string OnNoAudioClipPath { get { return ChildManager.Instance.ChildCount > Threshold ? "Audio/Death" : null; } }

    protected override void OnYes()
    {
        ChildManager.Instance.GiveBirthToChild();

        if (ChildManager.Instance.ChildCount > ChildManager.MaxChildCount)
        {
            // If this child brings us over the max, we queue an event about the child leaving home
            GameObject.Find(EventDialogScript.EventDialogName).GetComponent<EventDialogScript>().QueueEvent(new ChildLeftHomeEventScript());
        }
    }

    protected override void OnNo()
    {
        if (!ChoicesEnabled)
        {
            // If we don't have choices enabled we actually want to perform the behaviour of the yes button
            OnYes();
        }
        else
        {
            base.OnNo();

            GameObject.Find("Graves").GetComponent<GraveCreatorScript>().CreateGrave();
        }
    }
}