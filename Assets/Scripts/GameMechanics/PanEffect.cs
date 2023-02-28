using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PanEffect : MonoBehaviour {
	Vector3 touchStart;
    [Header("Pan Parameters")]
    public Bounds panBoundingBox;
    public float panSpeed = 1;
    [Header("Zoom Parameters")]
    public float minZoomValue;
    public float maxZoomValue;
    public float zoomSpeed = 1.2f;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            Zoom(difference * 0.01f);
        } else if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            if (panBoundingBox.Contains(Camera.main.transform.position + (direction * panSpeed)))
            {
                Camera.main.transform.position += direction * panSpeed;
            }
            
        }

        
    }
    
    private void Zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp((Camera.main.orthographicSize - increment) * zoomSpeed, minZoomValue, maxZoomValue);

    }
}
