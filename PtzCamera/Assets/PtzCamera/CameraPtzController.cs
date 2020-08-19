using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraPtzController : MonoBehaviour
{

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

        if (Input.GetKey(KeyCode.D))  // Pan left
        {
            panAxis.Rotate(0, -2F * timeDelta, 0);
        } else if (Input.GetKey(KeyCode.A))  // Pan right
        {
            panAxis.Rotate(0, 2F * timeDelta, 0);
        }
        else if (Input.GetKey(KeyCode.W))  // Tile up
        {
            tiltAxis.Rotate(2F * timeDelta, 0, 0);
        } else if (Input.GetKey(KeyCode.Z))  // Tild down
        {
            tiltAxis.Rotate(-2F * timeDelta, 0, 0);
        }
        else if (Input.GetKey(KeyCode.E))  // Zoom in
        {
            camera.gameObject.GetComponent<Camera>().fieldOfView -= 4F * timeDelta;
        } else if (Input.GetKey(KeyCode.X))  // Zoom out
        {
            camera.gameObject.GetComponent<Camera>().fieldOfView += 4F * timeDelta;
        }
    }
}
