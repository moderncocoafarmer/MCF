﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ChildExpelledEventScript : EventScript
{
    private string description;
    public override string Description { get { return description; } }
    public override string Name { get { return "Child Expelled"; } }

    private Child childToBeExpelledChild;

    public ChildExpelledEventScript(Child expelledChild)
    {
        childToBeExpelledChild = expelledChild;
        description = expelledChild.Name + " has been expelled from school for fighting.";
    }

    protected override void OnNo()
    {
        base.OnNo();

        childToBeExpelledChild.Apply(new DataPacket(0, 0, -childToBeExpelledChild.Education, 0));
    }
}