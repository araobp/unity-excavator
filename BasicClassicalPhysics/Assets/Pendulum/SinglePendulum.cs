using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SinglePendulum : MonoBehaviour
{
    Text textLength;
    Text textAngle;
    Text textSpeed;
    Text textPeriod;

    Rigidbody rb;
    float prevSpeed;
    float prevTurnAroundTime;
    float currentTime;

    float angle;
    float speed;
    float period;

    Arrow arrow;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        textLength = GameObject.Find("TextLength").GetComponent<Text>();
        textAngle = GameObject.Find("TextAngle").GetComponent<Text>();
        textSpeed = GameObject.Find("TextSpeed").GetComponent<Text>();
        textPeriod = GameObject.Find("TextPeriod").GetComponent<Text>();

        float length = GameObject.Find("String").transform.localScale.y;
        textLength.text = $"Length: {Mathf.Round(length * 10) / 10F} (m)";

        arrow = new Arrow(Arrow.Colors.RED, 5F, 3F);

        while (true)
        {
            yield return new WaitForSeconds(0.5F);
            Show();
        }
    }

    void Show()
    {
        textAngle.text = $"Angle: {Mathf.Round(angle*10)/10F} (deg)";
        textSpeed.text = $"Speed: {Mathf.Round(speed*10)/10F} (m/s)";
        textPeriod.text = $"Period: {Mathf.Round(period*10)/10F} (sec)";
    }

    // Update is called once per frame
    void Update()
    {
        float time = Time.time;

        angle = Vector3.SignedAngle(transform.up, Vector3.up, Vector3.forward);
        speed = transform.InverseTransformDirection(rb.velocity).x;
        //Debug.Log($"angle: {angle}, speed: {speed}");

        Vector3 gravityTangent = rb.mass * Vector3.Dot(Physics.gravity, rb.transform.right) * rb.transform.right;
        arrow.OrientVector(transform, gravityTangent);

        if (prevSpeed <= 0 && speed > 0)
        {
            currentTime = time;
            period = currentTime - prevTurnAroundTime;
            //Debug.Log($"period: {period}");
            prevTurnAroundTime = currentTime;
        }

        prevSpeed = speed;
    }
}
