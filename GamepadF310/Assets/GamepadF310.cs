using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [Reference] http://www.omarvision.com/Content/documents/Joystick%20Mappings%20F310.pdf
public class GamepadF310 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0)) {
            Debug.Log("A");
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            Debug.Log("B");
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            Debug.Log("X");
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button3))
        {
            Debug.Log("Y");
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            Debug.Log("START");
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button6))
        {
            Debug.Log("BACK");
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button5))
        {
            Debug.Log("RB");
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button4))
        {
            Debug.Log("LB");
        }

        float rightAnalogX = Input.GetAxis("RightAnalogX");
        float rightAnalogY = Input.GetAxis("RightAnalogY");
        float leftAnalogX = Input.GetAxis("LeftAnalogX");
        float leftAnalogY = Input.GetAxis("LeftAnalogY");
        float dPadX = Input.GetAxis("DPadX");
        float dPadY = Input.GetAxis("DPadY");
        float trigger = Input.GetAxis("Trigger");

        if (rightAnalogX != 0)
        {
            Debug.Log($"rightAnalogX: {rightAnalogX}");
        }
        if (rightAnalogY != 0)
        {
            Debug.Log($"rightAnalogY: {rightAnalogY}");
        }
        if (leftAnalogX != 0)
        {
            Debug.Log($"leftAnalogX: {leftAnalogX}");
        }
        if (leftAnalogY != 0)
        {
            Debug.Log($"leftAnalogY: {leftAnalogY}");
        }
        if (dPadX != 0)
        {
            Debug.Log($"DPadX: {dPadX}");
        }
        if (dPadY != 0)
        {
            Debug.Log($"DPadY: {dPadY}");
        }
        if (trigger != 0)
        {
            Debug.Log($"Trigger: {trigger}");
        }
    }
}
