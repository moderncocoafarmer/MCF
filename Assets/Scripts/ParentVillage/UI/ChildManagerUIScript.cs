using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildManagerUIScript : MonoBehaviour {

    public GameObject ChildUI;

    private List<ChildUIScript> childUIs = new List<ChildUIScript>();
    private const float Spacing = 80;

	// Use this for initialization
	void Start ()
    {
        // Create the UIs for each child and hide them at the start
        for (int i = 0; i < ChildManager.MaxChildCount; ++i)
        {
            GameObject ui = Instantiate(ChildUI, transform, false);
            ui.transform.localPosition = new Vector3(Spacing * i, 0, 0);

            ChildUIScript uiScript = ui.GetComponent<ChildUIScript>();
            uiScript.Child = ChildManager.Instance.GetChild(i);
            childUIs.Add(uiScript);

            ui.SetActive(false);
        }

        ChildManager.Instance.ChildKilled += ChildManager_ChildKilled;
        ChildManager.Instance.ChildGraduated += ChildManager_ChildGraduated;
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
        ChildManager.Instance.ChildKilled -= ChildManager_ChildKilled;
        ChildManager.Instance.ChildGraduated -= ChildManager_ChildGraduated;
    }
}
