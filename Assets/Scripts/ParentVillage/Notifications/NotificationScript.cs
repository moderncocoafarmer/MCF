﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class NotificationScript
{
    public abstract string Title { get; }
    public abstract string Description { get; }
    public virtual string OnShowAudioClip { get { return ""; } }

    protected virtual void OnShow() { }

    public void Show()
    {
        OnShow();
    }
}