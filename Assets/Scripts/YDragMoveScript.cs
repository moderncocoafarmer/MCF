using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YDragMoveScript : MonoBehaviour
{
    public float YSpeed = 1f;

    bool mouseDownLastFrame = false;
    private Vector3 lastPosition;
    private float minYPosition;
    private float maxYPosition;
 
    // Use this for initialization
    void Start()
    {
        minYPosition = transform.localPosition.y;
        maxYPosition = GetComponent<RectTransform>().sizeDelta.y * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.touchCount < 2) && Input.GetMouseButton(0))
        {
            if (!mouseDownLastFrame)
            {
                mouseDownLastFrame = true;
            }
            else
            {

                transform.localPosition += new Vector3(0, (lastPosition - Input.mousePosition).y * YSpeed, 0);
            }

            lastPosition = Input.mousePosition;
        }
        else
        {
            mouseDownLastFrame = false;
        }

        Vector3 position = transform.localPosition;
        position.y = Mathf.Clamp(position.y, minYPosition, maxYPosition);
        transform.localPosition = position;
    }
}
