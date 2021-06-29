using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PTButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GazeableObject.coef == 0.25f)
        {
            GetComponent<Image>().color = new Color(0.9811321f, 0.5071217f, 0);
        }
        else if (GazeableObject.coef == 1)
        {
            GetComponent<Image>().color = new Color(1, 1, 1);
        }
    }
}
