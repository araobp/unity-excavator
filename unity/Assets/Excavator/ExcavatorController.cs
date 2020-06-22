using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LeverAngles
{
    public float upDown;
    public float leftRight;

    public LeverAngles(float upDown, float leftRight)
    {
        this.upDown = upDown;
        this.leftRight = leftRight;
    }

    public bool isOperated()
    {
        if (this.upDown != 0 || this.leftRight != 0) return true;
        else return false;
    }

    public void clear()
    {
        this.upDown = 0F;
        this.leftRight = 0F;
    }
}


public class InputEvents
{
    private float _swing;
    private float _boom;
    private float _arm;
    private float _bucket;
    private float _trackRight;
    private float _trackLeft;

    private bool _updated;

    public void clear()
    {
        _swing = 0F;
        _boom = 0F;
        _arm = 0F;
        _bucket = 0F;
        _trackRight = 0F;
        _trackLeft = 0F;

        _updated = false;
    }

    public bool isUpdated
    {
        get { return _updated; }
    }

    public float swing
    {
        set { _swing = value; _updated = true; }
        get { return _swing; }
    }

    public float boom
    {
        set { _boom = value; _updated = true; }
        get { return _boom; }
    }

    public float arm
    {
        set { _arm = value; _updated = true; }
        get { return _arm; }
    }

    public float bucket
    {
        set { _bucket = value; _updated = true; }
        get { return _bucket; }
    }

    public float trackRight
    {
        set { _trackRight = value; _updated = true; }
        get { return _trackRight; }
    }

    public float trackLeft
    {
        set { _trackLeft = value; _updated = true; }
        get { return _trackLeft; }
    }

}


public class ExcavatorController : MonoBehaviour
{
    public float maxSpeed = 3F;
    public float creepSpeed = 1F;
    public float initialAccel = 30F;
    public float deltaAccel = 0.5F;
    public float maxAccel = 55F;
    public float deltaSwing = 2F;

    public bool useHook = false;

    private DriveParams driveParams;

    private InputEvents inputEvents = new InputEvents();

    private Excavator excavator;

    bool travel = false;
    Image travelIndicator;
    Image operationIndicator;

    private void ProcessGamepadEvents(InputEvents inputEvents)
    {

        bool modeChange = Input.GetKeyUp(KeyCode.Joystick1Button1);
        if (modeChange)
        {
            travel = !travel;
            float travelGreen = 0F;
            float operationGreen = 0F;
            if (travel) travelGreen = 1F;  else operationGreen = 1F;
            travelIndicator.color = new Color(0, travelGreen, 0);
            operationIndicator.color = new Color(0, operationGreen, 0);
        }

        if (!travel)
        {
            float joystickLeftX = Input.GetAxis("JoystickLeftX");
            if (joystickLeftX != 0)
            {
                inputEvents.swing = joystickLeftX;
            }

            float joystickLeftY = Input.GetAxis("JoystickLeftY");
            if (joystickLeftY != 0)
            {
                inputEvents.arm = -joystickLeftY;
            }

            float joystickRightX = Input.GetAxis("JoystickRightX");
            if (joystickRightX != 0)
            {
                inputEvents.bucket = joystickRightX;
            }

            float joystickRightY = Input.GetAxis("JoystickRightY");
            if (joystickRightY != 0)
            {
                inputEvents.boom = -joystickRightY;
            }

        } else

        {
            float joystickLeftY = Input.GetAxis("JoystickLeftY");
            if (joystickLeftY != 0)
            {
                inputEvents.trackLeft = -joystickLeftY;
            }

            float joystickRightY = Input.GetAxis("JoystickRightY");
            if (joystickRightY != 0)
            {
                inputEvents.trackRight = -joystickRightY;
            }
        }

    }

