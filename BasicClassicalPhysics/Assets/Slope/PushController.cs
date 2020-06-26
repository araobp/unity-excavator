using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushController : MonoBehaviour
{
    public float acceleration = 4F;
    private Rigidbody rb;
    private Rigidbody rb2;

    // Start is called before the first frame update
    void Start()
    {
        Text textAccel = GameObject.Find("TextAcceleration").GetComponent<Text>();
        textAccel.text = $"Acceleration: {acceleration} m/s^2";

        Text textMass = GameObject.Find("TextMass").GetComponent<Text>();
        Text textSlope = GameObject.Find("TextSlope").GetComponent<Text>();
        Text textMass2 = GameObject.Find("TextMass2").GetComponent<Text>();
        Text textSlope2 = GameObject.Find("TextSlope2").GetComponent<Text>();

        rb = GameObject.Find("Cube").GetComponent<Rigidbody>();
        rb2 = GameObject.Find("Cube2").GetComponent<Rigidbody>();

        textMass.text = $"Mass: {Mathf.RoundToInt(rb.mass)} kg";
        textMass2.text = $"Mass: {Mathf.RoundToInt(rb2.mass)} kg";

        Transform slope = GameObject.Find("Slope").transform;
        Transform slope2 = GameObject.Find("Slope2").transform;
        textSlope.text = $"Inclination: {360F - slope.rotation.eulerAngles.z} deg";
        textSlope2.text = $"Inclination: {360F - slope2.rotation.eulerAngles.z} deg";

    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(-rb.transform.right * rb.mass * acceleration, ForceMode.Force);
        rb2.AddForce(-rb2.transform.right * rb2.mass * acceleration, ForceMode.Force);
    }
}
