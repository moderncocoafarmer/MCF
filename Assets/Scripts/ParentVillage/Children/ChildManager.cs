using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChildManager : MonoBehaviour
{
    public delegate void ChildEventHandler(Child child);

    public static event ChildEventHandler ChildAdded;
    public static event ChildEventHandler ChildKilled;
    public static event ChildEventHandler ChildSelected;
    public static event ChildEventHandler ChildDeselected;
    public static event ChildEventHandler ChildGraduated;

    private static int currentChildIndex = 0;
    public static float ChildDegredation = 10;

    public static bool CanHaveChild { get { return currentChildIndex < MaxChildCount; } }
    public static int MaxChildCount { get { return Children.Count; } }
    public static int ChildCount { get { return Children.Count(x => x.State == Child.ChildState.kAlive); } }
    public static int ChildrenGraduated { get { return Children.Count(x => x.State == Child.ChildState.kGraduated); } }
    public static Child SelectedChild { get { return Children.Find(x => x.IsSelected); } }

    private static List<Child> Children;

    public ChildManager()
    {
        currentChildIndex = 0;
        ChildDegredation = 10;

        Children = new List<Child>()
        {
            new Child("Adama"),
            new Child("Oumar"),
            new Child("Salif"),
            new Child("Maria"),
            new Child("Kilia"),
            new Child("Sekou"),
            new Child("Siaka"),
        };
    }

    public void Start()
    {
        // Add two children to begin with
        GiveBirthToChild();
        GiveBirthToChild();
        GiveBirthToChild();
        GiveBirthToChild();
        GiveBirthToChild();
        GiveBirthToChild();
        GiveBirthToChild();
    }

    public static void GiveBirthToChild()
    {
        Child child = Children[currentChildIndex];
        child.State = Child.ChildState.kAlive;

        if (ChildAdded != null)
        {
            ChildAdded.Invoke(child);
        }

        currentChildIndex++;
    }
    
    public static void KillChild(int index)
    {
        KillChild(Children[index]);
    }

    public static void KillChild(Child child, Child.ChildState newState = Child.ChildState.kDead)
    {
        child.State = newState;
        child.LockIn(BuildingType.Idle);

        DeselectChild(child);
        
        if (ChildKilled != null)
        {
            ChildKilled.Invoke(child);
        }

        if (ChildCount == 0)
        {
            SceneManager.LoadScene("LoseMenu");
        }
    }
    
    public static Child GetChild(int index)
    {
        return Children[index];
    }

    public static Child FindChild(Predicate<Child> predicate)
    {
        return Children.Find(predicate);
    }

    public static void SelectChild(Child child)
    {
        foreach (Child c in Children.FindAll(x => x.State == Child.ChildState.kAlive))
        {
            c.IsSelected = false;
        }

        child.IsSelected = true;

        if (ChildSelected != null)
        {
            ChildSelected.Invoke(child);
        }
    }

    public static void DeselectChild(Child child)
    {
        child.IsSelected = false;

        if (ChildDeselected != null)
        {
            ChildDeselected.Invoke(child);
        }
    }

    public static void GraduateChild(Child child)
    {
        child.State = Child.ChildState.kGraduated;
        child.LockIn(BuildingType.Idle);

        DeselectChild(child);

        if (ChildGraduated != null)
        {
            ChildGraduated.Invoke(child);
        }
    }

    public static void ApplyEventToAllChildren(DataPacket data)
    {
        foreach (Child child in Children.FindAll(x => x.State == Child.ChildState.kAlive))
        {
            child.Apply(data);
        }
    }
}