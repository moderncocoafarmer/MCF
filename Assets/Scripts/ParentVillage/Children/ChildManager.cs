using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.SceneManagement;

public class ChildManager
{
    public delegate void ChildEventHandler(Child child);

    private static ChildManager instance = new ChildManager();
    public static ChildManager Instance { get { return instance; } }

    public event ChildEventHandler ChildAdded;
    public event ChildEventHandler ChildRemoved;
    public event ChildEventHandler ChildSelected;
    public event ChildEventHandler ChildDeselected;
    public event ChildEventHandler ChildGraduated;

    private int currentChildIndex = 0;
    public const int MaxChildCount = 7;
    public float ChildDegredation = 10;

    public int ChildCount { get { return Children.Count(x => x.State == Child.ChildState.kAlive); } }
    public int ChildrenGraduated { get { return Children.Count(x => x.State == Child.ChildState.kGraduated); } }
    public Child SelectedChild { get { return Children.Find(x => x.IsSelected); } }

    private List<Child> Children = new List<Child>()
    {
        new Child("Adama"),
        new Child("Oumar"),
        new Child("Salif"),
        new Child("Maria"),
        new Child("Kilia"),
        new Child("Sekou"),
        new Child("Siaka"),
    };

    private ChildManager() { }
    
    public void AddChild()
    {
        Child child = Children[currentChildIndex];
        child.State = Child.ChildState.kAlive;

        if (ChildAdded != null)
        {
            ChildAdded.Invoke(child);
        }

        currentChildIndex++;
    }
    
    public void RemoveChild(int index)
    {
        RemoveChild(Children[index]);
    }

    public void RemoveChild(Child child)
    {
        child.State = Child.ChildState.kDead;
        DeselectChild(child);
        
        if (ChildRemoved != null)
        {
            ChildRemoved.Invoke(child);
        }

        if (ChildCount == 0)
        {
            SceneManager.LoadScene("LoseMenu");
        }
    }
    
    public Child GetChild(int index)
    {
        return Children[index];
    }

    public Child FindChild(Predicate<Child> predicate)
    {
        return Children.Find(predicate);
    }

    public void SelectChild(Child child)
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

    public void DeselectChild(Child child)
    {
        child.IsSelected = false;

        if (ChildDeselected != null)
        {
            ChildDeselected.Invoke(child);
        }
    }

    public void GraduateChild(Child child)
    {
        child.State = Child.ChildState.kGraduated;
        DeselectChild(child);

        if (ChildGraduated != null)
        {
            ChildGraduated.Invoke(child);
        }
    }

    public void ApplyEventToAllChildren(DataPacket data)
    {
        foreach (Child child in Children.FindAll(x => x.State == Child.ChildState.kAlive))
        {
            child.Apply(data);
        }
    }
}