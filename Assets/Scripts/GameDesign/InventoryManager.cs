using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InventoryManager : MonoBehaviour {

    GridLayout gridLayout;
    public GameObject itemPreviewPrefab;
    public GameObject[] itemPreviewInstanceList;

    [Header("Debug")]
    public PlacableObjectSO[] objectList;
    private void Start()
    {
        objectList = GameManager.instance.levelData.objectList;
        itemPreviewInstanceList = new GameObject[objectList.Length];
        Debug.Log(objectList.Length);
        for(int i = 0; i < objectList.Length; ++i) {
            itemPreviewInstanceList[i] = Instantiate(itemPreviewPrefab, transform);
            itemPreviewInstanceList[i].GetComponent<ItemPreview>().SetItemPreview(objectList[i].objectID, objectList[i].objectPreview);
        }
    }
}
