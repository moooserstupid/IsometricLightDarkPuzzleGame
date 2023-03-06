using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider))]
public class Mirror : MonoBehaviour, ITriggeredByLight {

    public GameObject reflectionPrefab;
    public Transform reflectionTransform;
    
    

    [Header("Debug")]
    public GameObject reflectionInstance;
    public Transform inputLightTransform;
    public int angleBetween;

    private Vector3 previousPosition;
    private bool positionHasChanged = false;
    public static event Action updateLight = delegate { };
    private bool lightStatusChanged = false;
    private bool isInsideBeam = false;
    private BoxCollider col;

    private void Start()
    {
        
        reflectionInstance = Instantiate(reflectionPrefab, reflectionTransform);
        reflectionInstance.transform.position = reflectionTransform.position;
        reflectionInstance.SetActive(false);
        col = GetComponent<BoxCollider>();

        //reflectionTransform = transform;
        //reflectionTransform.transform.position = col.center;
        previousPosition = transform.position;
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
        }
        else
        {
            isInsideBeam = true;
            reflectionTransform.position = trigger.ClosestPoint(reflectionTransform.position);
            
        }

        if (isInsideBeam) inputLightTransform = trigger.transform;
        else inputLightTransform = null;


        Debug.Log("Trigger stay");
    }
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

    private void Update()
    {
        Debug.DrawRay(reflectionTransform.position, reflectionTransform.forward * 10, Color.red);
        
        if (isInsideBeam && inputLightTransform != null)
        {
            Debug.DrawRay(inputLightTransform.position, inputLightTransform.forward * 10, Color.green);
            angleBetween = (int)Mathf.Round(UtilFunctions.SignedAngleBetween(inputLightTransform.forward, reflectionTransform.forward, Vector3.up));
            Debug.Log("Angle Between = " + angleBetween);
            if (angleBetween >= 90 && angleBetween <= 180)
            {
                Debug.Log("Reflecting" + angleBetween);
                reflectionInstance.SetActive(true);
                reflectionInstance.transform.position = reflectionTransform.position;
                var reflectionAngle = reflectionTransform.rotation.y - 45;
                reflectionInstance.transform.localRotation = Quaternion.Euler(reflectionTransform.rotation.x,
                                                                        reflectionAngle, reflectionTransform.rotation.z);
                //reflectionInstance.transform.rotation = Quaternion.FromToRotation(inputLightTransform.forward, reflectionTransform.position);
                //reflectionInstance.transform.rotation = Quaternion.Euler(reflectionTransform.rotation.x, 
                //                                                        reflectionAngle, reflectionTransform.rotation.z);
            }
            else if (angleBetween <= -90 && angleBetween >= -180)
            {
                Debug.Log("Reflecting" + angleBetween);
                reflectionInstance.SetActive(true);
                reflectionInstance.transform.position = reflectionTransform.position;
                var reflectionAngle = reflectionTransform.rotation.y + 45;
                reflectionInstance.transform.localRotation = Quaternion.Euler(reflectionTransform.rotation.x,
                                                                        reflectionAngle, reflectionTransform.rotation.z);
            } else
            {
                //Debug.Log("Not Reflecting" + angleBetween);
                reflectionInstance.SetActive(false);
            }
        } else
        {
            
            reflectionInstance.SetActive(false);
        }
        //Debug.Log("mirror is inside beam = " + isInsideBeam);

        if ((previousPosition - transform.position).magnitude > 0)
        {
            positionHasChanged = true;
            previousPosition = transform.position;
        }
    }

    void LightUpdate()
    {
        updateLight?.Invoke();
    }
    
    IEnumerator LightUpdateDelay(GameObject light)
    {
        yield return new WaitForSeconds(0.1f);
        light.SetActive(true);
    }
    
    
}
