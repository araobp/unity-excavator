using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcavatorController : MonoBehaviour
{

    private const float ROTATION_SPEED = 20F;
    private const float RUNNING_SPEED = 1F;

    const string RUNNING_AXIS = "Bone/RunningAxis";
    const string ENGINE_AXIS = RUNNING_AXIS + "/Bone.002/EngineAxis";
    const string STICK_BONE = ENGINE_AXIS + "/Bone.004/StickBone";
    const string BUCKET_AXIS = STICK_BONE + "/Bone.014/Bone.016/BucketAxis";

    private Transform runningAxis;
    private Transform engineAxis;
    private Transform stickBone;
    private Transform bucketAxis;

    private Transform engineRightCylinder;
    private Transform engineLeftCylinder;
    private Transform engineLeftCylinderTarget;
    private Transform engineRightCylinderTarget;

    private Transform armLeftCylinderTarget;
    private Transform armRightCylinderTarget;
    private Transform armRightCylinder;
    private Transform armLeftCylinder;

    private Transform bucketCylinderA;
    private Transform bucketCylinderB;
    private Transform bucketCylinderATarget;
    private Transform bucketCylinderBTarget;

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
        runningAxis = transform.Find(RUNNING_AXIS);
        engineAxis = transform.Find(ENGINE_AXIS);
        stickBone = transform.Find(STICK_BONE);
        bucketAxis = transform.Find(BUCKET_AXIS);

        bucketCylinderA = transform.Find(BUCKET_AXIS + "/BucketCylinderA");
        bucketCylinderB = transform.Find(STICK_BONE + "/Bone.014/BucketCylinderB");
        bucketCylinderATarget = transform.Find(STICK_BONE + "/Bone.014/BucketCylinderB/BucketCylinderATarget");
        bucketCylinderBTarget = transform.Find(BUCKET_AXIS + "/BucketCylinderA/BucketCylinderBTarget");

        engineRightCylinder = transform.Find(RUNNING_AXIS + "/Bone.005/EngineRightCylinder");
        engineLeftCylinder = transform.Find(RUNNING_AXIS + "/Bone.006/EngineLeftCylinder");
        engineLeftCylinderTarget = transform.Find(ENGINE_AXIS + "/EngineLeftCylinderTarget");
        engineRightCylinderTarget = transform.Find(ENGINE_AXIS + "EngineRightCylinderTarget");

        armLeftCylinderTarget = transform.Find(RUNNING_AXIS + "/ArmLeftCylinderTarget");
        armRightCylinderTarget = transform.Find(RUNNING_AXIS + "/ArmRightCylinderTarget");
        armRightCylinder = transform.Find(ENGINE_AXIS + "/Bone.004/Bone.007/ArmRightCylinder");
        armLeftCylinder = transform.Find(ENGINE_AXIS + "/Bone.004/Bone.008/ArmLeftCylinder");
    }

    // Update is called once per frame
    void Update()
    {
        float rotation = ROTATION_SPEED * Time.deltaTime;
        float running = RUNNING_SPEED * Time.deltaTime;

        if (Input.GetKey(KeyCode.G))
        {
            runningAxis.Rotate(new Vector3(0, rotation, 0));
        }
        else if (Input.GetKey(KeyCode.J))
        {
            runningAxis.Rotate(new Vector3(0, -rotation, 0));
        }
        else if (Input.GetKey(KeyCode.U))
        {
            var pos = new Vector3(-running * 2F, 0, 0);
            transform.Translate(pos);
        }
        else if (Input.GetKey(KeyCode.N))
        {
            var pos = new Vector3(running * 2F, 0, 0);
            transform.Translate(pos);
        }
        else if (Input.GetKey(KeyCode.Y))
        {
            engineAxis.Rotate(new Vector3(rotation * 0.5F, 0, 0));
            OrientArmHydraulicCylinder();
        }
        else if (Input.GetKey(KeyCode.B))
        {
            engineAxis.Rotate(new Vector3(-rotation * 0.5F, 0, 0));
            OrientArmHydraulicCylinder();
        }
        else if (Input.GetKey(KeyCode.I))
        {
            if (stickBone.localPosition.y <= 0.024)
            {
                stickBone.Translate(new Vector3(0, running * 0.5F, 0));
            }
            Debug.Log("Stickbone: " + stickBone.transform.localPosition.y);
        }
        else if (Input.GetKey(KeyCode.M))
        {
            if (stickBone.localPosition.y >= 0.0094)
            {
                stickBone.Translate(new Vector3(0, -running * 0.5F, 0));
            }
            Debug.Log("Stickbone: " + stickBone.transform.localPosition.y);
        }
        else if (Input.GetKey(KeyCode.L))
        {
            bucketAxis.Rotate(new Vector3(rotation * 1.5F, 0, 0));
            OrientBucketHydraulicCylinder();
        }
        else if (Input.GetKey(KeyCode.K))
        {
            bucketAxis.Rotate(new Vector3(-rotation * 1.5F, 0, 0));
            OrientBucketHydraulicCylinder();
        }

        float joystickLeftX = Input.GetAxis("JoystickLeftX");
        if (joystickLeftX != 0)
        {
            var rot = -rotation * joystickLeftX;
            runningAxis.Rotate(new Vector3(0, rot, 0));
        }

        float joystickLeftY = Input.GetAxis("JoystickLeftY");
        if (joystickLeftY != 0)
        {
            var y = stickBone.localPosition.y;
            var run = -running * joystickLeftY;
            if ((run <= 0 && y >= 0.0094) || (run > 0 && y <= 0.024))
            {
                stickBone.Translate(new Vector3(0, run * 0.5F, 0));
            }
        }

        float joystickRightX = Input.GetAxis("JoystickRightX");
        if (joystickRightX != 0)
        {
            var rot = -rotation * joystickRightX;
            bucketAxis.Rotate(new Vector3(rot * 2F, 0, 0));
            OrientBucketHydraulicCylinder();
        }

        float joystickRightY = Input.GetAxis("JoystickRightY");
        if (joystickRightY != 0)
        {
            var rot = rotation * joystickRightY;
            engineAxis.Rotate(new Vector3(rot * 0.5F, 0, 0));
            OrientArmHydraulicCylinder();
        }

        if (Input.GetKey(KeyCode.Joystick1Button0))
        {
            var pos = new Vector3(running * 2F, 0, 0);
            transform.Translate(pos);
        }

        else if (Input.GetKey(KeyCode.Joystick1Button3))
        {
            var pos = new Vector3(-running * 2F, 0, 0);
            transform.Translate(pos);
        }

    }
}