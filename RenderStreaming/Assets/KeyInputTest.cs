using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This script tests if the old input system works.
 * 
 * Set the folloiwng parameter to "Both" before testing this script:
 * 
 * Project Settings -> Player -> Other Settings -> Active Input Handling
 */

public class KeyInputTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Debug.Log("Alpha1");
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            Debug.Log("Joystic1Button0");
        }
    }
}
