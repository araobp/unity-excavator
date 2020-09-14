using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExcavatorMonitor : MonoBehaviour
{
    public float friction = 0.6F;

    private Rigidbody rb;

    private Arrow arrow;
    private Arrow arrowGravityTangent;

    private Text textSlope;
    private Slider slider;

    private Transform slope;
    private Transform slope2;
    
    void Start()
    {
        Text textFrictionCoefficient = GameObject.Find("TextFrictionCoefficient").GetComponent<Text>();
        textFrictionCoefficient.text = $"Friction: {friction}";

        Text textMass = GameObject.Find("TextMass").GetComponent<Text>();
        textSlope = GameObject.Find("TextSlope").GetComponent<Text>();
        slider = GameObject.Find("Slider").GetComponent<Slider>();

        rb = GameObject.Find("Excavator").GetComponent<Rigidbody>();

        textMass.text = $"Mass: {Mathf.RoundToInt(rb.mass)} kg";

        slope = GameObject.Find("Joint").transform;
        textSlope.text = $"Slant: {360F - slope.rotation.eulerAngles.z} deg";

        arrow = new Arrow(Arrow.Colors.RED, 5F, 3F);
        arrowGravityTangent = new Arrow(Arrow.Colors.BLACK, 5F, 3F);

        slope2 = GameObject.Find("Joint2").transform;

        GameObject.Find("ButtonClose").GetComponent<Button>().onClick.AddListener(
            delegate
            {
                GoHome();
            }
        );
    }

    // Update is called once per frame
    void Update()
    {
        float slant = -slider.value;
        Quaternion q = Quaternion.Euler(new Vector3(0F, 0F, slant));
        slope.rotation = q;
        slope2.rotation = q;
        textSlope.text = $"Slant: {360F - slope.rotation.eulerAngles.z} deg";

        // Force to push the cubes along the slope
        Vector3 force = friction * rb.mass * Vector3.Dot(Physics.gravity, rb.transform.up) * -rb.transform.right;

        // Tangent component of Gravitaional force 
        Vector3 gravityTangent = rb.mass * Vector3.Dot(Physics.gravity, rb.transform.right) * rb.transform.right;


        // Push the cubes with the force
        if (gravityTangent.magnitude > force.magnitude)
        {
            rb.AddForce(force, ForceMode.Force);
            rb.AddForce(gravityTangent, ForceMode.Force);
        }

        // Draw vectors
        arrow.OrientVector(rb.transform, force/10000F);
        arrowGravityTangent.OrientVector(rb.transform, gravityTangent / 10000F);
    }

    void GoHome()
    {
        SceneManager.LoadScene("Scenes/Menu");
    }
}
