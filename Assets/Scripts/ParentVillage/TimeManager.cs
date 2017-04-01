﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float SecondsPerYear = 120;

    private float currentTimeInYear = 0;
    private EventDialogScript dialogScript;

	// Use this for initialization
	void Start ()
    {
        dialogScript = GameObject.Find(EventDialogScript.EventDialogName).GetComponent<EventDialogScript>();
        currentTimeInYear = SecondsPerYear;
    }
	
	// Update is called once per frame
	void Update ()
    {
        currentTimeInYear += Time.deltaTime;
        if (currentTimeInYear > SecondsPerYear)
        {
            currentTimeInYear = 0;
            NewYear();
        }
	}

    private void NewYear()
    {
        dialogScript.QueueEvent(new GiveBirthToChildEvent());
        dialogScript.QueueEvent(new PayBillsEventScript());
    }
}
