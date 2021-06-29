using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeableObject : MonoBehaviour
{

    public bool isTransformable = false;

    List<int> objectsLayer = new List<int>();
    private const int IGNORE_RAYCAST_LAYER = 2;
    private Vector3 initialObjectRotation;
    private Vector3 initialObjectScale;
    private Vector3 initialPlayerRotation;
    public static float coef = 1;

    private void Start()
    {
        if (isTransformable)
        {
            GetComponentInChildren<cakeslice.Outline>().enabled = false;

        }
    }
    public virtual void OnGazeEnter(RaycastHit hitInfo)
    {
        //Debug.Log("Gaze entered on " + gameObject.name);
        //If this object is furniture and the player is in a transformation mode, enable outline.
        if (isTransformable && (Player.instance.activeMode == InputMode.TRANSLATE || Player.instance.activeMode == InputMode.ROTATE || Player.instance.activeMode == InputMode.SCALE) && !Input.GetMouseButton(0))
        {
            GetComponentInChildren<cakeslice.Outline>().enabled = true;

        }

    }

    public virtual void OnGaze(RaycastHit hitInfo)
    {
       //Debug.Log("Gaze hold on " + gameObject.name);
    }

    public virtual void OnGazeExit()
    {
        //Debug.Log("Gaze exited on " + gameObject.name);
        if (isTransformable)
        {
            GetComponentInChildren<cakeslice.Outline>().enabled = false;

        }


    }

    public virtual void OnPress(RaycastHit hitInfo)
    {
        Debug.Log("Button pressed");

        initialObjectRotation = transform.rotation.eulerAngles;
        initialPlayerRotation = Camera.main.transform.eulerAngles;

        initialObjectScale = transform.localScale;

        if (isTransformable)
        {
            objectsLayer.Add(gameObject.layer);

            gameObject.layer = IGNORE_RAYCAST_LAYER;
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                GameObject child = gameObject.transform.GetChild(i).gameObject;
                if (null == child)
                {
                    continue;
                }
                objectsLayer.Add(child.gameObject.layer);
                child.gameObject.layer = IGNORE_RAYCAST_LAYER;
                for (int j = 0; j < child.transform.childCount; j++)
                {
                    GameObject childchild = child.transform.GetChild(j).gameObject;
                    if (null == childchild)
                    {
                        continue;
                    }
                    objectsLayer.Add(childchild.gameObject.layer);
                    child.gameObject.layer = IGNORE_RAYCAST_LAYER;
                }
            }
        }
    }

    public virtual void OnHold(RaycastHit hitInfo)
    {
        Debug.Log("Button hold");

        if (isTransformable)
        {
            GazeTransform(hitInfo);
        }
    }

    public virtual void OnRelease(RaycastHit hitInfo)
    {
        Debug.Log("Button released");

        if (isTransformable)
        {
            gameObject.layer = objectsLayer[0];
        }

        int i = 0;
        foreach (Transform child in gameObject.transform)
        {
            if (null == child)
            {
                continue;
            }
            i += 1;
            child.gameObject.layer = objectsLayer[i];
            foreach (Transform childchild in child.transform)
            {
                if (null == childchild)
                {
                    continue;
                }
                i += 1;
                childchild.gameObject.layer = objectsLayer[i];
            }
        }
    }

    public virtual void GazeTransform(RaycastHit hitInfo)
    {

        // Call the correct transformation function

        switch (Player.instance.activeMode)
        {

            // Call different cases for active mode
            case InputMode.TRANSLATE:
                GazeTranslate(hitInfo);
                break;

            case InputMode.ROTATE:
                GazeRotate(hitInfo);
                break;

            case InputMode.SCALE:
                GazeScale(hitInfo);
                break;

        }
    }

    public virtual void GazeTranslate(RaycastHit hitInfo)
    {

        // Move the object (position)

        if (hitInfo.collider != null && (hitInfo.collider.GetComponent<Floor>() || hitInfo.collider.GetComponent<Ceiling>()))
        {

            transform.position = hitInfo.point;

        }
    }

    public virtual void GazeRotate(RaycastHit hitInfo)
    {

        // Change the object's orientation (rotation)
        float rotationSpeed = 10.0f;

        Vector3 currentPlayerRotation = Camera.main.transform.rotation.eulerAngles;

        Vector3 currentObjectRotation = transform.rotation.eulerAngles;

        Vector3 rotationDelta = currentPlayerRotation - initialPlayerRotation;

        Vector3 newRotation = new Vector3(currentObjectRotation.x, initialObjectRotation.y + (rotationDelta.y * rotationSpeed * coef), currentObjectRotation.z);

        transform.rotation = Quaternion.Euler(newRotation);


    }

    public virtual void GazeScale(RaycastHit hitInfo)
    {
        // Make the object bigger/smaller (scale)

        float scaleSpeed = 0.1f;

        float scaleFactor = 1;

        Vector3 currentPlayerRotation = Camera.main.transform.rotation.eulerAngles;
        Vector3 rotationDelta = currentPlayerRotation - initialPlayerRotation;

        // If looking up
        if (rotationDelta.x < 0 && rotationDelta.x > -180.0f || rotationDelta.x > 180.0f && rotationDelta.x < 360.0f)
        {

            // If greater than 180, map it between 0 - 180
            if (rotationDelta.x > 180.0f)
            {
                rotationDelta.x = 360.0f - rotationDelta.x;
            }

            scaleFactor = 1.0f + Mathf.Abs(rotationDelta.x) * scaleSpeed * coef;
        }
        else
        {
            if (rotationDelta.x < -180.0f)
            {
                rotationDelta.x = 360.0f + rotationDelta.x;
            }

            scaleFactor = Mathf.Max(0.1f, 1.0f - (Mathf.Abs(rotationDelta.x) * (1 / scaleSpeed)) * coef / 180.0f);
        }

        transform.localScale = scaleFactor * initialObjectScale;    
    }
}

