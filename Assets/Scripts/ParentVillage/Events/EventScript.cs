﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventScript
{
    public enum DataType
    {
        kYes,
        kNo
    }

    public abstract string Name { get; }
    public abstract string Description { get; }
    public virtual float TimeOut { get { return float.MaxValue; } }
    public virtual string YesButtonText { get { return "Yes"; } }
    public virtual string NoButtonText { get { return "No"; } }
    public virtual bool ChoicesEnabled { get { return false; } }

    public virtual bool DataImplemented { get { return false; } }
    public virtual DataType EventDataType { get { return DataType.kYes; } }
    public virtual string HealthDeltaText { get { return ""; } }
    public virtual string SafetyDeltaText { get { return ""; } }
    public virtual string EducationDeltaText { get { return ""; } }
    public virtual string HappinessDeltaText { get { return ""; } }
    
    private AudioClip onShowAudio;
    public AudioClip OnShowAudioClip
    {
        get
        {
            if (!string.IsNullOrEmpty(OnShowAudioClipPath))
            {
                if (onShowAudio == null)
                {
                    onShowAudio = Resources.Load<AudioClip>(OnShowAudioClipPath);
                }
            }

            return onShowAudio;
        }
    }
    protected virtual string OnShowAudioClipPath { get { return null; } }

    private AudioClip onYesAudio;
    public AudioClip OnYesAudioClip
    {
        get
        {
            if (!string.IsNullOrEmpty(OnYesAudioClipPath))
            {
                if (onYesAudio == null)
                {
                    onYesAudio = Resources.Load<AudioClip>(OnYesAudioClipPath);
                }
            }

            return onYesAudio;
        }
    }
    protected virtual string OnYesAudioClipPath { get { return null; } }

    private AudioClip onNoAudio;
    public AudioClip OnNoAudioClip
    {
        get
        {
            if (!string.IsNullOrEmpty(OnNoAudioClipPath))
            {
                if (onNoAudio == null)
                {
                    onNoAudio = Resources.Load<AudioClip>(OnNoAudioClipPath);
                }
            }

            return onNoAudio;
        }
    }
    protected virtual string OnNoAudioClipPath { get { return null; } }

    public void Yes()
    {
        OnYes();
    }

    protected virtual void OnYes() { }

    public void No()
    {
        OnNo();
    }

    protected virtual void OnNo() { }
}
