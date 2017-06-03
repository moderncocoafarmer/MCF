using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildManagerUIScript : MonoBehaviour {

    public GameObject ChildUI;

    private List<ChildUIScript> childUIs = new List<ChildUIScript>();
    private const float Spacing = 80;

	// Use this for initialization
	void Awake ()
    {
        // Create the UIs for each child and hide them at the start
        for (int i = 0; i < ChildManager.MaxChildCount; ++i)
        {
            GameObject ui = Instantiate(ChildUI, transform, false);
            ui.transform.localPosition = new Vector3(Spacing * i, 0, 0);

            ChildUIScript uiScript = ui.GetComponentInChildren<ChildUIScript>();
            uiScript.Child = ChildManager.GetChild(i);
            uiScript.gameObject.SetActive(false);

            childUIs.Add(uiScript);
        }

        // Do this in Awake so we can hook into events immediately
        ChildManager.ChildAdded += ChildManager_ChildAdded;
        ChildManager.ChildKilled += ChildManager_ChildKilled;
        ChildManager.ChildGraduated += ChildManager_ChildGraduated;
    }

    private void ChildManager_ChildAdded(Child child)
    {
        childUIs.Find(x => x.GetComponent<ChildUIScript>().Child == child).gameObject.SetActive(true);
    }

    private void ChildManager_ChildKilled(Child child)
    {
        if (child.Health <= 0)
        {
            GameObject.Find(EventDialogScript.EventDialogName).GetComponent<EventDialogScript>().QueueEvent(new ChildDiedEventScript(child));
        }

        UpdateChildUIOnDeath(child);
    }

    private void ChildManager_ChildGraduated(Child child)
    {
        ChildUIScript childUI = childUIs.Find(x => x.GetComponent<ChildUIScript>().Child == child);
        childUI.UpdateUIForGraduatedChild();
    }

    private void UpdateChildUIOnDeath(Child child)
    {
        ChildUIScript childUI = childUIs.Find(x => x.GetComponent<ChildUIScript>().Child == child);
        childUI.UpdateUIForDeadChild();
    }

    private void OnDestroy()
    {
        ChildManager.ChildAdded -= ChildManager_ChildAdded;
        ChildManager.ChildKilled -= ChildManager_ChildKilled;
        ChildManager.ChildGraduated -= ChildManager_ChildGraduated;
    }
}
