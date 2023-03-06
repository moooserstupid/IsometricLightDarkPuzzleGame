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

    public Vector3 offset;

    public Vector3 initMousePos;
    public bool imousePosChanged = true;


    private void OnMouseDown()
    {
        initMousePos = Input.mousePosition;
        Vector3 mouseWorldPos = BuildingSystem.GetMouseWorldPosition();
        offset = transform.position - mouseWorldPos;

        //BuildingSystem.current.DrawHighlightTile(mouseWorldPos);

        placableObjectDragStartEvent?.Invoke();
        shouldRotate = true;
        
    }
    private void OnMouseDrag()
    {
        touchDelayCounter += Time.deltaTime;
        if (touchDelayCounter >= rotateTouchDelay) shouldRotate = false;

        if (Vector3.Magnitude(initMousePos - Input.mousePosition) >= 0.5f)
        {
            Vector3 mouseWorldPos = BuildingSystem.GetMouseWorldPosition();
            transform.position = mouseWorldPos;
            BuildingSystem.current.DrawHighlightTile(mouseWorldPos);
            initMousePos = Input.mousePosition;
        }
        

    }
    
    private void OnMouseUp()
    {
        if (shouldRotate)
        {
            PlaceableObject obj = GetComponent<PlaceableObject>();
            obj.Rotate();
            
        } else
        {
            transform.position = BuildingSystem.current.SnapCoordinateToGrid(BuildingSystem.GetMouseWorldPosition());
        }
        BuildingSystem.current.EraseHighlightTile();
        shouldRotate = false;
        touchDelayCounter = 0;
        placableObjectDragEndEvent?.Invoke();

    }


}
