using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class EventDialogScript : MonoBehaviour
{
    public const string EventDialogName = "EventDialog";

    public bool DialogOpen { get { return eventDialogUI.activeSelf; } }
    private EventScript CurrentEvent { get; set; }

    private bool timePausedOnEventShow;
    private AudioSource audioSource;
    private GameObject eventDialogUI;
    private Text nameUI;
    private Text descriptionUI;

    private GameObject yesButtonPanel;
    private GameObject yesButton;
    private GameObject childBusyText;

    #region Data UI

    private GameObject buttonEffects;
    private Text healthDeltaText;
    private Text safetyDeltaText;
    private Text educationDeltaText;
    private Text happinessDeltaText;
    
    #endregion

    private Queue<EventScript> events = new Queue<EventScript>();
    
    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        eventDialogUI = transform.Find("EventDialogUI").gameObject;
        nameUI = eventDialogUI.transform.Find("EventName").GetComponent<Text>();
        descriptionUI = eventDialogUI.transform.Find("EventDescription").GetComponent<Text>();

        buttonEffects = eventDialogUI.transform.Find("ButtonEffects").gameObject;
        healthDeltaText = buttonEffects.transform.FindChild("Health").GetComponentInChildren<Text>();
        safetyDeltaText = buttonEffects.transform.FindChild("Safety").GetComponentInChildren<Text>();
        educationDeltaText = buttonEffects.transform.FindChild("Education").GetComponentInChildren<Text>();
        happinessDeltaText = buttonEffects.transform.FindChild("Happiness").GetComponentInChildren<Text>();

        yesButtonPanel = eventDialogUI.transform.Find("YesButtonPanel").gameObject;
        yesButton = yesButtonPanel.transform.FindChild("YesButton").gameObject;
        childBusyText = yesButtonPanel.transform.FindChild("ChildBusyText").gameObject;
    }

    public void Start()
    {
        eventDialogUI.SetActive(false);
        Hide();
    }
    
    public void Update()
    {
        if (CurrentEvent == null)
        {
            ShowEvent();
        }
    }

    public void QueueEvent(EventScript eventScript)
    {
        events.Enqueue(eventScript);
    }

    private void ShowEvent()
    {
        if (events.Count > 0)
        {
            timePausedOnEventShow = TimeManager.Paused;
            TimeManager.Paused = true;
            CurrentEvent = events.Dequeue();
            Child selectedChild = ChildManager.SelectedChild;

            if (CurrentEvent.OnShowAudioClip != null)
            {
                audioSource.clip = CurrentEvent.OnShowAudioClip;
                audioSource.Play();
            }

            nameUI.text = CurrentEvent.Name;
            descriptionUI.text = CurrentEvent.Description;
            buttonEffects.SetActive(CurrentEvent.DataImplemented);

            if (CurrentEvent.DataImplemented)
            {
                healthDeltaText.text = CurrentEvent.HealthDeltaText;
                safetyDeltaText.text = CurrentEvent.SafetyDeltaText;
                educationDeltaText.text = CurrentEvent.EducationDeltaText;
                happinessDeltaText.text = CurrentEvent.HappinessDeltaText;
            }

            bool choicesEnabled = CurrentEvent.ChoicesEnabled;
            bool childLockedIn = (selectedChild != null) && (selectedChild.BuildingType != BuildingType.Idle);
            yesButtonPanel.SetActive(choicesEnabled);

            yesButton.SetActive(!childLockedIn);
            yesButton.GetComponentInChildren<Text>().text = choicesEnabled ? CurrentEvent.YesButtonText : "";

            childBusyText.SetActive(childLockedIn);
            childBusyText.GetComponent<Text>().text = childLockedIn ? selectedChild.Name + " is busy at the " + selectedChild.BuildingType.ToString() : "";

            eventDialogUI.SetActive(true);
        }
    }

    private void Hide()
    {
        CurrentEvent = null;
        eventDialogUI.SetActive(false);
        TimeManager.Paused = timePausedOnEventShow;
    }

    public void Yes()
    {
        CurrentEvent.Yes();

        if (CurrentEvent.OnYesAudioClip != null)
        {
            audioSource.clip = CurrentEvent.OnYesAudioClip;
            audioSource.Play();
        }

        Hide();
    }

    public void No()
    {
        CurrentEvent.No();

        if (CurrentEvent.OnNoAudioClip != null)
        {
            audioSource.clip = CurrentEvent.OnNoAudioClip;
            audioSource.Play();
        }

        Hide();
    }
}
