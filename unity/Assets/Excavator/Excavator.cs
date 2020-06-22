
using System;
using System.Collections;
using UnityEngine;

public class Geometry {

    Transform swingAxis;
    Transform bodyBoomJoint;
    Transform boomArmJoint;
    Transform armBucketJoint;
    Transform hookJoint;

    public Geometry(Transform swingAxis, Transform bodyBoomJoint, Transform boomArmJoint, Transform armBucketJoint, Transform hookJoint)
    {
        this.swingAxis = swingAxis;
        this.bodyBoomJoint = bodyBoomJoint;
        this.boomArmJoint = boomArmJoint;
        this.armBucketJoint = armBucketJoint;
        this.hookJoint = hookJoint;
    }

    public float verticalAngle
    {
        get {
            Vector3 turnVector = swingAxis.up;
            Vector3 boomVector = boomArmJoint.position - bodyBoomJoint.position;
            return Vector3.Angle(turnVector, boomVector);
        }
    }

    public float boomArmAngle
    {
        get
        {
            Vector3 boomVector = boomArmJoint.position - bodyBoomJoint.position;
            Vector3 armVector = armBucketJoint.position - boomArmJoint.position;
            return 180F - Vector3.Angle(boomVector, armVector);
        }
    }

    public float boomArmHookAngle
    {
        get
        {
            Vector3 boomVector = boomArmJoint.position - bodyBoomJoint.position;
            Vector3 armVector = hookJoint.position - boomArmJoint.position;
            return 180F - Vector3.Angle(boomVector, armVector);
        }
    }

}

public class Position
{
    public float R;
    public float E;
    public float S;
    public float A;
    public float B;
    public float C;
    public float D;

    public float verticalAngleZero;
    public float boomArmAngleZero;
    public float boomArmHookAngleZero;
}

public class DriveParams
{
    private float _mass;

    private float _maxSpeed;
    private float _creepSpeed;

    private float _initialAccel;
    private float _deltaAccel;
    private float _maxAccel;

    private float _deltaSwing;

    public DriveParams(float mass, float maxSpeed, float creepSpeed, float initialAccel, float deltaAccel, float maxAccel, float deltaSwing)
    {
        _mass = mass;
        _maxSpeed = maxSpeed;
        _creepSpeed = creepSpeed;
        _initialAccel = initialAccel;
        _deltaAccel = deltaAccel;
        _maxAccel = maxAccel;
        _deltaSwing = deltaSwing;
    }

    public float mass
    {
        get { return _mass; }
    }

    public float maxSpeed
    {
        get { return _maxSpeed;  }
    }

    public float creepSpeed
    {
        get { return _creepSpeed; }
    }

    public float initialAccel
    {
        get { return _initialAccel;  }
    }

    public float deltaAccel
    {
        get { return _deltaAccel;  }
    }

    public float maxAccel
    {
        get { return _maxAccel;  }
    }

    public float deltaSwing
    {
        get { return _deltaSwing;  }
    }
}

public class Excavator
{

    static string excavatorForwardPath = "Armature";
    static string swingAxisPath = "Armature/TurnAxis";
    static string boomAxisPath = "Armature/TurnAxis/Bone.001/BoomAxis";
    static string armAxisPath = "Armature/TurnAxis/Bone.001/BoomAxis/Bone.003/ArmAxis";
    static string bucketAxisPath = "Armature/TurnAxis/Bone.001/BoomAxis/Bone.003/ArmAxis/ArmAxis.002/ArmAxis.003/BucketAxis";
    static string armLinkageAxisPath = "Armature/TurnAxis/Bone.001/BoomAxis/Bone.003/ArmAxis/ArmAxis.002/ArmLinkageAxis";
    static string bucketLinkageAxisPath = "Armature/TurnAxis/Bone.001/BoomAxis/Bone.003/ArmAxis/ArmAxis.002/ArmAxis.003/BucketAxis/ArmAxis.005/BucketLinkageAxis";

    private Transform excavatorForwardAxis;
    private Transform swingAxis;
    private Transform boomAxis;
    private Transform armAxis;
    private Transform bucketAxis;

