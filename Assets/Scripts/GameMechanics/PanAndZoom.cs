using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DanielLochner.Assets.SimpleSideMenu;
using Random = UnityEngine.Random;

public class PanAndZoom : MonoBehaviour {
	Vector3 touchStart;
    [Header("Pan Parameters")]
    public Bounds panBoundingBox;
    public float panSpeed = 1;
    [Header("Zoom Parameters")]
    public float minZoomValue;
    public float maxZoomValue;
    public float zoomSpeed = 1.2f;

    private bool isPlacableObjectBeingDragged = false;
    private void OnEnable()
    {
        ObjectDrag.placableObjectDragStartEvent += PlacableObjectDragStart;
        ObjectDrag.placableObjectDragEndEvent += PlacableObjectDragEnd;
    }
    private void OnDisable()
    {
        ObjectDrag.placableObjectDragStartEvent -= PlacableObjectDragStart;
        ObjectDrag.placableObjectDragEndEvent -= PlacableObjectDragEnd;
    }
    private void PlacableObjectDragStart()
    {
        isPlacableObjectBeingDragged = true;
    }
    private void PlacableObjectDragEnd()
    {
        isPlacableObjectBeingDragged = false;

    }
    private SimpleSideMenu sideMenu;
    private void Start()
    {
        sideMenu = FindAnyObjectByType<SimpleSideMenu>();
    }
    private void Update()
    {
        //if (Input.touchCount > 0)
        //{
        //    foreach (Touch touch in Input.touches)
        //    {
        //        int id = touch.fingerId;
        //        if (EventSystem.current.IsPointerOverGameObject(id))
        //        {
        //            Debug.Log("Touching UI");
        //            return;
        //        }
        //    }
        //}
        Debug.Log("IsPLacableObjectBeingDragged" + isPlacableObjectBeingDragged);
        if (isPlacableObjectBeingDragged || (sideMenu && sideMenu.CurrentState == State.Open)) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("mouseDown");
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
