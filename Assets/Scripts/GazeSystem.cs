using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeSystem : MonoBehaviour
{
    public GameObject reticle;

    public Color inactiveReticleColor = Color.gray;
    public Color activeReticleColor = Color.green;

    private GazeableObject currentGazeObject;
    private GazeableObject currentSelectedObject;

    private RaycastHit lastHit;
    // Use this for initialization
    void Start()
    {

        SetReticleColor(inactiveReticleColor);

    }

    // Update is called once per frame
    void Update()
    {

        ProcessGaze();
        CheckForInput(lastHit);
    }

    public void ProcessGaze()
    {

        Ray raycastRay = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        Debug.DrawRay(raycastRay.origin, raycastRay.direction * 100);


        if (Physics.Raycast(raycastRay, out hitInfo))
        {
            // Do something to the object

            // Check if the object is interactable

            // Get the gameobject from the hitInfo

            GameObject hitObj = hitInfo.collider.gameObject;
            // Check if the object is a new object (first time looking)
            GazeableObject gazeObj = hitObj.GetComponentInParent<GazeableObject>();
            // Set the reticle color

            if (gazeObj != null)
            {
                if (gazeObj != currentGazeObject)
                {
                    ClearCurrentObject();
                    currentGazeObject = gazeObj;
                    currentGazeObject.OnGazeEnter(hitInfo);
                    SetReticleColor(activeReticleColor);
                }
                else
                {
                    currentGazeObject.OnGaze(hitInfo);
                }
            }
            else
            {
                ClearCurrentObject();
            }


            lastHit = hitInfo;
        }
        else
        {
            ClearCurrentObject();
        }
    }

    private void SetReticleColor(Color reticleColor)
    {
        // Set the color of the reticle
        reticle.GetComponent<Renderer>().material.SetColor("_Color", reticleColor);
    }
    private void CheckForInput(RaycastHit hitInfo)
    {
        if (Input.GetMouseButtonDown(0) && currentGazeObject != null)
        {
            currentSelectedObject = currentGazeObject;
            currentSelectedObject.OnPress(hitInfo);

        }

        else if (Input.GetMouseButton(0) && currentSelectedObject != null)
        {
            currentSelectedObject.OnHold(hitInfo);
        }

        else if (Input.GetMouseButtonUp(0) && currentSelectedObject != null)
        {
            currentSelectedObject.OnRelease(hitInfo);
            currentSelectedObject = null;
        }
    }
    private void ClearCurrentObject()
    {
        if (currentGazeObject != null)
        {
            currentGazeObject.OnGazeExit();
            SetReticleColor(inactiveReticleColor);
            currentGazeObject = null;
        }
    }
}