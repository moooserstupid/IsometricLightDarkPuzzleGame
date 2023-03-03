using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DanielLochner.Assets.SimpleSideMenu;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    public static event Action<int> objectDropEvent = delegate { };

    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }
    public void OnDrop(PointerEventData eventData)
    {
        RectTransform invPanel = transform as RectTransform;
        if (!RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
        {
            //Debug.Log("Drop Item");
            FindAnyObjectByType<SimpleSideMenu>()?.ToggleState();
            //Debug.Log(eventData.pointerDrag.gameObject.GetComponent<ItemDragHandler>());
            int objectID = eventData.pointerDrag.gameObject.GetComponent<ItemDragHandler>().itemID;
            objectDropEvent?.Invoke(objectID);
            eventData.pointerDrag.gameObject.SetActive(false);
        }
    }
}
