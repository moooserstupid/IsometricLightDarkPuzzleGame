using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InvisibleWall : MonoBehaviour, ITriggeredByLight{

    bool positionHasChanged = false;

	private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter");
        if (positionHasChanged)
        {
            other.gameObject.SetActive(false);
            other.gameObject.SetActive(true);
            //StartCoroutine(LightUpdateDelay(other.gameObject));
            positionHasChanged = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger exit");
        if (positionHasChanged)
        {
            other.gameObject.SetActive(false);
            other.gameObject.SetActive(true);
            //StartCoroutine(LightUpdateDelay(other.gameObject));
            positionHasChanged = false;
        }

        //other.gameObject.SetActive(true);

    }
}
