using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : GazeableObject
{
    private RaycastHit hit;

    void Update()
    {
        if (Player.instance.activeMode == InputMode.AUTOWALK)
        {
            Vector3 destLocation = hit.point;

            float step = Player.instance.playerSpeed * Time.deltaTime;
            Player.instance.transform.position = Vector3.MoveTowards(Player.instance.transform.position, new Vector3(destLocation.x, Player.instance.transform.position.y, destLocation.z), step);
            Player.instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

    }

    public override void OnPress(RaycastHit hitInfo)
    {
        base.OnPress(hitInfo);

        if (Player.instance.activeMode == InputMode.TELEPORT)
        {
            Vector3 destLocation = hitInfo.point;

            destLocation.y = Player.instance.transform.position.y;

            Player.instance.transform.position = destLocation;
            Player.instance.GetComponent<Rigidbody>().velocity = Vector3.zero;

        }
        else if(Player.instance.activeMode == InputMode.AUTOWALK)
        {
            hit = hitInfo;
        }
        else if (Player.instance.activeMode == InputMode.FURNITURE)
        {

            // Create the piece of furniture
            GameObject placedFurniture = GameObject.Instantiate(Player.instance.activeFurniturePrefab) as GameObject;
            if (placedFurniture.tag == "Sensor"){
                placedFurniture.transform.GetChild(0).gameObject.transform.localScale *= Player.instance.scaleSensor;
            }
                 
            // Set the position of the furniture
                placedFurniture.transform.position = hitInfo.point;



        }
    }

}