using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plus_Minus_Button : GazeableButton
{
    public bool plus_or_minus;


    public override void OnPress(RaycastHit hitInfo)
    {
        GetComponent<Image>().color = new Color(0, 1, 0);
        // Set player mode to place furniture
        if (plus_or_minus)
        {
            if (Player.instance.preciseTransf)
            {
                Player.instance.scaleSensor += 0.25f;
            }
            else
            {
                Player.instance.scaleSensor += 0.5f;

            }
        }
        else{
            if (Player.instance.preciseTransf)
            {
                Player.instance.scaleSensor -= 0.25f;
            }
            else
            {
                Player.instance.scaleSensor -= 0.5f;

            }
        }
        Invoke("SetColor", 0.1f);
    }
    private void SetColor()
    {
        GetComponent<Image>().color = new Color(1, 1, 1);
    }
}
