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

    public const int MaxChildCount = 7;
    public float ChildDegredation = 10;
    public int ChildCount { get; private set; }
    public int ChildrenGraduated { get; private set; }
    public Child SelectedChild { get { return Children.Find(x => x.IsSelected); } }

    private List<Child> ChildrenToAdd = new List<Child>();
    private List<Child> ChildrenToRemove = new List<Child>();
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

    public void Update()
    {
        foreach (Child child in ChildrenToAdd)
        {
            AddChildImpl(child);
        }

        ChildrenToAdd.Clear();

        foreach (Child child in ChildrenToRemove)
        {
            RemoveChildImpl(child);
        }

        ChildrenToRemove.Clear();
    }

    public void AddChild()
    {
        ChildCount++;
    }

    private void AddChildImpl(Child child)
    {
        Children.Add(child);

        if (ChildAdded != null)
        {
            ChildAdded.Invoke(child);
        }
    }

    public void RemoveChild(int index)
    {
        RemoveChild(Children[index]);
    }

    public void RemoveChild(Child child)
    {
        ChildCount--;
        ChildrenToRemove.Add(child);
    }

    private void RemoveChildImpl(Child child)
    {
        DeselectChild(child);
        Children.Remove(child);

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
        foreach (Child c in Children)
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
        ChildrenGraduated++;

        DeselectChild(child);
        Children.Remove(child);

        if (ChildGraduated != null)
        {
            ChildGraduated.Invoke(child);
        }
    }

    public void ApplyEventToAllChildren(DataPacket data)
    {
        foreach (Child child in Children)
        {
            child.Apply(data);
        }
    }
}