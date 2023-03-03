using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectDrag : MonoBehaviour {

	

    public static event Action placableObjectDragStartEvent = delegate { };
    public static event Action placableObjectDragEndEvent = delegate { };
    [Header("Debug")]
    public float rotateTouchDelay = 0.1f;
    public float touchDelayCounter = 0f;
    public bool shouldRotate = false;

    private Vector3 offset;
    


    private void OnMouseDown()
    {
        offset = transform.position - BuildingSystem.GetMouseWorldPosition();
        placableObjectDragStartEvent?.Invoke();
        shouldRotate = true;
    }
    private void OnMouseDrag()
    {
        touchDelayCounter += Time.deltaTime;
        if (touchDelayCounter >= rotateTouchDelay) shouldRotate = false;
        Vector3 pos = BuildingSystem.GetMouseWorldPosition() + offset;
        transform.position = BuildingSystem.current.SnapCoordinateToGrid(pos);

    }
    private void OnMouseUp()
    {
        if (shouldRotate)
        {
            PlaceableObject obj = GetComponent<PlaceableObject>();
            obj.Rotate();
            
        }
        shouldRotate = false;
        touchDelayCounter = 0;
        placableObjectDragEndEvent?.Invoke();

    }


}
