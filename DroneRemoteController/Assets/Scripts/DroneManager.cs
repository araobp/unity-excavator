using UnityEngine;
using UnityEngine.InputSystem;

public class DroneManager : MonoBehaviour
{
    [SerializeField]
    GameObject _CameraObject;

    [SerializeField]
    StickController _StickControllerLeft;

    [SerializeField]
    StickController _StickControllerRight;

    [SerializeField]
    float _MultiplierTranslate = 1F;

    [SerializeField]
    float _MultiplierRotate = 1F;

    float _RotationY = 0F;

    float _SimulatedPitch = 0F;
    float _SimulatedRoll = 0F;

    private void OnEnable()
    {
        // Enable the AttitudeSensor device when the script is enabled
        if (AttitudeSensor.current != null)
        {
            InputSystem.EnableDevice(AttitudeSensor.current);
        }
    }

    bool _HasLoggedNullControllersWarning = false;

    // Start is called before the first frame update
    void Start()
    {
        // Allow auto-rotation between landscape orientations to fit different device screen cutouts (like Pixel 9a)
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        // Attempt to auto-find stick controllers if they are not assigned in the Inspector
        if (_StickControllerLeft == null || _StickControllerRight == null)
        {
            StickController[] controllers = FindObjectsOfType<StickController>();
            foreach (var controller in controllers)
            {
                if (controller.gameObject.name.ToLower().Contains("left"))
                {
                    if (_StickControllerLeft == null) _StickControllerLeft = controller;
                }
                else if (controller.gameObject.name.ToLower().Contains("right"))
                {
                    if (_StickControllerRight == null) _StickControllerRight = controller;
                }
            }
        }
        // Attempt to auto-find camera if not assigned
        if (_CameraObject == null)
        {
            if (Camera.main != null)
            {
                _CameraObject = Camera.main.gameObject;
            }
            else
            {
                _CameraObject = GameObject.FindWithTag("MainCamera");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Ensure stick controllers are assigned before processing input
        if (_StickControllerLeft == null || _StickControllerRight == null)
        {
            if (!_HasLoggedNullControllersWarning)
            {
                Debug.LogWarning("DroneManager: Stick Controllers are not assigned! Please assign them in the Inspector.");
                _HasLoggedNullControllersWarning = true;
            }
            return;
        }

        // Ensure camera object is assigned before processing camera movement
        if (_CameraObject == null)
        {
            if (Camera.main != null)
            {
                _CameraObject = Camera.main.gameObject;
            }
            else
            {
                _CameraObject = GameObject.FindWithTag("MainCamera");
            }

            if (_CameraObject == null)
            {
                return;
            }
        }

        // Get the delta time for smooth movement
        float deltaTime = Time.deltaTime;

        // Calculate the translation and rotation values based on the stick input and multipliers
        float deltaYLeft = _StickControllerLeft.deltaY * deltaTime * _MultiplierTranslate;
        float deltaXRight = _StickControllerRight.deltaX * deltaTime * _MultiplierRotate;
        float deltaYRight = _StickControllerRight.deltaY * deltaTime * _MultiplierTranslate;

        Transform cam = _CameraObject.transform;
        // Translate the camera position based on the stick input values
        cam.Translate(new Vector3(20F * deltaXRight, 20F * deltaYLeft, 40F * deltaYRight));

        // Rotate the camera around the Y-axis based on the left stick input
        float deltaXLeft = _StickControllerLeft.deltaX * deltaTime * _MultiplierTranslate;
        _RotationY += 100F * deltaXLeft;

        // Get the attitude from the AttitudeSensor and apply it to the camera's rotation
        Quaternion attitude;
        if (AttitudeSensor.current != null)
        {
            // Read rotation directly from the device's physical attitude sensor
            attitude = AttitudeSensor.current.attitude.ReadValue();
            attitude = Quaternion.Euler(0, 0, -180) * Quaternion.Euler(-90, 0, 0) * attitude * Quaternion.Euler(0, 0, 180);
        }
        else
        {
            // Fallback: Simulate device tilt (pitch and roll) for editor / sensorless devices
            float pitchInput = 0f;
            float rollInput = 0f;

            if (Keyboard.current != null)
            {
                // Use T/G keys to tilt forward/backward (Pitch), and F/H to tilt left/right (Roll)
                if (Keyboard.current.tKey.isPressed) pitchInput = 1f;
                if (Keyboard.current.gKey.isPressed) pitchInput = -1f;
                if (Keyboard.current.fKey.isPressed) rollInput = -1f;
                if (Keyboard.current.hKey.isPressed) rollInput = 1f;
            }

            // Mouse Right-Click Dragging fallback: dragging simulates tilting the controller
            bool isMouseDragging = Mouse.current != null && Mouse.current.rightButton.isPressed;
            if (isMouseDragging)
            {
                Vector2 mouseDelta = Mouse.current.delta.ReadValue();
                _SimulatedPitch -= mouseDelta.y * 0.2f; // Invert y-axis to match pitch pull-back feeling
                _SimulatedRoll += mouseDelta.x * 0.2f;
                _SimulatedPitch = Mathf.Clamp(_SimulatedPitch, -80f, 80f);
                _SimulatedRoll = Mathf.Clamp(_SimulatedRoll, -80f, 80f);
            }
            else
            {
                // Keyboard Tilt Simulation
                if (pitchInput != 0f)
                {
                    _SimulatedPitch += pitchInput * 50f * deltaTime;
                    _SimulatedPitch = Mathf.Clamp(_SimulatedPitch, -80f, 80f);
                }
                else
                {
                    // Auto-center Pitch smoothly when there is no input
                    _SimulatedPitch = Mathf.MoveTowards(_SimulatedPitch, 0f, 30f * deltaTime);
                }

                if (rollInput != 0f)
                {
                    _SimulatedRoll += rollInput * 50f * deltaTime;
                    _SimulatedRoll = Mathf.Clamp(_SimulatedRoll, -80f, 80f);
                }
                else
                {
                    // Auto-center Roll smoothly when there is no input
                    _SimulatedRoll = Mathf.MoveTowards(_SimulatedRoll, 0f, 30f * deltaTime);
                }
            }

            // Construct attitude quaternion based on simulated pitch and roll angles
            attitude = Quaternion.Euler(_SimulatedPitch, 0, _SimulatedRoll);
        }

        cam.transform.rotation = attitude;// * Quaternion.AngleAxis(m_RotationY, Vector3.up);

        // Rotate the camera around Y-axis in world splace based on the accumulated rotation value
        cam.Rotate(new Vector3(0F, _RotationY, 0F), Space.World);
    }
}
