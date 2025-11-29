using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PushController : MonoBehaviour
{
    public float acceleration = 4F;

    private Rigidbody rb;
    private Rigidbody rb2;

    private Arrow arrow;
    private Arrow arrow2;

    private Arrow arrowGravityTangent;
    private Arrow arrowGravityTangent2;

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

        arrow = new Arrow(Arrow.Colors.BLUE, 3F, 2F);
        arrow2 = new Arrow(Arrow.Colors.GREEN ,3F, 2F);
        arrowGravityTangent = new Arrow(Arrow.Colors.BLACK);
        arrowGravityTangent2 = new Arrow(Arrow.Colors.BLACK);

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
        // Force to push the cubes along the slope
        Vector3 force = rb.mass * acceleration * -rb.transform.right;
        Vector3 force2 = rb2.mass * acceleration * -rb2.transform.right;

        // Tangent component of Gravitaional force 
        Vector3 gravityTangent = rb.mass * Vector3.Dot(Physics.gravity, rb.transform.right) * rb.transform.right;
        Vector3 gravityTangent2 = rb2.mass * Vector3.Dot(Physics.gravity, rb2.transform.right) * rb2.transform.right;

        // Push the cubes with the force
        rb.AddForce(force, ForceMode.Force);
        rb2.AddForce(force2, ForceMode.Force);

        // Draw vectors
        arrow.OrientVector(rb.transform, force/1000F);
        arrow2.OrientVector(rb2.transform, force2/1000F);
        arrowGravityTangent.OrientVector(rb.transform, gravityTangent / 1000F);
        arrowGravityTangent2.OrientVector(rb2.transform, gravityTangent2 / 1000F);
    }

    private void GoHome()
    {
        SceneManager.LoadScene("Scenes/Menu");
    }
}
