using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildIndicatorUIScript : MonoBehaviour
{
    public BarScript Progress
    {
        get;
        private set;
    }

    private Child child;
    public Child Child
    {
        get { return child; }
        set
        {
            child = value;
            childNameText = GetComponentInChildren<TextMesh>();
            childNameText.text = child.Name;
        }
    }

    private TextMesh childNameText;
    private DataDialogScript dataDialog;

    private void Start()
    {
        dataDialog = GameObject.Find(DataDialogScript.DataDialogName).GetComponent<DataDialogScript>();
        //Progress = 
    }

    private void OnMouseDown()
    {
        dataDialog.Toggle(Child);
    }
}
