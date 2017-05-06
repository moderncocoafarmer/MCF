﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public const float SecondsPerYear = 90;

    public static float CurrentTimeInYear { get; private set; }
    public static float DeltaTime { get; private set; }
    public static bool Paused { get; set; }

    private bool midYearReached = false;
    private bool quarterYearReached = false;
    private EventDialogScript dialogScript;
    private NotificationDialogScript notificationScript;
    private bool levelStarted = false;

	// Use this for initialization
	void Start ()
    {
        dialogScript = GameObject.Find(EventDialogScript.EventDialogName).GetComponent<EventDialogScript>();
        notificationScript = GameObject.Find(NotificationDialogScript.NotificationDialogName).GetComponent<NotificationDialogScript>();
        CurrentTimeInYear = 0;
        Paused = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!levelStarted)
        {
            if (Time.timeSinceLevelLoad > 2)
            {
                levelStarted = true;
                Paused = false;
                dialogScript.QueueEvent(new InstructionEventScript());
            }
        }

        if (!Paused)
        {
            DeltaTime = Time.deltaTime;
        }
        else
        {
            DeltaTime = 0;
        }

        CurrentTimeInYear += DeltaTime;

        if (CurrentTimeInYear > SecondsPerYear)
        {
            CurrentTimeInYear = 0;
            NewYear();
        }
        else if (!quarterYearReached && CurrentTimeInYear > SecondsPerYear * 0.25f)
        {
            QuarterYear();
        }
        else if (!midYearReached && CurrentTimeInYear > SecondsPerYear * 0.5f)
        {
            MidYear();
        }
	}

    private void NewYear()
    {
        notificationScript.QueueNotification(new ReceiveIncomeNotificationScript());
        midYearReached = false;
        quarterYearReached = false;
    }

    private void QuarterYear()
    {
        dialogScript.QueueEvent(new GiveBirthToChildEvent());
        quarterYearReached = true;
    }

    private void MidYear()
    {
        dialogScript.QueueEvent(new PayBillsEventScript());
        midYearReached = true;
    }
}
