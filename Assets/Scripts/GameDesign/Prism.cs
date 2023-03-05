using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Prism : MonoBehaviour, ITriggeredByLight{
    public GameObject reflectionPrefab;
    public Transform[] reflectionTransformList;



    [Header("Debug")]
    public GameObject[] reflectionInstanceList;
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
        reflectionInstanceList = new GameObject[reflectionTransformList.Length];
        for(int i = 0; i < reflectionTransformList.Length; ++i)
        {
            reflectionInstanceList[i] = Instantiate(reflectionPrefab, reflectionTransformList[i]);
            reflectionInstanceList[i].transform.position = reflectionTransformList[i].position;
            reflectionInstanceList[i].SetActive(false);
        }
        
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
        foreach(var reflectionTransform in reflectionTransformList)
        {
            Debug.DrawRay(reflectionTransform.position, reflectionTransform.forward * 10, Color.red);

        }

        if (isInsideBeam && inputLightTransform != null)
        {
            for(int i = 0; i < reflectionInstanceList.Length; ++i)
            {
                angleBetween = (int)Mathf.Round(UtilFunctions.SignedAngleBetween(inputLightTransform.forward, reflectionTransformList[i].forward, Vector3.up));
                if (angleBetween >= 170 && angleBetween <= 180)
                {
                    reflectionInstanceList[i].SetActive(false);
                } else
                {
                    reflectionInstanceList[i].SetActive(true);
                }
            }
            //Debug.DrawRay(inputLightTransform.position, inputLightTransform.forward * 10, Color.green);
            //angleBetween = (int)Mathf.Round(UtilFunctions.SignedAngleBetween(inputLightTransform.forward, reflectionTransformList.forward, Vector3.up));
            //Debug.Log("Angle Between = " + angleBetween);
            //if (angleBetween >= 130 && angleBetween <= 150)
            //{
            //    Debug.Log("Reflecting" + angleBetween);
            //    reflectionInstanceList.SetActive(true);
            //    reflectionInstanceList.transform.position = reflectionTransformList.position;
            //    var reflectionAngle = reflectionTransformList.rotation.y - 45;
            //    reflectionInstanceList.transform.localRotation = Quaternion.Euler(reflectionTransformList.rotation.x,
            //                                                            reflectionAngle, reflectionTransformList.rotation.z);
            //    //reflectionInstance.transform.rotation = Quaternion.FromToRotation(inputLightTransform.forward, reflectionTransform.position);
            //    //reflectionInstance.transform.rotation = Quaternion.Euler(reflectionTransform.rotation.x, 
            //    //                                                        reflectionAngle, reflectionTransform.rotation.z);
            //}
            //else if (angleBetween <= -130 && angleBetween >= -150)
            //{
            //    Debug.Log("Reflecting" + angleBetween);
            //    reflectionInstanceList.SetActive(true);
            //    reflectionInstanceList.transform.position = reflectionTransformList.position;
            //    var reflectionAngle = reflectionTransformList.rotation.y + 45;
            //    reflectionInstanceList.transform.localRotation = Quaternion.Euler(reflectionTransformList.rotation.x,
            //                                                            reflectionAngle, reflectionTransformList.rotation.z);
            //}
            //else
            //{
            //    //Debug.Log("Not Reflecting" + angleBetween);
            //    reflectionInstanceList.SetActive(false);
            //}
        }
        else
        {
            foreach(var reflectionInstance in reflectionInstanceList)
            {
                reflectionInstance.SetActive(false);
            }
            
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
