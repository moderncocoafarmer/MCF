using UnityEngine;

public class ReceiveIncomeNotificationScript : NotificationScript
{
    public override string Title
    {
        get { return "Income Received"; }
    }

    public override string Description
    {
        get
        {
            string income = "Your husband's monthly salary of $ " + IncomeManager.CurrentIncome.ToString() + " has been paid.";
            if (ChildManager.ChildrenGraduated > 0)
            {
                income += "  Your children send you back $ " + IncomeManager.IncomeFromChildren.ToString();
            }

            return income;
        }
    }

    public override string OnShowAudioClip { get { return "Audio/Money"; } }

    protected override void OnShow()
    {
        base.OnShow();

        IncomeManager.AddMoney(IncomeManager.CurrentIncome + IncomeManager.IncomeFromChildren);
    }
}