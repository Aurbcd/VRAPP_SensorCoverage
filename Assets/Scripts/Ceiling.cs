using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ceiling : GazeableObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void OnPress(RaycastHit hitInfo)
    {
        base.OnPress(hitInfo);

        if (Player.instance.activeMode == InputMode.FURNITURE)
        {

            // Create the piece of furniture
            GameObject placedFurniture = GameObject.Instantiate(Player.instance.activeFurniturePrefab) as GameObject;
            if (placedFurniture.tag == "Sensor")
            {
                placedFurniture.transform.GetChild(0).gameObject.transform.localScale *= Player.instance.scaleSensor;
            }


            // Set the position of the furniture
            placedFurniture.transform.position = hitInfo.point;

        }
    }
}
