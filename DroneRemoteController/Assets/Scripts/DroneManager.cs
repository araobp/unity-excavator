using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;

        float deltaXLeft = m_StickControllerLeft.deltaX * deltaTime * m_MultiplierTranslate;
        float deltaYLeft = m_StickControllerLeft.deltaY * deltaTime * m_MultiplierTranslate;
        float deltaXRight = m_StickControllerRight.deltaX * deltaTime * m_MultiplierRotate;
        float deltaYRight = m_StickControllerRight.deltaY * deltaTime * m_MultiplierTranslate;
        Transform cam = m_CameraObject.transform;
        cam.Translate(new Vector3(10F * deltaXRight, 10F * deltaYLeft, 20F * deltaYRight));
        cam.Rotate(new Vector3(0F, 100F * deltaXLeft, 0F));

    }
}
