using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CompassController : MonoBehaviour
{
    RawImage m_RawImageCompassNeedle = null;

    // Start is called before the first frame update
    void Start()
    {
    }

    void OnEnable()
    {
        m_RawImageCompassNeedle = GetComponent<RawImage>();
        m_RawImageCompassNeedle.transform.eulerAngles = Vector3.zero;

        Input.compass.enabled = true;
        Input.location.Start();
        StartCoroutine(Wait());
    }

    void OnDisable()
    {
        Input.location.Stop();
        Input.compass.enabled = false;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1F);  // wait for location service start up
    }

    void Update()
    {
        m_RawImageCompassNeedle.transform.eulerAngles = new Vector3(0, 0, Input.compass.trueHeading);
    }

}
