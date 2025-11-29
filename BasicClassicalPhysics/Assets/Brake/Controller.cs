using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    GameObject vehicle10;
    GameObject vehicle20;
    GameObject vehicle30;
    GameObject vehicle40;
    GameObject vehicle50;
    GameObject vehicle60;
    GameObject vehicle70;
    GameObject vehicle80;
    GameObject vehicle90;
    GameObject vehicle100;

    List<GameObject> vehicles = new List<GameObject>();
    bool started = false;

    // Start is called before the first frame update
    void Start()
    {
        vehicle10 = GameObject.Find("Vehicle10");
        vehicle20 = GameObject.Find("Vehicle20");
        vehicle30 = GameObject.Find("Vehicle30");
        vehicle40 = GameObject.Find("Vehicle40");
        vehicle50 = GameObject.Find("Vehicle50");
        vehicle60 = GameObject.Find("Vehicle60");
        vehicle70 = GameObject.Find("Vehicle70");
        vehicle80 = GameObject.Find("Vehicle80");
        vehicle90 = GameObject.Find("Vehicle90");
        vehicle100 = GameObject.Find("Vehicle100");

        vehicles.Add(vehicle10);
        vehicles.Add(vehicle20);
        vehicles.Add(vehicle30);
        vehicles.Add(vehicle40);
        vehicles.Add(vehicle50);
        vehicles.Add(vehicle60);
        vehicles.Add(vehicle70);
        vehicles.Add(vehicle80);
        vehicles.Add(vehicle90);
        vehicles.Add(vehicle100);

        GameObject.Find("ButtonStart").GetComponent<Button>().onClick.AddListener(
            delegate
            {
                StartSimulation();
            });

        GameObject.Find("ButtonClose").GetComponent<Button>().onClick.AddListener(
            delegate
            {
                GoHome();
            });

    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            GameObject toBeRemoved = null;
            foreach (GameObject vehicle in vehicles)
            {
                float currentSpeed = vehicle.GetComponent<Rigidbody>().linearVelocity.magnitude;
                if (currentSpeed == 0F)
                {
                    toBeRemoved = vehicle;
                }
            }
            if (toBeRemoved != null)
            {
                float distance = toBeRemoved.transform.position.z;
                distance = Mathf.RoundToInt(distance);
                TextMesh textMesh = toBeRemoved.transform.GetChild(0).GetComponent<TextMesh>();
                textMesh.text += $"\n{distance}m";
                vehicles.Remove(toBeRemoved);
            }
        }
    }

    void StartSimulation()
    {
        float speed = 10F;

        foreach (GameObject vehicle in vehicles)
        {
            vehicle.GetComponent<Rigidbody>().linearVelocity = new Vector3(0, 0, speed * 1000F / 3600F);
            speed += 10F;
        }

        started = true;
    }

    void GoHome()
    {
        SceneManager.LoadScene("Scenes/Menu");
    }
}
