using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Player.instance.activeMode == InputMode.NONE)
        {
            GetComponent<Image>().color = new Color(1, 1, 1);
        }
    }
}
