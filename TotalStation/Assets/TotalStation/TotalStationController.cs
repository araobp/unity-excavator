using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalStationController : MonoBehaviour
{
    public GameObject prism1;
    public GameObject prism2;
    public GameObject prism3;
    public GameObject prism4;
    public GameObject prism5;
    public GameObject prism6;
    public GameObject prism7;
    public GameObject prism8;


    public float legAngle = 30F;
    public float legExtension = 0.6F;

    // Theodolite and tripod
    Transform horizontalAxis;
    Transform verticalAxis;
    Transform legAngleJointAxis1;
    Transform legAngleJointAxis2;
    Transform legAngleJointAxis3;
    Transform legLock1;
    Transform legLock2;
    Transform legLock3;
    Transform body;

    Camera telescope;

    GameObject[] prisms;

    static string horizontalAxisPath = "Armature.003/horizontalAxis";
    static string verticalAxisPath = "Armature.003/horizontalAxis/verticalAxis";
    static string legAngleJointAxis1Path = "leg1/leg1Axis";
    static string legAngleJointAxis2Path = "leg2/leg2Axis";
    static string legAngleJointAxis3Path = "leg3/leg3Axis";
    static string legLock1Path = "leg1/leg1Axis/leg1.001";
    static string legLock2Path = "leg2/leg2Axis/leg2.001";
    static string legLock3Path = "leg3/leg3Axis/leg3.001";
    static string bodyPath = "Armature.003/horizontalAxis/Cube";

    Text textHorizontalAngle;
    Text textVerticalAngle;
    Text textSlantDistance;
    Text textCoordinates;
    Text textRelativeCoordinates;
    Text textFov;

    static string telescopePath = verticalAxisPath + "/Camera";

    Quaternion initialHorizontalRotation;
    Quaternion initialVerticalRotation;

    private float RoundTo1st(float value)
    {
        return Mathf.RoundToInt(value * 10F) / 10F;
    }

    private float SlantDistance()
    {
        RaycastHit hit;
        float slantDistance = -1F;

        if (Physics.Raycast(telescope.transform.position, telescope.transform.TransformDirection(Vector3.forward), out hit, 500F))
            slantDistance = hit.distance;
        return slantDistance;
    }

    private Vector3 PolarToCartesian(float rotationX, float rotationY, float r)
    {
        Vector3 origin = new Vector3(0, 0, r);
        Quaternion rotation = Quaternion.Euler(-rotationX, rotationY, 0);
        return rotation * origin;
    }

    private Vector3 ToAbsoluteSurveyPosition(Vector3 relativeSurveyPosition)
    {
        Debug.Log($"{telescope.transform.position.x},{telescope.transform.position.y},{telescope.transform.position.z}");
        return telescope.transform.position + relativeSurveyPosition;
    }

    // Returns FOV
    private float Magnify(float slantDistance)
    {
        if (slantDistance <= 0) return 3F; 
        else return telescope.fieldOfView = 20F / slantDistance;
    }

    private void OrientTelescope(GameObject prism)
    {
        horizontalAxis.rotation = initialHorizontalRotation;
        verticalAxis.rotation = initialVerticalRotation;

        Vector3 direction = prism.transform.position - transform.position;
        Vector3 directionXZ = new Vector3(direction.x, 0, direction.z);
        float rotationY = Vector3.SignedAngle(Vector3.forward, directionXZ, Vector3.up);
        horizontalAxis.Rotate(0, rotationY, 0);
        float rotationX = Vector3.SignedAngle(directionXZ, direction, verticalAxis.right);
        verticalAxis.Rotate(rotationX, 0, 0);

        float slantDistance = SlantDistance();
        textSlantDistance.text = $"Slant distance: {RoundTo1st(slantDistance)} m";
        float fov = Magnify(slantDistance);
        textFov.text = $"FOV: {RoundTo1st(fov)} deg";

        if (rotationY < 0) rotationY += 360F;
        textHorizontalAngle.text = $"Horizontal angle: {RoundTo1st(rotationY)} deg";
        textVerticalAngle.text = $"Vertical angle: {RoundTo1st(rotationX)} deg";

        Vector3 relativeSurveyPosition = PolarToCartesian(rotationX, rotationY, slantDistance);
        textRelativeCoordinates.text = $"Relative coordinates: {RoundTo1st(relativeSurveyPosition.x)}, {RoundTo1st(relativeSurveyPosition.y)}, {RoundTo1st(relativeSurveyPosition.z)} m";

        Vector3 absoluteSurveyPosition = ToAbsoluteSurveyPosition(relativeSurveyPosition);
        textCoordinates.text = $"Absolute coordinates: {RoundTo1st(absoluteSurveyPosition.x)}, {RoundTo1st(absoluteSurveyPosition.y)}, {RoundTo1st(absoluteSurveyPosition.z)} m";

    }

    // Start is called before the first frame update
    void Start()
    {
        prisms = new GameObject[] { prism1, prism2, prism3, prism4, prism5, prism6, prism7, prism8 };

        horizontalAxis = transform.Find(horizontalAxisPath);
        horizontalAxis.rotation = Quaternion.Euler(0F, 180F, 0F);  // Set the forward direction To the North

        verticalAxis = transform.Find(verticalAxisPath);
        legAngleJointAxis1 = transform.Find(legAngleJointAxis1Path);
        legAngleJointAxis2 = transform.Find(legAngleJointAxis2Path);
        legAngleJointAxis3 = transform.Find(legAngleJointAxis3Path);
        legLock1 = transform.Find(legLock1Path);
        legLock2 = transform.Find(legLock2Path);
        legLock3 = transform.Find(legLock3Path);
        body = transform.Find(bodyPath);

        telescope = transform.Find(telescopePath).GetComponent<Camera>();

        initialHorizontalRotation = horizontalAxis.rotation;
        initialVerticalRotation = verticalAxis.rotation;

        legAngleJointAxis1.Rotate(0, legAngle, 0);
        legAngleJointAxis2.Rotate(0, legAngle, 0);
        legAngleJointAxis3.Rotate(0, legAngle, 0);

        Vector3 leg1Pos = legLock1.localPosition;
        legLock1.localPosition = new Vector3(leg1Pos.x, leg1Pos.y, leg1Pos.z - legExtension);
        Vector3 leg2Pos = legLock2.localPosition;
        legLock2.localPosition = new Vector3(leg2Pos.x, leg2Pos.y, leg2Pos.z - legExtension);
        Vector3 leg3Pos = legLock3.localPosition;
        legLock3.localPosition = new Vector3(leg3Pos.x, leg3Pos.y, leg3Pos.z - legExtension);

        GameObject.FindWithTag("prism1").GetComponent<Button>().onClick.AddListener(delegate { onClickListener(0); });
        GameObject.FindWithTag("prism2").GetComponent<Button>().onClick.AddListener(delegate { onClickListener(1); });
        GameObject.FindWithTag("prism3").GetComponent<Button>().onClick.AddListener(delegate { onClickListener(2); });
        GameObject.FindWithTag("prism4").GetComponent<Button>().onClick.AddListener(delegate { onClickListener(3); });
        GameObject.FindWithTag("prism5").GetComponent<Button>().onClick.AddListener(delegate { onClickListener(4); });
        GameObject.FindWithTag("prism6").GetComponent<Button>().onClick.AddListener(delegate { onClickListener(5); });
        GameObject.FindWithTag("prism7").GetComponent<Button>().onClick.AddListener(delegate { onClickListener(6); });
        GameObject.FindWithTag("prism8").GetComponent<Button>().onClick.AddListener(delegate { onClickListener(7); });

        textHorizontalAngle = GameObject.FindWithTag("horizontalAngle").GetComponent<Text>();
        textVerticalAngle = GameObject.FindWithTag("verticalAngle").GetComponent<Text>();
        textCoordinates = GameObject.FindWithTag("coordinates").GetComponent<Text>();
        textRelativeCoordinates = GameObject.FindWithTag("relativeCoordinates").GetComponent<Text>();
        textSlantDistance = GameObject.FindWithTag("slantDistance").GetComponent<Text>();
        textFov = GameObject.FindWithTag("fov").GetComponent<Text>();
    }

    void onClickListener(int idx)
    {
        OrientTelescope(prisms[idx]);
    }

}
