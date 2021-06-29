using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GazeableButton : GazeableObject
{

    protected VRCanvas parentPanel;

    // Use this for initialization
    void Start()
    {

        parentPanel = GetComponentInParent<VRCanvas>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetButtonColor(Color buttonColor)
    {
        GetComponent<Image>().color = buttonColor;
    }

    public override void OnPress(RaycastHit hitInfo)
    {
        base.OnPress(hitInfo);

        if (parentPanel != null)
        {
            parentPanel.SetActiveButton(this);
        }
        else
        {
            Debug.LogError("Button not a child of object with VRPanel component.", this);
        }
    }
}