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
}
