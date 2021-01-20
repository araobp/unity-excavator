using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcavatorController : MonoBehaviour
{

    private const float ROTATION_SPEED = 20F;
    private const float RUNNING_SPEED = 1F;

    private GameObject runningAxis;
    private GameObject engineAxis;
    private GameObject stickBone;
    private GameObject bucketAxis;

    private GameObject engineRightCylinder;
    private GameObject engineLeftCylinder;
    private GameObject engineLeftCylinderTarget;
    private GameObject engineRightCylinderTarget;

    private GameObject armLeftCylinderTarget;
    private GameObject armRightCylinderTarget;
    private GameObject armRightCylinder;
    private GameObject armLeftCylinder;

    private GameObject bucketCylinderA;
    private GameObject bucketCylinderB;
    private GameObject bucketCylinderATarget;
    private GameObject bucketCylinderBTarget;

    void OrientBucketHydraulicCylinder()
    {
        bucketCylinderA.transform.LookAt(bucketCylinderATarget.transform);
        bucketCylinderB.transform.LookAt(bucketCylinderBTarget.transform);

        bucketCylinderA.transform.Rotate(new Vector3(90F, 0F, 0F));
        bucketCylinderB.transform.Rotate(new Vector3(90F, 0F, 0F));
    }

    void OrientArmHydraulicCylinder()
    {
        engineRightCylinder.transform.LookAt(engineRightCylinderTarget.transform);
        engineLeftCylinder.transform.LookAt(engineLeftCylinderTarget.transform);
        armRightCylinder.transform.LookAt(armRightCylinderTarget.transform);
        armLeftCylinder.transform.LookAt(armLeftCylinderTarget.transform);

        engineRightCylinder.transform.Rotate(new Vector3(90F, 0F, 0F));
        engineLeftCylinder.transform.Rotate(new Vector3(90F, 0F, 0F));
        armRightCylinder.transform.Rotate(new Vector3(90F, 0F, 0F));
        armLeftCylinder.transform.Rotate(new Vector3(90F, 0F, 0F));
    }

    // Start is called before the first frame update
    void Start()
    {
        runningAxis = GameObject.Find("RunningAxis") as GameObject;
        engineAxis = GameObject.Find("EngineAxis") as GameObject;
        stickBone = GameObject.Find("StickBone");
        bucketAxis = GameObject.Find("BucketAxis");

        bucketCylinderA = GameObject.Find("BucketCylinderA");
        bucketCylinderB = GameObject.Find("BucketCylinderB");
        bucketCylinderATarget = GameObject.Find("BucketCylinderATarget");
        bucketCylinderBTarget = GameObject.Find("BucketCylinderBTarget");

        engineRightCylinder = GameObject.Find("EngineRightCylinder");
        engineLeftCylinder = GameObject.Find("EngineLeftCylinder");
        engineLeftCylinderTarget = GameObject.Find("EngineLeftCylinderTarget");
        engineRightCylinderTarget = GameObject.Find("EngineRightCylinderTarget");

        armLeftCylinderTarget = GameObject.Find("ArmLeftCylinderTarget");
        armRightCylinderTarget = GameObject.Find("ArmRightCylinderTarget");
        armRightCylinder = GameObject.Find("ArmRightCylinder");
        armLeftCylinder = GameObject.Find("ArmLeftCylinder");

}

// Update is called once per frame
void Update()
    {
        float rotation = ROTATION_SPEED * Time.deltaTime;
        float running = RUNNING_SPEED * Time.deltaTime;

        if (Input.GetKey(KeyCode.G))
        {
            runningAxis.transform.Rotate(new Vector3(0, rotation, 0));
        } else if (Input.GetKey(KeyCode.J))
        {
            runningAxis.transform.Rotate(new Vector3(0, -rotation, 0));
        } else if (Input.GetKey(KeyCode.U))
        {
            var pos = new Vector3(-running * 2F, 0, 0);
            transform.Translate(pos);
        } else if (Input.GetKey(KeyCode.N))
        {
            var pos = new Vector3(running * 2F, 0, 0);
            transform.Translate(pos);
        }
        else if (Input.GetKey(KeyCode.Y))
        {
            engineAxis.transform.Rotate(new Vector3(rotation * 0.5F, 0, 0));
            OrientArmHydraulicCylinder();
        } else if (Input.GetKey(KeyCode.B))
        {
            engineAxis.transform.Rotate(new Vector3(-rotation * 0.5F, 0, 0));
            OrientArmHydraulicCylinder();
        } else if (Input.GetKey(KeyCode.I))
        {
            if (stickBone.transform.localPosition.y <= 0.024)
            {
                stickBone.transform.Translate(new Vector3(0, running * 0.5F, 0));
            }
            Debug.Log("Stickbone: " + stickBone.transform.localPosition.y);
        } else if (Input.GetKey(KeyCode.M))
        {
            if (stickBone.transform.localPosition.y >= 0.0094)
            {
                stickBone.transform.Translate(new Vector3(0, -running * 0.5F, 0));
            }
            Debug.Log("Stickbone: " + stickBone.transform.localPosition.y);
        }
        else if (Input.GetKey(KeyCode.L))
        {
            bucketAxis.transform.Rotate(new Vector3(rotation * 1.5F, 0, 0));
            OrientBucketHydraulicCylinder();
        }
        else if (Input.GetKey(KeyCode.K))
        {
            bucketAxis.transform.Rotate(new Vector3(-rotation * 1.5F, 0, 0));
            OrientBucketHydraulicCylinder();
        }

        float joystickLeftX = Input.GetAxis("JoystickLeftX");
        if (joystickLeftX != 0)
        {
            var rot = -rotation * joystickLeftX;
            runningAxis.transform.Rotate(new Vector3(0, rot, 0));
        }

        float joystickLeftY = Input.GetAxis("JoystickLeftY");
        if (joystickLeftY != 0)
        {
            var y = stickBone.transform.localPosition.y;
            var run = -running * joystickLeftY;
            if ((run <= 0 && y >= 0.0094) || (run > 0 && y <= 0.024)) {
                stickBone.transform.Translate(new Vector3(0, run * 0.5F, 0));
            }
        }

        float joystickRightX = Input.GetAxis("JoystickRightX");
        if (joystickRightX != 0)
        {
            var rot = -rotation * joystickRightX;
            bucketAxis.transform.Rotate(new Vector3(rot * 2F, 0, 0));
            OrientBucketHydraulicCylinder();
        }

        float joystickRightY = Input.GetAxis("JoystickRightY");
        if (joystickRightY != 0)
        {
            var rot = rotation * joystickRightY;
            engineAxis.transform.Rotate(new Vector3(rot * 0.5F, 0, 0));
            OrientArmHydraulicCylinder();
        }

        if (Input.GetKey(KeyCode.Joystick1Button0)) {
            var pos = new Vector3(running * 2F, 0, 0);
            transform.Translate(pos);
        }

        else if (Input.GetKey(KeyCode.Joystick1Button3)) {
            var pos = new Vector3(-running * 2F, 0, 0);
            transform.Translate(pos);
        }

    }
} 