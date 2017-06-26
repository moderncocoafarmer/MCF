using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildIndicatorUIScript : MonoBehaviour
{
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
    private BarScript progressBar;

    private void Awake()
    {
        dataDialog = GameObject.Find(DataDialogScript.DataDialogName).GetComponent<DataDialogScript>();
        progressBar = transform.Find("ChildIndicatorPanel").GetComponentInChildren<BarScript>();
        progressBar.transform.localPosition -= new Vector3(progressBar.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.extents.x, 0, 0);
    }

    private void OnMouseDown()
    {
        dataDialog.Toggle(Child);
    }

    public void IncrementBar(float increment)
    {
        progressBar.Value += increment;
        progressBar.transform.localPosition += new Vector3(progressBar.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.extents.x * increment / progressBar.Max, 0, 0);
    }
}