    private Transform boomCylinderRightAxis1;
    private Transform boomCylinderRight1Target;

    private Transform boomCylinderRightAxis2;
    private Transform boomCylinderRight2Target;

    private Transform boomCylinderLeftAxis1;
    private Transform boomCylinderLeft1Target;

    private Transform boomCylinderLeftAxis2;
    private Transform boomCylinderLeft2Target;

    private Transform armCylinderAxis1;
    private Transform armCylinder1Target;

    private Transform armCylinderAxis2;
    private Transform armCylinder2Target;

    private Transform bucketCylinderAxis1;
    private Transform bucketCylinder1Target;

    private Transform bucketCylinderAxis2;
    private Transform bucketCylinder2Target;

    private Transform armLinkageAxis;
    private Transform armLinkageTarget;

    private Transform bucketLinkageAxis;

    private Transform hookMainAxis;

    /* Cockpit */

    private Transform rightOperationLeverAxis;
    private Transform leftOperationLeverAxis;

    private Transform rightPedalAxis;
    private Transform leftPedalAxis;

    private Transform leftTravelLeverAxis;
    private Transform rightTravelLeverAxis;

    Vector3 joystickRightEulerAngles;
    Vector3 joystickLeftEulerAngles;

    Vector3 pedalRightEulerAngles;
    Vector3 pedalLeftEulerAngles;

    Vector3 leverRightEulerAngles;
    Vector3 leverLeftEulerAngles;

    /* Geometory */
    Transform bodyBoomJoint;
    Transform boomArmJoint;
    Transform armBucketJoint;

    float boomLength;
    float armLength;
    float armLengthHook;
    float boomLength2;
    float armLength2;
    float armLengthHook2;

    private GameObject excavator;

    Quaternion swingAxisInitialQ;
    Quaternion boomAxisInitialQ;
    Quaternion armAxisInitialQ;
    Quaternion bucketAxisInitialQ;

    private Geometry geometry;
    private float verticalAngleZero;
    private float boomArmAngleZero;
    private float boomArmHookAngleZero;

    GameObject bucket;
    Transform bucketCuttingEdges;
    Transform bucketPosition;
    GameObject arm;
    GameObject boom;
    GameObject body;
    GameObject cabin;
    GameObject rightTrack;
    GameObject leftTrack;

