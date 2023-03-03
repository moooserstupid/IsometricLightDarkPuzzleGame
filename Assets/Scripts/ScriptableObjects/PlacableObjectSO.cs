using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Placable Object", menuName ="Placable Object")]
public class PlacableObjectSO : ScriptableObject {
	[SerializeField] public int objectID;
	[SerializeField] public Sprite objectPreview;
	[SerializeField] public GameObject objectPrefab;
}
