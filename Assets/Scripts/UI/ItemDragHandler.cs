using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DanielLochner.Assets.SimpleSideMenu;
using Random = UnityEngine.Random;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public int itemID;
    
    public static event Action<int> objectDragEvent = delegate { };
    public static event Action<int> objectEndDragEvent = delegate { };

    private bool dragEventFired = false;
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        //FindAnyObjectByType<SimpleSideMenu>()?.ToggleState();
        if (!dragEventFired)
        {
            objectDragEvent?.Invoke(itemID);
            dragEventFired = true;
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
        dragEventFired = false;
    }
}