    public Excavator(GameObject excavator)
    {
        this.excavator = excavator;
        Transform transform = excavator.transform;

        excavatorForwardAxis = transform.Find(excavatorForwardPath);
        swingAxis = transform.Find(swingAxisPath);
        boomAxis = transform.Find(boomAxisPath);
        armAxis = transform.Find(armAxisPath);
        bucketAxis = transform.Find(bucketAxisPath);

        swingAxisInitialQ = swingAxis.localRotation;
        boomAxisInitialQ = boomAxis.localRotation;
        armAxisInitialQ = armAxis.localRotation;
        bucketAxisInitialQ = bucketAxis.localRotation;

        armLinkageAxis = transform.Find(armLinkageAxisPath);
        bucketLinkageAxis = transform.Find(bucketLinkageAxisPath);

        boomCylinderRightAxis1 = transform.Find(swingAxisPath + "/Bone.001/Bone.011/BoomCylinderRightAxis1");
        boomCylinderRight1Target = transform.Find(boomAxisPath + "/BoomCylinderRight1Target");

        boomCylinderRightAxis2 = transform.Find(boomAxisPath + "/Bone.015/BoomCylinderRightAxis2");
        boomCylinderRight2Target = transform.Find(swingAxisPath + "/BoomCylinderRight2Target");

        boomCylinderLeftAxis1 = transform.Find(swingAxisPath + "/Bone.001/Bone.012/BoomCylinderLeftAxis1");
        boomCylinderLeft1Target = transform.Find(boomAxisPath + "/BoomCylinderLeft1Target");

        boomCylinderLeftAxis2 = transform.Find(boomAxisPath + "/Bone.016/BoomCylinderLeftAxis2");
        boomCylinderLeft2Target = transform.Find(swingAxisPath + "/BoomCylinderLeft2Target");

        armCylinderAxis1 = transform.Find(boomAxisPath + "/ArmCylinderAxis1");
        armCylinder1Target = transform.Find(armAxisPath + "/ArmCylinder1Target");

        armCylinderAxis2 = transform.Find(armAxisPath + "/Bone.010/ArmCylinderAxis2");
        armCylinder2Target = transform.Find(boomAxisPath + "/ArmCylinder2Target");

        bucketCylinderAxis1 = transform.Find(armAxisPath + "/BucketCylinderAxis1");
        bucketCylinder1Target = transform.Find(armLinkageAxisPath + "/BucketCylinder1Target");

        bucketCylinderAxis2 = transform.Find(armLinkageAxisPath + "/BucketCylinderAxis2");
        bucketCylinder2Target = transform.Find(armAxisPath + "/BucketCylinder2Target");

        armLinkageAxis = transform.Find(armLinkageAxisPath);
        armLinkageTarget = transform.Find(bucketLinkageAxisPath + "/ArmLinkageTarget");

        bucketLinkageAxis = transform.Find(bucketLinkageAxisPath);

        hookMainAxis = transform.Find(bucketLinkageAxisPath + "/HookMainAxis");

        rightOperationLeverAxis = transform.Find(swingAxisPath + "/TurnAxis.001/TurnAxis.003/JoystickRightAxis");
        leftOperationLeverAxis = transform.Find(swingAxisPath + "/TurnAxis.001/TurnAxis.002/JoystickLeftAxis");

        rightPedalAxis = transform.Find(swingAxisPath + "/TurnAxis.001/TurnAxis.004/PedalRightAxis");
        leftPedalAxis = transform.Find(swingAxisPath + "/TurnAxis.001/TurnAxis.005/PedalLeftAxis");

        rightTravelLeverAxis = transform.Find(swingAxisPath + "/TurnAxis.001/TurnAxis.004/PedalRight/LeverRightAxis");
        leftTravelLeverAxis = transform.Find(swingAxisPath + "/TurnAxis.001/TurnAxis.005/PedalLeft/LeverLeftAxis");

        joystickLeftEulerAngles = leftOperationLeverAxis.eulerAngles;
        joystickRightEulerAngles = rightOperationLeverAxis.eulerAngles;

        pedalRightEulerAngles = rightPedalAxis.eulerAngles;
        pedalLeftEulerAngles = leftPedalAxis.eulerAngles;

        leverRightEulerAngles = leftTravelLeverAxis.eulerAngles;
        leverLeftEulerAngles = rightTravelLeverAxis.eulerAngles;

        rightOperationLeverAxis = transform.Find(swingAxisPath + "/TurnAxis.001/TurnAxis.003/JoystickRightAxis");
        leftOperationLeverAxis = transform.Find(swingAxisPath + "/TurnAxis.001/TurnAxis.002/JoystickLeftAxis");

        rightPedalAxis = transform.Find(swingAxisPath + "/TurnAxis.001/TurnAxis.004/PedalRightAxis");
        leftPedalAxis = transform.Find(swingAxisPath + "/TurnAxis.001/TurnAxis.005/PedalLeftAxis");

        rightTravelLeverAxis = transform.Find(swingAxisPath + "/TurnAxis.001/TurnAxis.004/PedalRight/LeverRightAxis");
        leftTravelLeverAxis = transform.Find(swingAxisPath + "/TurnAxis.001/TurnAxis.005/PedalLeft/LeverLeftAxis");

        joystickLeftEulerAngles = leftOperationLeverAxis.eulerAngles;
        joystickRightEulerAngles = rightOperationLeverAxis.eulerAngles;

        pedalRightEulerAngles = rightPedalAxis.eulerAngles;
        pedalLeftEulerAngles = leftPedalAxis.eulerAngles;

        leverRightEulerAngles = leftTravelLeverAxis.eulerAngles;
        leverLeftEulerAngles = rightTravelLeverAxis.eulerAngles;

        bodyBoomJoint = transform.Find(boomAxisPath);
        boomArmJoint = transform.Find(boomAxisPath + "/Cylinder.023");
        armBucketJoint = transform.Find(armAxisPath + "/Cylinder.041");

        boomLength = (boomArmJoint.position - bodyBoomJoint.position).magnitude;
        armLength = (armBucketJoint.position - boomArmJoint.position).magnitude;
        armLengthHook = (hookMainAxis.position - boomArmJoint.position).magnitude;
        boomLength2 = Mathf.Pow(boomLength, 2F);
        armLength2 = Mathf.Pow(armLength, 2F);
        armLengthHook2 = Mathf.Pow(armLengthHook, 2F);

        geometry = new Geometry(swingAxis, bodyBoomJoint, boomArmJoint, armBucketJoint, hookMainAxis);
        //Debug.Log($"verticalAngle={geometry.verticalAngle}, boomArmAngle={geometry.boomArmAngle}");
        verticalAngleZero = geometry.verticalAngle;
        boomArmAngleZero = geometry.boomArmAngle;
        boomArmHookAngleZero = geometry.boomArmHookAngle;

        bucket = transform.Find(bucketAxisPath + "/Vert.002").gameObject;
        bucketCuttingEdges = transform.Find(bucketAxisPath + "/Cube.019");
        bucketPosition = transform.Find(armLinkageAxisPath + "/BucketCylinderAxis2/Sphere.007");
        arm = transform.Find(armAxisPath + "/Vert.001").gameObject;
        boom = transform.Find(boomAxisPath + "/Cube.010").gameObject;
        body = transform.Find(swingAxisPath + "/Cube").gameObject;
        cabin = transform.Find(swingAxisPath + "/Vert.009").gameObject;
        leftTrack = transform.Find("TrackLeft").gameObject;
        rightTrack = transform.Find("TrackRight").gameObject;

        bucket.AddComponent<MeshCollider>();
        bucket.GetComponent<MeshCollider>().convex = true;
        arm.AddComponent<MeshCollider>();
        arm.GetComponent<MeshCollider>().convex = true;
        boom.AddComponent<MeshCollider>();
        boom.GetComponent<MeshCollider>().convex = true;
        body.AddComponent<MeshCollider>();
        body.GetComponent<MeshCollider>().convex = true;
        cabin.AddComponent<MeshCollider>();
        cabin.GetComponent<MeshCollider>().convex = true;
        
        leftTrack.AddComponent<BoxCollider>();
        var lt = leftTrack.GetComponent<BoxCollider>().size;
        leftTrack.GetComponent<BoxCollider>().size = new Vector3(lt.x * 4 / 5F, lt.y * 16 / 17F, lt.z);
        rightTrack.AddComponent<BoxCollider>();
        var rt = rightTrack.GetComponent<BoxCollider>().size;
        rightTrack.GetComponent<BoxCollider>().size = new Vector3(rt.x * 4 / 5F, rt.y * 16 / 17F, rt.z);
        
    }

