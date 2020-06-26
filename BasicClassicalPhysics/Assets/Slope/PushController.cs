using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushController : MonoBehaviour
{
    public float acceleration = 4F;
    private Rigidbody rb;
    private Rigidbody rb2;

    public GameObject arrowPrefab;

    private GameObject arrow;
    private GameObject arrow2;

    void OrientVector(GameObject arrow, Transform transform, Vector3 vector)
    {
        Vector3 startPoint = transform.position;
        Vector3 endPoint = vector + transform.position;
        Vector3 direction = endPoint - startPoint;

        float length = direction.magnitude * 10F - 2F;  // 1/10 scale, subtract the cone length
        arrow.transform.position = endPoint;
        arrow.transform.GetChild(1).transform.localScale = new Vector3(1F, 1F, length);
        arrow.transform.LookAt(transform);
    }

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

        arrow = Instantiate(arrowPrefab);
        arrow2 = Instantiate(arrowPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 force = rb.mass * acceleration * -rb.transform.right;
        Vector3 force2 = rb2.mass * acceleration * -rb2.transform.right;

        rb.AddForce(force, ForceMode.Force);
        rb2.AddForce(force2, ForceMode.Force);

        OrientVector(arrow, rb.transform, force/1000F);
        OrientVector(arrow2, rb2.transform, force2/1000F);
    }
}
