using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TaskCompleteNotification : NotificationScript
{
    public override string Title { get { return ""; } }

    private string description;
    public override string Description { get { return description; } }

    public TaskCompleteNotification(string taskCompletedDescription)
    {
        description = taskCompletedDescription;
    }
}