using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class CameraPtzController : MonoBehaviour
{

    enum Direction
    {
        PAN_LEFT, PAN_RIGHT, TILT_UP, TILT_DONW, ZOOM_IN, ZOOM_OUT
    };

    Transform camera;
    Transform panAxis;
    Transform tiltAxis;

    LineRenderer lineRenderer;
    RaycastHit raycastHit;

    public float raycastDistance = 100F;
    public float raycastWidth = 0.03F;
    public bool enableLazer = true;

    Text textRaycastLocation;
    Text textRaycastDistance;

    Button buttonPanLeft;
    Button buttonPanRight;
    Button buttonTiltUp;
    Button buttonTiltDown;
    Button buttonZoomIn;
    Button buttonZoomOut;

    float roundTo1st(float v)
    {
        return Mathf.RoundToInt(v * 10F) / 10F;
    }

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        camera = transform.Find("Armature/PanAxis/TiltAxis/Camera");
        panAxis = transform.Find("Armature/PanAxis");
        tiltAxis = transform.Find("Armature/PanAxis/TiltAxis");

        camera.gameObject.AddComponent<LineRenderer>();
        lineRenderer = camera.gameObject.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, camera.position);
        lineRenderer.startWidth = raycastWidth;
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
        lineRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));

        textRaycastLocation = GameObject.FindWithTag("raycastLocation").GetComponent<Text>();
        textRaycastDistance = GameObject.FindWithTag("raycastDistance").GetComponent<Text>();

        buttonPanLeft = GameObject.FindWithTag("buttonPanLeft").GetComponent<Button>();
        buttonPanRight = GameObject.FindWithTag("buttonPanRight").GetComponent<Button>();
        buttonTiltUp = GameObject.FindWithTag("buttonTiltUp").GetComponent<Button>();
        buttonTiltDown = GameObject.FindWithTag("buttonTiltDown").GetComponent<Button>();
        buttonZoomIn = GameObject.FindWithTag("buttonZoomIn").GetComponent<Button>();
        buttonZoomOut = GameObject.FindWithTag("buttonZoomOut").GetComponent<Button>();

        float timeDelta = 1F;

        buttonPanLeft.onClick.AddListener(delegate { OrientCamera(Direction.PAN_LEFT, timeDelta); });
        buttonPanRight.onClick.AddListener(delegate { OrientCamera(Direction.PAN_RIGHT, timeDelta); });
        buttonTiltUp.onClick.AddListener(delegate { OrientCamera(Direction.TILT_UP, timeDelta); });
        buttonTiltDown.onClick.AddListener(delegate { OrientCamera(Direction.TILT_DONW, timeDelta); });
        buttonZoomIn.onClick.AddListener(delegate { OrientCamera(Direction.ZOOM_IN, timeDelta); });
        buttonZoomOut.onClick.AddListener(delegate { OrientCamera(Direction.ZOOM_OUT, timeDelta); });
    }

    void OrientCamera(Direction direction, float timeDelta)
    {

        switch (direction)
        {
            case Direction.PAN_LEFT:
                panAxis.Rotate(0, 2F * timeDelta, 0);
                break;
            case Direction.PAN_RIGHT:
                panAxis.Rotate(0, -2F * timeDelta, 0);
                break;
            case Direction.TILT_UP:
                tiltAxis.Rotate(2F * timeDelta, 0, 0);
                break;
            case Direction.TILT_DONW:
                tiltAxis.Rotate(-2F * timeDelta, 0, 0);
                break;
            case Direction.ZOOM_IN:
                camera.gameObject.GetComponent<Camera>().fieldOfView -= 4F * timeDelta;
                break;
            case Direction.ZOOM_OUT:
                camera.gameObject.GetComponent<Camera>().fieldOfView += 4F * timeDelta;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var timeDelta3 = Time.deltaTime * 3;

        if (enableLazer)
        {
            lineRenderer.SetPosition(1, camera.forward * raycastDistance + camera.position);
        } else
        {
            lineRenderer.SetPosition(1, camera.forward * 0F + camera.position);
        }

        bool hit = Physics.Raycast(camera.position, camera.forward * raycastDistance, out raycastHit, raycastDistance);

        if (hit)
        {
            var position = raycastHit.point;
            var distance = raycastHit.distance;
            Debug.Log($"Hit position: {position}");
            textRaycastLocation.text = $"Location: {roundTo1st(position.x)}, {roundTo1st(position.y)}, {roundTo1st(position.z)}";
            textRaycastDistance.text = $"Distance: {roundTo1st(distance)} meters";
        }

        var keyboard = Keyboard.current;
        if (keyboard.aKey.isPressed)  // Pan left
        {
            OrientCamera(Direction.PAN_LEFT, timeDelta3);
        } else if (keyboard.dKey.isPressed)  // Pan right
        {
            OrientCamera(Direction.PAN_RIGHT, timeDelta3);
        }
        else if (keyboard.wKey.isPressed)  // Tile up
        {
            OrientCamera(Direction.TILT_UP, timeDelta3);
        } else if (keyboard.sKey.isPressed)  // Tild down
        {
            OrientCamera(Direction.TILT_DONW, timeDelta3);
        }
        if (keyboard.eKey.isPressed)  // Zoom in
        {
            OrientCamera(Direction.ZOOM_IN, timeDelta3);
        } else if (keyboard.xKey.isPressed)  // Zoom out
        {
            OrientCamera(Direction.ZOOM_OUT, timeDelta3);
        }
    }

}
