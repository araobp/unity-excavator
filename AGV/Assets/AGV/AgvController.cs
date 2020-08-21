using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class AgvController : MonoBehaviour
{
    public float mass = 250F;
    public float maxTorque = 20F; // maximum torque the motor can apply to wheel
    public float maxSpeed = 60F;  // 60 meters / min
    public float slowDownMaxTorque = 5F;
    public float slowDownMaxSpeed = 10F;
    public float wheelDampingRate = 0.8F;
    public bool display = false;

    Rigidbody rb;

    GameObject bar1;
    GameObject bar2;
    GameObject body1;
    GameObject body2;
    GameObject guard;
    GameObject loadingPlatform;
    GameObject sensorFront;
    GameObject sensorRear;

    GameObject leftFrontLineTracker;
    GameObject rightFrontLineTracker;
    GameObject leftRearLineTracker;
    GameObject rightRearLineTracker;
    GameObject leftFrontLineTracker2;
    GameObject rightFrontLineTracker2;
    GameObject leftRearLineTracker2;
    GameObject rightRearLineTracker2;

    GameObject leftFrontWheel;
    GameObject rightFrontWheel;
    GameObject leftRearWheel;
    GameObject rightRearWheel;

    WheelCollider leftFrontWheelCollider;
    WheelCollider rightFrontWheelCollider;
    WheelCollider leftRearWheelCollider;
    WheelCollider rightRearWheelCollider;

    PhysicMaterial metalMaterial;
    PhysicMaterial woodMaterial;
    PhysicMaterial rubberMaterial;
    PhysicMaterial floorMaterial;

    Text textTorque;
    Text textSpeed;

    float maxSpeedInSiUnit;
    float deltaTorque;
    float currentTorque = 0F;
    bool convergencePlus = true;

    bool emergencyStop = false;

    private float RoundTo1st(float value)
    {
        return Mathf.RoundToInt(value * 10F) / 10F;
    }

    public void Start()
    {

        metalMaterial = Resources.Load("Metal", typeof(Material)) as PhysicMaterial;
        woodMaterial = Resources.Load("Wood", typeof(Material)) as PhysicMaterial;
        rubberMaterial = Resources.Load("Rubber", typeof(Material)) as PhysicMaterial;
        floorMaterial = Resources.Load("Floor", typeof(Material)) as PhysicMaterial;

        bar1 = transform.Find("Bar1").gameObject;
        bar2 = transform.Find("Bar2").gameObject;
        body1 = transform.Find("Body1").gameObject;
        body2 = transform.Find("Body2").gameObject;
        guard = transform.Find("Guard").gameObject;
        loadingPlatform = transform.Find("LoadingPlatform").gameObject;
        sensorFront = transform.Find("SensorFront").gameObject;
        sensorRear = transform.Find("SensorRear").gameObject;
        leftFrontLineTracker = transform.Find("LeftFrontLineTracker").gameObject;
        rightFrontLineTracker = transform.Find("RightFrontLineTracker").gameObject;
        leftRearLineTracker = transform.Find("LeftRearLineTracker").gameObject;
        rightRearLineTracker = transform.Find("RightRearLineTracker").gameObject;
        leftFrontLineTracker2 = transform.Find("LeftFrontLineTracker2").gameObject;
        rightFrontLineTracker2 = transform.Find("RightFrontLineTracker2").gameObject;
        leftRearLineTracker2 = transform.Find("LeftRearLineTracker2").gameObject;
        rightRearLineTracker2 = transform.Find("RightRearLineTracker2").gameObject;
        leftFrontWheel = transform.Find("LeftFrontWheel").gameObject;
        rightFrontWheel = transform.Find("RightFrontWheel").gameObject;
        leftRearWheel = transform.Find("LeftRearWheel").gameObject;
        rightRearWheel = transform.Find("RightRearWheel").gameObject;

        gameObject.AddComponent<Rigidbody>();
        rb = gameObject.GetComponent<Rigidbody>();
        rb.mass = mass;

        bar1.AddComponent<MeshCollider>();
        MeshCollider bar1Collider = bar1.GetComponent<MeshCollider>();
        bar1Collider.convex = true;
        bar1Collider.material = metalMaterial;
        bar2.AddComponent<MeshCollider>();
        MeshCollider bar2Collider = bar2.GetComponent<MeshCollider>();
        bar2Collider.convex = true;
        bar2Collider.material = metalMaterial;

        body1.AddComponent<MeshCollider>();
        MeshCollider body1Collider = body1.GetComponent<MeshCollider>();
        body1Collider.convex = true;
        body1Collider.material = metalMaterial;
        body2.AddComponent<MeshCollider>();
        MeshCollider body2Collider = body2.GetComponent<MeshCollider>();
        body2Collider.convex = true;
        body2Collider.material = metalMaterial;

        guard.AddComponent<MeshCollider>();
        MeshCollider guardCollider = guard.GetComponent<MeshCollider>();
        guardCollider.convex = true;
        guardCollider.material = rubberMaterial;

        loadingPlatform.AddComponent<BoxCollider>();
        BoxCollider loadingPlatformCollider = loadingPlatform.GetComponent<BoxCollider>();
        loadingPlatformCollider.material = metalMaterial;

        // Wheel colliders
        List<WheelCollider> wheelColliders = new List<WheelCollider>();
        leftFrontWheel.AddComponent<WheelCollider>();
        rightFrontWheel.AddComponent<WheelCollider>();
        leftRearWheel.AddComponent<WheelCollider>();
        rightRearWheel.AddComponent<WheelCollider>();

        leftFrontWheelCollider = leftFrontWheel.GetComponent<WheelCollider>();
        wheelColliders.Add(leftFrontWheelCollider);
        rightFrontWheelCollider = rightFrontWheel.GetComponent<WheelCollider>();
        wheelColliders.Add(rightFrontWheelCollider);
        leftRearWheelCollider = leftRearWheel.GetComponent<WheelCollider>();
        wheelColliders.Add(leftRearWheelCollider);
        rightRearWheelCollider = rightRearWheel.GetComponent<WheelCollider>();
        wheelColliders.Add(rightRearWheelCollider);
        foreach (WheelCollider wc in wheelColliders)
        {
            wc.radius = 0.09F;
            wc.center = new Vector3(0, 0, 0.01F);
            wc.suspensionDistance = 0.02F;
            wc.wheelDampingRate = wheelDampingRate;
            JointSpring ss = wc.suspensionSpring;
            ss.spring = 35000F * 1F;
            ss.damper = 4500F * 0.5F;
            ss.targetPosition = 0.5F;
            wc.suspensionSpring = ss;
        }

        maxSpeedInSiUnit = maxSpeed / 60F;
        currentTorque = maxTorque;

        textTorque = GameObject.FindWithTag("TextTorque").GetComponent<Text>();
        textSpeed = GameObject.FindWithTag("TextSpeed").GetComponent<Text>();

        // Hazard area
        GameObject red;
        GameObject yellow;

        red = transform.Find("FrontHazardRed").gameObject;
        yellow = transform.Find("FrontHazardYellow").gameObject;
        red.AddComponent<BoxCollider>();
        red.GetComponent<BoxCollider>().isTrigger = true;
        red.AddComponent<HazardArea>();
        red.GetComponent<HazardArea>().SetController(this, true);
        yellow.AddComponent<BoxCollider>();
        yellow.GetComponent<BoxCollider>().isTrigger = true;
        yellow.AddComponent<HazardArea>();
        yellow.GetComponent<HazardArea>().SetController(this, false);

        red = transform.Find("RearHazardRed").gameObject;
        yellow = transform.Find("RearHazardYellow").gameObject;
        red.AddComponent<BoxCollider>();
        red.GetComponent<BoxCollider>().isTrigger = true;
        red.AddComponent<HazardArea>();
        red.GetComponent<HazardArea>().SetController(this, true);
        yellow.AddComponent<BoxCollider>();
        yellow.GetComponent<BoxCollider>().isTrigger = true;
        yellow.AddComponent<HazardArea>();
        yellow.GetComponent<HazardArea>().SetController(this, false);
    }

    public void FixedUpdate()
    {
        RaycastHit hit;

        bool leftHit = false;
        bool rightHit = false;
        bool leftHit2 = false;
        bool rightHit2 = false;

        bool forward = true;

        if (Vector3.Angle(rb.velocity, transform.forward) <= 90F)
        {
            forward = true;
            Physics.Raycast(leftFrontLineTracker.transform.position, leftFrontLineTracker.transform.TransformDirection(Vector3.back), out hit, 0.2F);
            if (hit.collider.tag == "Line") leftHit = true;
            Physics.Raycast(rightFrontLineTracker.transform.position, rightFrontLineTracker.transform.TransformDirection(Vector3.back), out hit, 0.2F);
            if (hit.collider.tag == "Line") rightHit = true;
            Physics.Raycast(leftFrontLineTracker2.transform.position, leftFrontLineTracker2.transform.TransformDirection(Vector3.back), out hit, 0.2F);
            if (hit.collider.tag == "Line") leftHit2 = true;
            Physics.Raycast(rightFrontLineTracker2.transform.position, rightFrontLineTracker2.transform.TransformDirection(Vector3.back), out hit, 0.2F);
            if (hit.collider.tag == "Line") rightHit2 = true;
        }
        else  // Reverse
        {
            forward = false;
            Physics.Raycast(rightRearLineTracker.transform.position, rightRearLineTracker.transform.TransformDirection(Vector3.back), out hit, 0.2F);
            if (hit.collider.tag == "Line") leftHit = true;
            Physics.Raycast(leftRearLineTracker.transform.position, leftRearLineTracker.transform.TransformDirection(Vector3.back), out hit, 0.2F);
            if (hit.collider.tag == "Line") rightHit = true;
            Physics.Raycast(rightRearLineTracker2.transform.position, rightRearLineTracker2.transform.TransformDirection(Vector3.back), out hit, 0.2F);
            if (hit.collider.tag == "Line") leftHit2 = true;
            Physics.Raycast(leftRearLineTracker2.transform.position, leftRearLineTracker2.transform.TransformDirection(Vector3.back), out hit, 0.2F);
            if (hit.collider.tag == "Line") rightHit2 = true;
        }

        if (leftHit || rightHit) Debug.Log($"LeftHit: {leftHit}, RightHit: {rightHit}");

        float deltaAngle;

        if (rightHit2 && leftHit2)  // Crossing
        {
            leftFrontWheelCollider.steerAngle = 0F;
            rightFrontWheelCollider.steerAngle = 0F;
            leftRearWheelCollider.steerAngle = 0F;
            rightRearWheelCollider.steerAngle = 0F;
        }
        else if (!rightHit && !leftHit)  // Along the track
        {
            float angle = leftFrontWheelCollider.steerAngle;
            deltaAngle = angle / 3F;
            leftFrontWheelCollider.steerAngle -= deltaAngle;
            rightFrontWheelCollider.steerAngle -= deltaAngle;
            leftRearWheelCollider.steerAngle += deltaAngle;
            rightRearWheelCollider.steerAngle += deltaAngle;
            /*
            leftFrontWheelCollider.steerAngle = 0F;
            rightFrontWheelCollider.steerAngle = 0F;
            leftRearWheelCollider.steerAngle = 0F;
            rightRearWheelCollider.steerAngle = 0F;
            */
        }
        else  // over the track
        {

            float delta = 1F;
            if (!forward) delta = -1F;
            if (rightHit) deltaAngle = delta;
            else deltaAngle = -delta;

            leftFrontWheelCollider.steerAngle += deltaAngle;
            rightFrontWheelCollider.steerAngle += deltaAngle;
            leftRearWheelCollider.steerAngle -= deltaAngle;
            rightRearWheelCollider.steerAngle -= deltaAngle;
        }

        if (rb.velocity.magnitude < maxSpeedInSiUnit) {
            currentTorque = maxTorque;
        } else
        {
            currentTorque = 0F;
        }
            leftFrontWheelCollider.motorTorque = currentTorque;
            rightFrontWheelCollider.motorTorque = currentTorque;
            leftRearWheelCollider.motorTorque = currentTorque;
            rightRearWheelCollider.motorTorque = currentTorque;

        if (display)
        {
            textTorque.text = $"Torque: 4 x {currentTorque} N*m";
            textSpeed.text = $"Speed: {-Mathf.RoundToInt(rb.velocity.z * 60F)} m/min";
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EmergencyStop")
        {
            maxTorque = 0F;
        } 
    }

    public void OnHazardEnter(bool red)  // red: true, yellow: false
    {
        if (red)
        {
            maxTorque = 0F;
            maxSpeedInSiUnit = 0F;
            emergencyStop = true;
        }
        else
        {
            if (!emergencyStop)
            {
                maxTorque = slowDownMaxTorque;
                maxSpeedInSiUnit = slowDownMaxSpeed / 60F;
            }
        }
    }
}