    private void OrientHydraulicCylinder(Transform cylinder1, Transform cylinder1Target,
    Transform cylinder2, Transform cylinder2Target, Vector3 up)
    {
        cylinder1.LookAt(cylinder1Target, up);
        cylinder2.LookAt(cylinder2Target, up);

        cylinder1.Rotate(new Vector3(90F, 0F, 0F));
        cylinder2.Rotate(new Vector3(90F, 0F, 0F));
    }

    private void OrientLinkage(Transform linkage, Transform linkageTarget, Vector3 up)
    {
        linkage.LookAt(linkageTarget, up);
        linkage.Rotate(new Vector3(90F, 0F, 0F));
    }

    private void OrientArmCylinder()
    {
        OrientHydraulicCylinder(armCylinderAxis1, armCylinder1Target, armCylinderAxis2, armCylinder2Target, arm.transform.right);
    }

    private void OrientBoomCylinder()
    {
        OrientHydraulicCylinder(boomCylinderRightAxis1, boomCylinderRight1Target, boomCylinderRightAxis2, boomCylinderRight2Target, arm.transform.right);
        OrientHydraulicCylinder(boomCylinderLeftAxis1, boomCylinderLeft1Target, boomCylinderLeftAxis2, boomCylinderLeft2Target, arm.transform.right);
    }

