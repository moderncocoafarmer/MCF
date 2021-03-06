﻿using System;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    Idle,
    Home,
    Mosque,
    Farm,
    School,
    Hospital,
    Market,
    Well
}

public abstract class InteractableBuildingEventScript : EventScript
{
    public sealed override string Description
    {
        get
        {
            Child selectedChild = ChildManager.SelectedChild;
            string buildingDescription = BuildingDescription;

            if (selectedChild == null)
            {
                return buildingDescription + "\n\nSelect a child using the icons at the top and click on this building to send them here.";
            }
            else if (selectedChild.BuildingType != BuildingType.Idle)
            {
                return buildingDescription;
            }
            else
            {
                return ChildSelectedDescription;
            }
        }
    }

    protected abstract string ChildSelectedDescription { get; }
    protected abstract string BuildingDescription { get; }

    public sealed override bool ChoicesEnabled { get { return ChildManager.SelectedChild == null ? false : ChoicesEnabledImpl; } }
    protected abstract bool ChoicesEnabledImpl { get; }
    public override string YesButtonText { get { return "Send " + ChildManager.SelectedChild.Name; } }

    public abstract int CostToPerform { get; }
    public abstract BuildingType BuildingType { get; }
    protected abstract Vector3 BuildingLocation { get; }

    protected abstract float LockTime { get; }
    protected List<Child> LockedInChildren = new List<Child>();
    protected List<float> Timers = new List<float>();
    protected List<float> Tickers = new List<float>();

    private List<GameObject> ChildIndicatorUIs = new List<GameObject>();
    private GameObject ChildIndicatorUI;
    private Transform FirstChildIndicatorPosition;
    private Transform EvenChildIndicatorPositions;
    private Transform OddChildIndicatorPositions;

    public abstract string GetOnCompleteDescription(Child child);
    protected abstract DataPacket GetDataPacketPerSecond(Child child);

    // Called for any child whose state is alive when their time ends
    // Called before OnChildTimeComplete
    protected virtual void OnAliveChildTimeComplete(Child child) { }

    // Called for any child when their time ends, no matter their state
    protected virtual void OnChildTimeComplete(Child child) { }

    public virtual bool ConfirmEventQueued(Child selectedChild)
    {
        return true;
    }

    protected override void OnYes()
    {
        base.OnYes();

        IncomeManager.AddMoney(-CostToPerform);

        Child child = ChildManager.SelectedChild;
        if (!ConfirmEventQueued(child))
        {
            // We shouldn't lock this child in
            return;
        }

        child.LockIn(BuildingType);

        LockedInChildren.Add(child);
        Timers.Add(0);
        Tickers.Add(0);

        AddChildIndicator(child);

        GameObject.Find("InteractableBuildings").transform.FindChild("Home").GetComponent<ChildVillagerCreatorScript>().CreateChildVillager(BuildingLocation);
    }
    
    public void Update()
    {
        List<int> childrenToRemoveIndexes = new List<int>();
        
        // Go through and update children locked in time
        for (int i = 0; i < Timers.Count; ++i)
        {
            Timers[i] += TimeManager.DeltaTime;
            Tickers[i] += TimeManager.DeltaTime;
            ChildIndicatorUIs[i].GetComponent<ChildIndicatorUIScript>().IncrementBar(100f * TimeManager.DeltaTime / LockTime);

            if (Tickers[i] >= 1)
            {
                Tickers[i] = 0;
                LockedInChildren[i].Apply(GetDataPacketPerSecond(LockedInChildren[i]));
            }

            if (Timers[i] > LockTime)
            {
                LockedInChildren[i].LockIn(BuildingType.Idle);
                childrenToRemoveIndexes.Add(i);
            }
        }
        
        foreach (int childIndex in childrenToRemoveIndexes)
        {
            Child child = LockedInChildren[childIndex];
            LockedInChildren.RemoveAt(childIndex);
            Timers.RemoveAt(childIndex);
            Tickers.RemoveAt(childIndex);

            Transform home = GameObject.Find("InteractableBuildings").transform.FindChild("Home");
            home.GetComponent<ChildVillagerCreatorScript>().CreateChildVillager(BuildingLocation, home.position);

            RemoveChildIndicator(child);

            if (child.State == Child.ChildState.kAlive)
            {
                // If they are still alive, we should trigger the on complete behaviour
                OnAliveChildTimeComplete(child);
                GameObject.Find(NotificationDialogScript.NotificationDialogName).GetComponent<NotificationDialogScript>().QueueNotification(new TaskCompleteNotification(GetOnCompleteDescription(child)));
            }

            OnChildTimeComplete(child);
        }
    }

    public void SetUpIndicatorUI(GameObject childIndicatorUI, Transform firstChildIndicator, Transform evenChildIndicator, Transform oddChildIndicator)
    {
        ChildIndicatorUI = childIndicatorUI;
        FirstChildIndicatorPosition = firstChildIndicator;
        EvenChildIndicatorPositions = evenChildIndicator;
        OddChildIndicatorPositions = oddChildIndicator;
    }

    private void AddChildIndicator(Child child)
    {
        Transform parent = OddChildIndicatorPositions;

        // Called after the child is added to the Locked in children list
        if (LockedInChildren.Count == 1)
        {
            parent = FirstChildIndicatorPosition;
        }
        else if (LockedInChildren.Count % 2 == 0)
        {
            // Even so add to rhs
            parent = EvenChildIndicatorPositions;
        }

        Debug.Assert(ChildIndicatorUI != null);
        Debug.Assert(parent != null);
        Debug.Assert(child != null);

        GameObject indicator = GameObject.Instantiate(ChildIndicatorUI, parent);
        indicator.GetComponent<ChildIndicatorUIScript>().Child = child;
        ChildIndicatorUIs.Add(indicator);

        LayoutIndicators();
    }

    private void RemoveChildIndicator(Child child)
    {
        GameObject indicatorUI = ChildIndicatorUIs.Find(x => x.GetComponent<ChildIndicatorUIScript>().Child == child);
        ChildIndicatorUIs.Remove(indicatorUI);
        GameObject.Destroy(indicatorUI);

        LayoutIndicators();
    }

    private void LayoutIndicators()
    {
        if (ChildIndicatorUIs.Count == 0)
        {
            return;
        }

        ChildIndicatorUIs[0].transform.parent = FirstChildIndicatorPosition;
        ChildIndicatorUIs[0].transform.localPosition = Vector3.zero;

        for (int i = 1; i < ChildIndicatorUIs.Count; ++i)
        {
            bool rhs = i % 2 == 1;
            int multiplier = rhs ? 1 : -1;
            int index = (i - 1) / 2;
            Vector3 rendererBounds = ChildIndicatorUIs[i].transform.FindChild("ChildIndicatorPanel").GetComponent<SpriteRenderer>().bounds.extents * 2;
            ChildIndicatorUIs[i].transform.parent = rhs ? EvenChildIndicatorPositions : OddChildIndicatorPositions;
            ChildIndicatorUIs[i].transform.localPosition = new Vector3(rendererBounds.x * 1.1f, 0, 0) * multiplier * index * ChildIndicatorUIs[i].transform.localScale.x;
        }
    }
}