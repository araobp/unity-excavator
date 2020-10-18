using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlanetController : MonoBehaviour
{
    public float initialSpeed = 20F;
    public float rotation = 0F;
    public float angularVelocity = 0.5F;

    GameObject star;
    float starMass;
    float planetMass;
    Rigidbody rb;

    const float G = 6.674F;  // This is 10^11 times larger than the real constant of gravitatioin. 

    // Start is called before the first frame update
    void Start()
    {
        star = GameObject.Find("Star");
        starMass = star.GetComponent<Rigidbody>().mass;
        rb = gameObject.GetComponent<Rigidbody>();
        planetMass = rb.mass;
        transform.Rotate(rotation, 0, 0);
        rb.angularVelocity = new Vector3(0, angularVelocity, 0);
        Vector3 initialForward = new Vector3(rb.transform.forward.x, rb.transform.forward.y, rb.transform.forward.z);
        rb.velocity = new Vector3(initialForward.x, initialForward.y, initialForward.z) * initialSpeed;

        GameObject.Find("ButtonClose").GetComponent<Button>().onClick.AddListener(
            delegate
            {
                GoHome();
            }
        );
    }

    void FixedUpdate()
    {
        Vector3 r = star.transform.position - transform.position;
        float gravity = G * starMass * planetMass / Mathf.Pow(r.magnitude, 2F);
        rb.AddForce(gravity * r.normalized, ForceMode.Force);
    }

    private void GoHome()
    {
        SceneManager.LoadScene("Scenes/Menu");
    }
}