    private void OrientBucketCylinder()
    {
        OrientLinkage(armLinkageAxis, armLinkageTarget, arm.transform.right);
        OrientLinkage(bucketLinkageAxis, bucketCylinder1Target,  arm.transform.forward);
        OrientHydraulicCylinder(bucketCylinderAxis1, bucketCylinder1Target, bucketCylinderAxis2, bucketCylinder2Target, boom.transform.right);
    }

    public Transform transform
    {
        get { return excavator.transform; }
    }

    public Vector3 position
    {
        set { excavator.transform.position = value; }
        get { return excavator.transform.position; }
    }

    public void swingRotate(float value)
    {
        swingAngle += value;
    }

    public void boomRotate(float value)
    {
        boomAngle += value;
    }

    public void armRotate(float value)
    {
        armAngle -= value;
    }

    public void bucketRotate(float value)
    {
        bucketAngle += value;
    }

    public float swingAngle
    {
        set
        {
            swingAxis.localRotation = swingAxisInitialQ;
            swingAxis.Rotate(0, value, 0);
        }
        get
        {
            float angle = Vector3.SignedAngle(excavatorForwardAxis.right, swingAxis.right, swingAxis.up);
            Debug.Log($"swing angle = {angle}");
            return angle;
        }
    }

    public float boomAngle
    {
        set
        {
            if (value >= 0F && value <= 56F)
            {
                boomAxis.localRotation = boomAxisInitialQ;
                boomAxis.Rotate(value, 0, 0);
                OrientBoomCylinder();
            }
            Debug.Log($"boomAngle: {boomAngle}");
        }
        get
        {
            return Quaternion.Angle(boomAxisInitialQ, boomAxis.localRotation);
        }
    }

    public float armAngle
    {
        set
        {
            if (value >= 0F && value <= 120F)
            {
                armAxis.localRotation = armAxisInitialQ;
                armAxis.Rotate(-value, 0, 0);
                OrientArmCylinder();
            }
            Debug.Log($"armAngle: {armAngle}");
        }
        get
        {
            return Quaternion.Angle(armAxisInitialQ, armAxis.localRotation);
        }
    }

    public float bucketAngle
    {
        set
        {
            if (value >= 0F && value <= 160F)
            {
                bucketAxis.localRotation = bucketAxisInitialQ;
                bucketAxis.Rotate(value, 0, 0);
                OrientBucketCylinder();
            }
            Debug.Log($"bucketAngle: {bucketAngle}");
        }
        get
        {
            return Quaternion.Angle(bucketAxisInitialQ, bucketAxis.localRotation);
        }
    }


    public void leftOperationLeverRotate(float leftRight, float upDown)
    {
        leftOperationLeverAxis.Rotate(new Vector3(leftRight, 0, upDown));
    }

    public void rightOperationLeverRotate(float leftRight, float upDown)
    {
        rightOperationLeverAxis.Rotate(new Vector3(leftRight, 0, upDown));
    }

    public void leftTravelLeverRotate(float upDown)
    {
        leftTravelLeverAxis.Rotate(new Vector3(upDown, 0, 0));
    }

    public void rightTravelLeverRotate(float upDown)
    {
        rightTravelLeverAxis.Rotate(new Vector3(upDown, 0, 0));
    }

    public void rightPedalRotate(float upDown)
    {
        rightPedalAxis.Rotate(new Vector3(upDown, 0, 0));

    }

    public void leftPedalRotate(float upDown)
    {
        leftPedalAxis.Rotate(new Vector3(upDown, 0, 0));

    }

    float accel = 0;

