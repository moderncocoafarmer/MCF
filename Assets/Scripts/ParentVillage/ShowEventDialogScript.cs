using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
public class ShowEventDialogScript : MonoBehaviour, IPointerClickHandler
{
    public string EventName;
    public GameObject ChildIndicatorUI;
    public Transform FirstChildPosition;
    public Transform EvenChildPositions;
    public Transform OddChildPositions;

    private InteractableBuildingEventScript eventScript;
    public InteractableBuildingEventScript EventScript { get { return eventScript; } }
    private GameObject dialog;
    private AudioSource click;

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
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
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
