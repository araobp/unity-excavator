using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismController : MonoBehaviour
{
    private GameObject totalStation;

    Transform verticalAxis;
    static string verticalAxisPath = "Armature.001/Bone/VerticalAxis";

    // Start is called before the first frame update
    void Start()
    {
        totalStation = GameObject.FindWithTag("totalStation");

        verticalAxis = transform.Find(verticalAxisPath);
        Vector3 direction = totalStation.transform.position - transform.position;
        Vector3 directionXZ = new Vector3(direction.x, 0, direction.z);
        float rotationY = Vector3.SignedAngle(Vector3.forward, directionXZ, Vector3.up) + 90F;
        transform.Rotate(0, rotationY, 0);
        float rotationX = Vector3.SignedAngle(directionXZ, direction, verticalAxis.right);
        verticalAxis.Rotate(rotationX, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