    private bool Move(Transform target, DriveParams driveParams)
    {

        float up = Mathf.Abs(Vector3.Angle(excavatorForwardAxis.rotation * Vector3.up, Vector3.up));
        if (up < 40F || up >= 140F) throw new Exception();

        //Debug.Log($"up = {Vector3.Angle(excavatorForwardAxis.rotation*Vector3.up, Vector3.up)}");

        Rigidbody rb = transform.GetComponent<Rigidbody>();

        Vector3 directionToTarget = target.position - transform.position;
        Vector3 forwardDirection = excavatorForwardAxis.rotation * Vector3.right;
        float distance = new Vector2(directionToTarget.x, directionToTarget.z).magnitude;
        directionToTarget.y = 0F;
        forwardDirection.y = 0;

        if (distance > 7F || distance <= 5F)
        {
            float angle = Vector3.SignedAngle(forwardDirection, directionToTarget, Vector3.up);
            //Debug.Log(angle);
            if (Mathf.Abs(angle) > driveParams.deltaSwing)
            {
                if (angle > 0)
                {
                    transform.Rotate(0, driveParams.deltaSwing, 0);
                } else
                {
                    transform.Rotate(0, -driveParams.deltaSwing, 0);
                }
            }
            forwardDirection = (excavatorForwardAxis.rotation * Vector3.right).normalized;
            if (distance <= 5F)  // reverse traveling range
            {
                forwardDirection = -forwardDirection;
            }
            float maxSpeed = driveParams.maxSpeed;
            if (distance < 15F) maxSpeed = driveParams.creepSpeed;  // slow down range
            if (rb.velocity.magnitude < maxSpeed && this.accel + driveParams.deltaAccel < driveParams.maxAccel)
            {
                this.accel += driveParams.deltaAccel;
            }
            else
            {
                this.accel -= driveParams.deltaAccel;
            }

            float force = driveParams.mass * this.accel;  // F = m * a
            rb.AddForceAtPosition(forwardDirection * force, transform.position, ForceMode.Force);
            Debug.Log($"Force: {driveParams.mass * this.accel}");

            return true;
        }

        if (rb.velocity.magnitude < 0.01F)
        {
            accel = 0F;
            return false;
        }
        else return true;
    }

    private RaycastHit raycastHit;
    private RaycastHit hit;
    private bool _useHook = false;

    public void OrientHook()
    {
        if (_useHook) {
            Physics.Raycast(hookMainAxis.position, Vector3.down, out hit, 200F);
            hookMainAxis.LookAt(hit.point, hookMainAxis.up);
            hookMainAxis.Rotate(new Vector3(0F, 210F, 0F));
        }
    }

    public void EnableCuttingEdges(bool enable)
    {
        bucket.GetComponent<MeshCollider>().enabled = !enable;
    }

    public bool useHook
    {
        set
        {
            _useHook = true;
        }
        get
        {
            return _useHook;
        }
    }


    /**
     * Inverse Kinematic for swing, boom and arm.
     */
    public Position IK(Vector3 target)
    {

        Vector3 directionToTarget = target - swingAxis.position;
        Vector3 forwardDirection = excavatorForwardAxis.right;
        float D = Vector2.SignedAngle(new Vector2(directionToTarget.x, directionToTarget.z), new Vector2(forwardDirection.x, forwardDirection.z));

        float R = Mathf.Sqrt(Mathf.Pow(target.x - swingAxis.position.x, 2f) + Mathf.Pow(target.z - swingAxis.position.z,  2f));

        float E = Mathf.Atan2(R, (boomAxis.position.y - target.y)) * Mathf.Rad2Deg;
        float S = Mathf.Sqrt(Mathf.Pow(R, 2f) + Mathf.Pow(boomAxis.position.y - target.y, 2F));
        float S2 = Mathf.Pow(S, 2F);

        float A;
        float B;
        float C;
        if (_useHook)
        {
            A = Mathf.Acos((boomLength2 + S2 - armLengthHook2) / (2F * boomLength * S)) * Mathf.Rad2Deg;
            B = Mathf.Acos((armLengthHook2 + boomLength2 - S2) / (2F * boomLength * armLengthHook)) * Mathf.Rad2Deg;
            C = Mathf.Acos((armLengthHook2 + S2 - boomLength2) / (2F * armLengthHook * S)) * Mathf.Rad2Deg;
        }
        else
        {
            A = Mathf.Acos((boomLength2 + S2 - armLength2) / (2F * boomLength * S)) * Mathf.Rad2Deg;
            B = Mathf.Acos((armLength2 + boomLength2 - S2) / (2F * boomLength * armLength)) * Mathf.Rad2Deg;
            C = Mathf.Acos((armLength2 + S2 - boomLength2) / (2F * armLength * S)) * Mathf.Rad2Deg;
        }

        //Debug.Log($"boomLength={boomLength}, armLength={armLength}");

        //Debug.Log($"target.x={target.x}, turnAxis.position.x={swingAxis.position.x}, target.z={target.z}, turnAxis.position.z={swingAxis.position.z}");

        //Debug.Log($"A={A}, B={B}, C={C}, D={D}, E={E}, R={R}");

        Position p = new Position();
        p.R = R;
        p.D = D;
        p.E = E;
        p.S = S;
        p.A = A;
        p.B = B;
        p.C = C;

        p.verticalAngleZero = verticalAngleZero;
        p.boomArmAngleZero = boomArmAngleZero;
        p.boomArmHookAngleZero = boomArmHookAngleZero;

        return p;


    }

