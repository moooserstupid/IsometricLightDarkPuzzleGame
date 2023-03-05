using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Level Metadata", menuName = "Level Essentials")]
public class LevelMetaData : ScriptableObject {

    [SerializeField] public int objectiveCount;
    [SerializeField] public PlacableObjectSO[] objectList;


}
