using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

    public PlacableObjectListSO placableObjectList;
    private void Awake()
    {
        if (!instance) instance = this;
        else Destroy(instance);
    }

}