    private bool coroutineIsRunning = false;

    public IEnumerator Reset(float targetSwingAngle=0F, float targetBoomAngle=55F, float targetArmAngle=45F, float targetBucketAngle=60F)
    {
        float t = 0F;
        float currentSwingAngle = swingAngle;
        float currentBoomAngle = boomAngle;
        float currentArmAngle = armAngle;
        float currentBucketAngle = bucketAngle;

        while(coroutineIsRunning)
        {
            yield return null;
        }

        coroutineIsRunning = true;

        while (t < 1F)
        {
            t += Time.deltaTime * 0.8F;
            swingAngle = Mathf.Lerp(currentSwingAngle, targetSwingAngle, t);
            boomAngle = Mathf.Lerp(currentBoomAngle, targetBoomAngle, t);
            armAngle = Mathf.Lerp(currentArmAngle, targetArmAngle, t);
            bucketAngle = Mathf.Lerp(currentBucketAngle, targetBucketAngle, t);
            yield return null;
        }

        coroutineIsRunning = false;
    }

    public IEnumerator MoveToTarget(GameObject target, DriveParams driveParams, float startBucketAngle, float finishBucketAngle)
    {

        while (coroutineIsRunning)
        {
            yield return null;
        }

        coroutineIsRunning = true;
        bool emergency = false;

        float t = 0;

        float currentBucketAngle = bucketAngle;

        while (t < 1F)
        {
            t += Time.deltaTime * 0.7F;
            bucketAngle = Mathf.Lerp(currentBucketAngle, startBucketAngle, t);
            yield return null;
        }

        accel = driveParams.initialAccel;
        while (true)
        {
            try
            {
                bool cont = Move(target.transform, driveParams);
                if (!cont) break;
            }
            catch (Exception e)
            {
                emergency = true;
            }
            yield return null;
        }

        if (!emergency)
        {
            Position p = IK(target.transform.position);


            float Adash = p.A + p.E + p.verticalAngleZero - 180F;
            float Bdash;
            if (_useHook)
            {
                Bdash = p.B - p.boomArmHookAngleZero;
            }
            else
            {
                Bdash = p.B - p.boomArmAngleZero;
            }


            t = 0F;

            float currentSwingAngle = swingAngle;
            float currentBoomAngle = boomAngle;
            float currentArmAngle = armAngle;

            while (t < 1F)
            {
                t += Time.deltaTime * 0.3F;
                swingAngle = Mathf.Lerp(currentSwingAngle, p.D, t);
                boomAngle = Mathf.Lerp(currentBoomAngle, Adash, t);
                armAngle = Mathf.Lerp(currentArmAngle, Bdash, t);
                yield return null;
            }

            currentBucketAngle = bucketAngle;

            t = 0;
            while (t < 1F)
            {
                t += Time.deltaTime * 0.7F;
                bucketAngle = Mathf.Lerp(currentBucketAngle, finishBucketAngle, t);
                yield return null;
            }
        }

        coroutineIsRunning = false;

    }
}