    private void ProcessKeyEvents(InputEvents inputEvents)
    {
        // Swing
        if (Input.GetKey(KeyCode.H)) { inputEvents.swing = 1F; }  // Swing right
        else if (Input.GetKey(KeyCode.F)) { inputEvents.swing = -1F; }  // Swing left

        // Arm
        if (Input.GetKey(KeyCode.T)) { inputEvents.arm = 1F; }  // Arm roll out
        else if (Input.GetKey(KeyCode.G)) { inputEvents.arm = -1F; }  // Arm rool in

        // Boom
        if (Input.GetKey(KeyCode.I)) { inputEvents.boom = 1F; }  // Boom roll in
        else if (Input.GetKey(KeyCode.K)) { inputEvents.boom = -1F; }  // Boom roll out

        // Bucket
        if (Input.GetKey(KeyCode.L)) { inputEvents.bucket = 1F; }  // Bucket roll out
        else if (Input.GetKey(KeyCode.J)) { inputEvents.bucket = -1F; }  // Bucket roll in

        // Track
        if (Input.GetKey(KeyCode.U)) { inputEvents.trackRight = 1F; }  // Track right
        else if (Input.GetKey(KeyCode.O)) { inputEvents.trackRight = -1F; }
        if (Input.GetKey(KeyCode.Y)) { inputEvents.trackLeft = 1F; }  // Track left
        else if (Input.GetKey(KeyCode.R)) { inputEvents.trackLeft = -1F; }
    }

    // Start is called before the first frame update
    void Start()
    {
        excavator = new Excavator(transform.root.gameObject);

        travelIndicator = GameObject.FindWithTag("TravelIndicator").GetComponent<Image>();
        operationIndicator = GameObject.FindWithTag("OperationIndicator").GetComponent<Image>();
        travelIndicator.color = new Color(0, 0, 0);
        operationIndicator.color = new Color(0, 1F, 0);
        float mass = gameObject.GetComponent<Rigidbody>().mass;
        driveParams = new DriveParams(mass, maxSpeed, creepSpeed, initialAccel, deltaAccel, maxAccel, deltaSwing);

        excavator.useHook = useHook;
    }

    private LeverAngles rightOperationLeverAngles = new LeverAngles(0F, 0F);
    private LeverAngles leftOperationLeverAngles = new LeverAngles(0F, 0F);
    private LeverAngles rightTravelLeverAngles = new LeverAngles(0F, 0F);
    private LeverAngles leftTravelLeverAngles = new LeverAngles(0F, 0F);

