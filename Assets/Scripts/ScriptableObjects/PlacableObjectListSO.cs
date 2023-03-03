using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Placable Object List", menuName = "Level Essentials")]
public class PlacableObjectListSO : ScriptableObject {
    
    [SerializeField] public PlacableObjectSO[] objectList;


}
