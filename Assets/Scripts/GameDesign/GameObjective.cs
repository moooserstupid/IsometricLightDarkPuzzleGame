using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider))]
public class GameObjective : MonoBehaviour, ITriggeredByLight {

    
    public static event Action GameWon = delegate { };
	bool isInsideBeam = false;
	Collider col = null;

    [Header("Debug")]
    [SerializeField] Color originalColor;
    [SerializeField] Color activationColor;
    private void Start()
    {
        gameObject.GetComponent<Renderer>().material.color = originalColor;
        
        col = GetComponent<Collider>();
        Debug.Assert(col);
    }

    private void FixedUpdate()
    {
        isInsideBeam = false;
    }

    private void OnTriggerStay(Collider trigger)
    {
        var dynamicOcclusion = trigger.GetComponent<VLB.DynamicOcclusionRaycasting>();

        if (dynamicOcclusion)
        {
            //  GameObject is inside the beam's trigger zone
            //  Make sure its not hidden by an occluder
            isInsideBeam = !dynamicOcclusion.IsColliderHiddenByDynamicOccluder(col);
        } else
        {
            
            isInsideBeam = true;
        }
    }
    private void Update()
    {
        if (isInsideBeam)
        {
            gameObject.GetComponent<Renderer>().material.color = activationColor;
        } else
        {
            gameObject.GetComponent<Renderer>().material.color = originalColor;
        }
        Debug.Log("IsInsideBeam = " + isInsideBeam);
    }
}
