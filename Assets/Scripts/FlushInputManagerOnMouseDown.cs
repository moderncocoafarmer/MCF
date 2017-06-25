using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlushInputManagerOnMouseDown : MonoBehaviour
{
    private void OnMouseDown()
    {
        InputManager.Flush();
    }
}
