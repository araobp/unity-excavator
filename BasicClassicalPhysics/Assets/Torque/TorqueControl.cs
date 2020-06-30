using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TorqueControl : MonoBehaviour
{
    public float force = 1F;
    private Rigidbody rb;

    private float startTime;

    private float momentOfInertia;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 10000000F;
        startTime = Time.time;

        // [Reference] http://hyperphysics.phy-astr.gsu.edu/hbase/mi.html#mi
        momentOfInertia = 2F / 5F * rb.mass * Mathf.Pow(rb.transform.localScale.x / 2F, 2F);
    }

    bool stopPowering = false;


    // Update is called once per frame
    void Update()
    {
        // [Reference] http://hyperphysics.phy-astr.gsu.edu/hbase/rke.html
        float rotationalEnergy = 1F / 2F * momentOfInertia * Mathf.Pow(rb.angularVelocity.magnitude, 2F);

        if (!stopPowering)
        {
            if (rotationalEnergy >= 20F)
            {
                stopPowering = true;
                float elapsedTime = Time.time - startTime;
                Debug.Log($"Elapsed time: {elapsedTime} for {gameObject.name}");
            }
            else
            {
                rb.AddTorque(transform.up * force);
            }
        }
    }
}
