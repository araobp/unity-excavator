using UnityEngine;
using UnityEngine.InputSystem;

public class DroneManager : MonoBehaviour
{
    [SerializeField]
    GameObject m_CameraObject;

    [SerializeField]
    StickController m_StickControllerLeft;

    [SerializeField]
    StickController m_StickControllerRight;

    [SerializeField]
    float m_MultiplierTranslate = 1F;

    [SerializeField]
    float m_MultiplierRotate = 1F;

    float m_RotationY = 0F;

    private void OnEnable()
    {
        InputSystem.EnableDevice(AttitudeSensor.current);
    }

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;

        // Stick Control
        float deltaYLeft = m_StickControllerLeft.deltaY * deltaTime * m_MultiplierTranslate;
        float deltaXRight = m_StickControllerRight.deltaX * deltaTime * m_MultiplierRotate;
        float deltaYRight = m_StickControllerRight.deltaY * deltaTime * m_MultiplierTranslate;
        Transform cam = m_CameraObject.transform;
        cam.Translate(new Vector3(20F * deltaXRight, 20F * deltaYLeft, 40F * deltaYRight));

        float deltaXLeft = m_StickControllerLeft.deltaX * deltaTime * m_MultiplierTranslate;
        m_RotationY += 100F * deltaXLeft;

        // Camera attitude Control
        Quaternion attitude = AttitudeSensor.current.attitude.ReadValue();
        attitude = Quaternion.Euler(0, 0, -180) * Quaternion.Euler(-90, 0, 0) * attitude * Quaternion.Euler(0, 0, 180);
        cam.transform.rotation = attitude;// * Quaternion.AngleAxis(m_RotationY, Vector3.up);

        // Rotate the camera around Y-axis in world splace
        cam.Rotate(new Vector3(0F, m_RotationY, 0F), Space.World);
    }
}
