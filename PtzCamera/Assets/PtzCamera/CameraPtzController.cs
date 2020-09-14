using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraPtzController : MonoBehaviour, IPointerDownHandler
{

    enum Direction
    {
        PAN_LEFT, PAN_RIGHT, TILT_UP, TILT_DONW, ZOOM_IN, ZOOM_OUT
    };

    Transform camera;
    Transform panAxis;
    Transform tiltAxis;

    LineRenderer lineRederer;
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
        camera = transform.Find("Armature/PanAxis/TiltAxis/Camera");
        panAxis = transform.Find("Armature/PanAxis");
        tiltAxis = transform.Find("Armature/PanAxis/TiltAxis");

        camera.gameObject.AddComponent<LineRenderer>();
        lineRederer = camera.gameObject.GetComponent<LineRenderer>();
        lineRederer.SetPosition(0, camera.position);
        lineRederer.startWidth = raycastWidth;

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
        var timeDelta = Time.deltaTime;

        if (enableLazer)
        {
            lineRederer.SetPosition(1, camera.forward * raycastDistance + camera.position);
        } else
        {
            lineRederer.SetPosition(1, camera.forward * 0F + camera.position);
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

        if (Input.GetKey(KeyCode.A))  // Pan left
        {
            OrientCamera(Direction.PAN_LEFT, timeDelta);
        } else if (Input.GetKey(KeyCode.D))  // Pan right
        {
            OrientCamera(Direction.PAN_RIGHT, timeDelta);
        }
        else if (Input.GetKey(KeyCode.W))  // Tile up
        {
            OrientCamera(Direction.TILT_UP, timeDelta);
        } else if (Input.GetKey(KeyCode.Z))  // Tild down
        {
            OrientCamera(Direction.TILT_DONW, timeDelta);
        }
        else if (Input.GetKey(KeyCode.E))  // Zoom in
        {
            OrientCamera(Direction.ZOOM_IN, timeDelta);
        } else if (Input.GetKey(KeyCode.X))  // Zoom out
        {
            OrientCamera(Direction.ZOOM_OUT, timeDelta);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Button button = eventData.
    }
}