    // Update is called once per frame
    void Update()
    {
        excavator.OrientHook();  // TODO: add hook operations

        //--- Code for autonomous operations from this line ---
        if (Input.GetKey(KeyCode.Alpha1))
        {
            excavator.EnableCuttingEdges(false);
            StartCoroutine(excavator.Reset());
            GameObject target = GameObject.FindWithTag("Target1");
            StartCoroutine(excavator.MoveToTarget(target, driveParams, 40F, 40F));
        }
        if (Input.GetKey(KeyCode.Alpha2)) {
            excavator.EnableCuttingEdges(false);
            StartCoroutine(excavator.Reset());
            GameObject target2 = GameObject.FindWithTag("Target2");
            StartCoroutine(excavator.MoveToTarget(target2, driveParams, 40F, 40F));
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            excavator.EnableCuttingEdges(true);
            StartCoroutine(excavator.Reset());
            GameObject target3 = GameObject.FindWithTag("Target3");
            StartCoroutine(excavator.MoveToTarget(target3, driveParams, 40F, 40F));
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            excavator.EnableCuttingEdges(false);
            StartCoroutine(excavator.Reset());
            GameObject target4 = GameObject.FindWithTag("Target4");
            StartCoroutine(excavator.MoveToTarget(target4, driveParams, 50F, 50F));
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            excavator.EnableCuttingEdges(true);
            StartCoroutine(excavator.Reset());
            GameObject target5 = GameObject.FindWithTag("Target5");
            StartCoroutine(excavator.MoveToTarget(target5, driveParams, 140F, 40F));
        }
        if (Input.GetKey(KeyCode.Alpha6))
        {
            excavator.EnableCuttingEdges(true);
            GameObject target6 = GameObject.FindWithTag("Target6");
            StartCoroutine(excavator.MoveToTarget(target6, driveParams, 40F, 140F));
        }

        //--- Code for manual operations from this line ---
        var delta = Time.deltaTime * 30F;

        if (leftOperationLeverAngles.isOperated() || rightOperationLeverAngles.isOperated())
        {
            excavator.leftOperationLeverRotate(-leftOperationLeverAngles.leftRight, -leftOperationLeverAngles.upDown);
            excavator.rightOperationLeverRotate(-rightOperationLeverAngles.leftRight, -rightOperationLeverAngles.upDown);

            leftOperationLeverAngles.clear();
            rightOperationLeverAngles.clear();
        }


        if (leftTravelLeverAngles.isOperated() || rightTravelLeverAngles.isOperated())
        {
            excavator.leftTravelLeverRotate(-leftTravelLeverAngles.upDown);
            excavator.rightTravelLeverRotate(-rightTravelLeverAngles.upDown);

            excavator.rightPedalRotate(rightTravelLeverAngles.upDown*2F);
            excavator.leftPedalRotate(leftTravelLeverAngles.upDown*2F);

            leftTravelLeverAngles.clear();
            rightTravelLeverAngles.clear();

        }

        ProcessKeyEvents(inputEvents);
        ProcessGamepadEvents(inputEvents);

        // Control
        if (inputEvents.isUpdated)
        {

            /* Swing */
            if (inputEvents.swing != 0F)
            {
                Debug.Log(excavator.swingAngle);
                excavator.swingRotate(delta * 1F * inputEvents.swing);
                leftOperationLeverAngles.leftRight = 5F * inputEvents.swing;
            }

            /* Arm */
            if (inputEvents.arm != 0F)
            {
                excavator.armRotate(-delta * 1F * inputEvents.arm);
                leftOperationLeverAngles.upDown = 5F * inputEvents.arm;
            }

            /* Boom */
            if (inputEvents.boom != 0F)
            {
                excavator.boomRotate(-delta * 0.6F * inputEvents.boom);
                rightOperationLeverAngles.upDown = 5F * inputEvents.boom;
            }

            /* Bucket */
            if (inputEvents.bucket != 0F)
            {
                excavator.bucketRotate(delta * 1.5F * inputEvents.bucket);
                rightOperationLeverAngles.leftRight = 5F * inputEvents.bucket;
            }

            /* Track right */
            if (inputEvents.trackRight != 0F)
            {
                excavator.transform.Rotate(new Vector3(0, -delta * 1.2F * inputEvents.trackRight, 0));
                excavator.transform.Translate(new Vector3(delta * 0.04F * inputEvents.trackRight, 0, 0));
                rightTravelLeverAngles.upDown = 5F * inputEvents.trackRight;
            }

            /* Track left */
            if (inputEvents.trackLeft != 0F)
            {
                excavator.transform.Rotate(new Vector3(0, delta * 1.2F * inputEvents.trackLeft, 0));
                excavator.transform.Translate(new Vector3(delta * 0.04F * inputEvents.trackLeft, 0, 0));
                leftTravelLeverAngles.upDown = 5F * inputEvents.trackLeft;
            }

            inputEvents.clear();
        }

        if (rightOperationLeverAngles.isOperated() || leftOperationLeverAngles.isOperated())
        {
            excavator.rightOperationLeverRotate(rightOperationLeverAngles.leftRight, rightOperationLeverAngles.upDown);
            excavator.leftOperationLeverRotate(leftOperationLeverAngles.leftRight, leftOperationLeverAngles.upDown);
        }

        if (rightTravelLeverAngles.isOperated() || leftTravelLeverAngles.isOperated())
        {
            excavator.rightTravelLeverRotate(rightTravelLeverAngles.upDown);
            excavator.leftTravelLeverRotate(leftTravelLeverAngles.upDown);

            excavator.rightPedalRotate(-rightTravelLeverAngles.upDown*2F);
            excavator.leftPedalRotate(-leftTravelLeverAngles.upDown*2F);
        }

    }

}
