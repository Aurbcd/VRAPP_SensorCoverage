using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputMode
{
    NONE,
    TELEPORT,
    WALK,
    AUTOWALK,
    FURNITURE,
    TRANSLATE,
    ROTATE,
    SCALE,
    COMPUTECOVERAGE,
    PRECISETRANSFORM,
    DRAG,
    CLEARSENSORS
}

public class Player : MonoBehaviour
{
    public static Player instance;
    public InputMode activeMode = InputMode.NONE;
    public float playerSpeed = 2.0f;
    public Object activeFurniturePrefab;
    public GameObject leftWall;
    public GameObject rightWall;
    public bool preciseTransf;
    public GameObject forwardWall;
    public GameObject backWall;
    public float scaleSensor = 1f;
    public GameObject ceiling;
    public GameObject floor;

    public GameObject[] sensors;

    public static float full_coverage = 0;

    void Awake()
    {
        if (instance != null)
        {
            GameObject.Destroy(instance.gameObject);
        }

        instance = this;
        scaleSensor = 1f;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TryWalk();
        TryComputeCoverage();
        TryPreciseTransform();
        TryClearSensors();
        TryClear();
    }

    public void TryWalk()
    {
        if (Input.GetMouseButton(0) && activeMode == InputMode.WALK)
        {
            Vector3 forward = Camera.main.transform.forward;

            Vector3 newPosition = transform.position + forward * Time.deltaTime * playerSpeed;

            if (newPosition.x < rightWall.transform.position.x && newPosition.x > leftWall.transform.position.x &&
                newPosition.y < ceiling.transform.position.y && newPosition.y > floor.transform.position.y &&
                newPosition.z > backWall.transform.position.z && newPosition.z < forwardWall.transform.position.z)
            {
                transform.position = new Vector3(newPosition.x, transform.position.y,newPosition.z);
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }
    public void TryComputeCoverage()
    {
        if (activeMode == InputMode.COMPUTECOVERAGE)
        {
            sensors = GameObject.FindGameObjectsWithTag("Sensor");
            full_coverage = 0;
            foreach (GameObject sensor in sensors)
            {
                if (sensor.GetComponentInChildren<Sensor>().enabled == false)
                {
                    sensor.GetComponentInChildren<Sensor>().enabled = true;
                }
                full_coverage += sensor.GetComponentInChildren<Sensor>().coverage;
            }
            Debug.Log("Coverage " + full_coverage);
            Invoke("SetBoolBack", 0.7f);
        }
    }



    private void SetBoolBack()
    {
        activeMode = InputMode.NONE;
    }
    public void TryPreciseTransform()
    {
        if (activeMode == InputMode.PRECISETRANSFORM && preciseTransf == false)
        {
            preciseTransf = true;
            GazeableObject.coef = 0.25f;
        }
    }

    public void TryClearSensors()
    {
        if (activeMode == InputMode.CLEARSENSORS)
        {
            if (activeMode != InputMode.COMPUTECOVERAGE)
            {
                sensors = GameObject.FindGameObjectsWithTag("Sensor");
                foreach (GameObject sensor in sensors)
                {
                    Destroy(sensor);
                }
            }
            Invoke("SetBoolBack", 0.7f);
        }
    }
    public void TryClear()
    {
        if (activeMode != InputMode.COMPUTECOVERAGE)
        {
            sensors = GameObject.FindGameObjectsWithTag("Sensor");
            foreach (GameObject sensor in sensors)
            {
                if (sensor.GetComponentInChildren<Sensor>().enabled == true)
                {
                    sensor.GetComponentInChildren<Sensor>().enabled = false;
                }
            }
        }
        if (activeMode != InputMode.PRECISETRANSFORM && activeMode != InputMode.TRANSLATE && activeMode != InputMode.ROTATE && activeMode != InputMode.SCALE)
        {
            GazeableObject.coef = 1;
            preciseTransf = false;
        }
    }
}