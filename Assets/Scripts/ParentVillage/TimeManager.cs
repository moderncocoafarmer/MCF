﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public const float SecondsPerMonth = 30;
    public const float SecondsPerYear = SecondsPerMonth * 12;

    public static int CurrentYearNumber { get { return (int)(TotalGameTimePassed / SecondsPerYear) + 1; } }
    public static float TotalGameTimePassed { get; private set; }
    public static float CurrentTimeInMonth { get; private set; }
    public static float CurrentTimeInYear { get; private set; }
    public static float DeltaTime { get; private set; }
    public static bool Paused { get; set; }

    private bool midYearReached = false;
    private bool quarterYearReached = false;
    private EventDialogScript dialogScript;
    private NotificationDialogScript notificationScript;

	// Use this for initialization
	void Start ()
    {
        dialogScript = GameObject.Find(EventDialogScript.EventDialogName).GetComponent<EventDialogScript>();
        dialogScript.QueueEvent(new InstructionEventScript());
        notificationScript = GameObject.Find(NotificationDialogScript.NotificationDialogName).GetComponent<NotificationDialogScript>();
        TotalGameTimePassed = 0;
        CurrentTimeInMonth = 0;
        CurrentTimeInYear = 0;
        DeltaTime = 0;
        Paused = false; // Instruction event will keep game paused otherwise when it launches on startup
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!Paused)
        {
            DeltaTime = Time.deltaTime;
        }
        else
        {
            DeltaTime = 0;
        }

        TotalGameTimePassed += DeltaTime;
        CurrentTimeInYear += DeltaTime;
        CurrentTimeInMonth += DeltaTime;

        if (CurrentTimeInMonth > SecondsPerMonth)
        {
            NewMonth();
        }

        if (CurrentTimeInYear > SecondsPerYear)
        {
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

    private void NewMonth()
    {
        CurrentTimeInMonth = 0;
        notificationScript.QueueNotification(new ReceiveIncomeNotificationScript());
    }

    private void NewYear()
    {
        CurrentTimeInYear = 0;
        midYearReached = false;
        quarterYearReached = false;
    }

    private void QuarterYear()
    {
        if (ChildManager.CanHaveChild && (CurrentYearNumber % 2 == 0))
        {
            // Have a child every two years
            notificationScript.QueueNotification(new GiveBirthToChildNotification());
        }

        quarterYearReached = true;
    }

    private void MidYear()
    {
        dialogScript.QueueEvent(new PayBillsEventScript());
        midYearReached = true;
    }
}
