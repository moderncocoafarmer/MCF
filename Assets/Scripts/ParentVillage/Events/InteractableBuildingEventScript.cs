using System;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    Idle,
    Home,
    Mosque,
    Work,
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
            return ChildManager.SelectedChild == null ? BuildingDescription + "\n\nSelect a child using the icons at the top and click on this building to send them here."
                : ChildSelectedDescription;
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
    protected virtual void OnTimeComplete(Child child) { }

    public virtual bool ConfirmEventQueued(Child selectedChild)
    {
        return true;
    }

    protected override void OnYes()
    {
        base.OnYes();

        IncomeManager.AddMoney(-CostToPerform);

        Child child = ChildManager.SelectedChild;
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

            RemoveChildIndicator(child);

            if (child.Health > 0)
            {
                // If they are still alive, we should trigger the on complete behaviour
                OnTimeComplete(child);
                GameObject.Find(EventDialogScript.EventDialogName).GetComponent<EventDialogScript>().QueueEvent(new TaskCompleteScript(GetOnCompleteDescription(child)));
            }
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