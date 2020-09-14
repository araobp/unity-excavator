using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    void AddOnClickListeners(String scene)
    {
        GameObject.Find("Button" + scene).GetComponent<Button>().onClick.AddListener(
            delegate { SceneManager.LoadScene("Scenes/" + scene); }
        );
    }

    // Start is called before the first frame update
    void Start()
    {
        AddOnClickListeners("Brake");
        AddOnClickListeners("Collision");
        AddOnClickListeners("Friction");
        AddOnClickListeners("Excavator");
        AddOnClickListeners("Slope");

        AddOnClickListeners("Freefall");
        AddOnClickListeners("Pendulum");
        AddOnClickListeners("Vector");
        AddOnClickListeners("Torque");
        AddOnClickListeners("SolarSystem");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
