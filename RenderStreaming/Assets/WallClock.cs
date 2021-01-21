using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallClock : MonoBehaviour
{
    Text clock;

    // Start is called before the first frame update
    void Start()
    {
        clock = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        var timeString = DateTime.Now.ToString("HH:mm:ss.fff");
        clock.text = $"{timeString}";
    }
}