using UnityEngine;

public class GiveBirthToChildNotification : NotificationScript
{
    public override string Title
    {
        get { return "New Family Member"; }
    }

    public override string Description { get { return "You've given birth to a healthy baby."; } }
    public override string OnShowAudioClip { get { return "Audio/Birth"; } }

    protected override void OnShow()
    {
        base.OnShow();

        ChildManager.GiveBirthToChild();
    }
}