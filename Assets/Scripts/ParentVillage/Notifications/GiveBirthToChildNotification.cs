using UnityEngine;

public class GiveBirthToChildNotification : NotificationScript
{
    public override string Title
    {
        get { return ""; }
    }

    public override string Description { get { return "You've had a baby."; } }
    public override string OnShowAudioClip { get { return "Audio/Birth"; } }

    protected override void OnShow()
    {
        base.OnShow();

        ChildManager.GiveBirthToChild();
    }
}