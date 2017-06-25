using System;
using System.Reflection;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
public class ShowEventDialogScript : MonoBehaviour {

    public string EventName;
    public GameObject ChildIndicatorUI;
    public Transform FirstChildPosition;
    public Transform EvenChildPositions;
    public Transform OddChildPositions;

    private InteractableBuildingEventScript eventScript;
    public InteractableBuildingEventScript EventScript { get { return eventScript; } }
    private GameObject dialog;
    private AudioSource click;

    private bool clicked = false;

    void Awake()
    {
        dialog = GameObject.Find(EventDialogScript.EventDialogName);        
    }

    // Use this for initialization
    void Start ()
    {
        click = GetComponent<AudioSource>();

        Type type = Assembly.GetExecutingAssembly().GetType(EventName);
        eventScript = Activator.CreateInstance(type) as InteractableBuildingEventScript;
        eventScript.SetUpIndicatorUI(ChildIndicatorUI, FirstChildPosition, EvenChildPositions, OddChildPositions);
	}
	
	// Update is called once per frame
	void Update ()
    {
        eventScript.Update();

        if (clicked)
        {
            HandleClick();
            clicked = false;
        }
	}
    
    private void OnMouseDown()
    {
        // On mouse down happens before update, so we need to register the click here, but handle it in update after
        // other elements have had a chance to cancel the input through InputManager.Flush();
        clicked = true;
    }

    private void HandleClick()
    {
        if (!InputManager.PressedThisFrame)
        {
            // The mouse is down but we have clicked on an element in front of this object
            return;
        }

        // Only do this if there isn't a dialog open already
        if (!dialog.GetComponent<EventDialogScript>().DialogOpen)
        {
            click.Play();
            dialog.GetComponent<EventDialogScript>().QueueEvent(eventScript);
        }
    }
}
