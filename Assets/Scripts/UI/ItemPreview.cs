using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ItemPreview : MonoBehaviour {
	public void SetItemPreview(int id, Sprite image)
    {
        var transforms = GetComponentsInChildren<Transform>().Where(x => x != this.transform);
        transforms.ElementAt(0).gameObject.GetComponentInChildren<Image>().sprite = image;
        transforms.ElementAt(0).gameObject.GetComponentInChildren<ItemDragHandler>().itemID = id;
    }
}
