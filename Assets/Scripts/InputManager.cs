using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static bool PressedThisFrame { get; private set; }

	// Use this for initialization
	void Start ()
    {
        PressedThisFrame = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        PressedThisFrame = Input.GetMouseButton(0);
	}

    public static void Flush()
    {
        PressedThisFrame = false;
    }
}
