using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveController : MonoBehaviour
{

    public float speed = 60F;  // in km/h

    Rigidbody rb;
    BoxCollider cl;

    PhysicMaterial phyMatZero;
    PhysicMaterial phyMatBrake;

    float vX;
    float vY;
    float vZ;
    float maxDeltaAccel = 0F;

    // Start is called before the first frame update
    void Start()
    {

        rb = gameObject.GetComponent<Rigidbody>();
        rb.mass = transform.localScale.x * transform.localScale.y * transform.localScale.z * 100F;
        rb.velocity = speed * 1000 / 3600F * transform.forward;
        cl = gameObject.GetComponent<BoxCollider>();

        phyMatZero = new PhysicMaterial();
        phyMatZero.dynamicFriction = 0F;
        phyMatZero.staticFriction = 0F;

        phyMatBrake = new PhysicMaterial();
        phyMatBrake.dynamicFriction = 0.2F;
        phyMatBrake.staticFriction = 0.2F;

        cl.material = phyMatZero;

        vX = rb.velocity.x;
        vY = rb.velocity.y;
        vZ = rb.velocity.z;

    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        Vector3 prevVelocity = new Vector3(vX, vY, vZ);
        vX = rb.velocity.x;
        vY = rb.velocity.y;
        vZ = rb.velocity.z;

        float deltaSpeed = (rb.velocity - prevVelocity).magnitude;
        float deltaAccel = deltaSpeed / deltaTime;
        if (deltaAccel > maxDeltaAccel)
        {
            maxDeltaAccel = deltaAccel;
            Debug.Log($"Max delta accel: {gameObject.name}:{maxDeltaAccel}");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        cl.material = phyMatBrake;
    }
}
