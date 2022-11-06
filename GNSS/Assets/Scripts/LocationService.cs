using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LocationService : MonoBehaviour
{
    [SerializeField]
    Text m_textLatitude;

    [SerializeField]
    Text m_textLongitude;

    [SerializeField]
    Text m_textAltitude;

    [SerializeField]
    Text m_textDebug;

    [SerializeField]
    Text m_textDistanceX;

    [SerializeField]
    Text m_textDistanceY;

    [SerializeField]
    Text m_textDistance;

    [SerializeField]
    float m_updateInterval = 5F;

    [SerializeField]
    float m_desiredAccuracyInMeters = 3F;

    [SerializeField]
    float m_updateDistanceInMeters = 3F;

    bool m_isReady = false;
    bool m_alreadySet = false;
    LocationInfo m_referencePoint;
    LocationInfo m_lastLocationInfo;
    float m_lastDistanceX;
    float m_lastDistanceY;
    float m_lastDistance;

    void Start()
    {
        StartCoroutine(PeriodicUpdate());
    }

    public void SetReferencePoint()
    {
        if (m_isReady)
        {
            m_referencePoint = Input.location.lastData;
            m_alreadySet = true;
        }
        else
        {
            m_textDebug.text = "Not ready";
        }
    }

    int cnt = 0;

    private IEnumerator PeriodicUpdate()
    {
        if (!Input.location.isEnabledByUser)
        {
            m_textDebug.text = "Location service not granted by user";
            yield break;
        }

        Input.location.Start(m_desiredAccuracyInMeters, m_updateDistanceInMeters);

        int timeout = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && timeout > 0)
        {
            yield return new WaitForSeconds(1);
            timeout--;
        }

        if (timeout <= 0)
        {
            m_textDebug.text = "Time out";
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            m_textDebug.text = "Failed";
            yield break;
        }

        m_isReady = true;

        while (true)
        {
            m_lastLocationInfo = Input.location.lastData;

            if (m_alreadySet)
            {
                m_lastDistanceX = Util.DistanceX(m_referencePoint.latitude, m_referencePoint.longitude, m_lastLocationInfo.longitude);
                m_lastDistanceY = Util.DistanceY(m_referencePoint.latitude, m_referencePoint.longitude, m_lastLocationInfo.latitude);
                m_lastDistance = Util.Distance(m_referencePoint.latitude, m_referencePoint.longitude, m_lastLocationInfo.latitude, m_lastLocationInfo.longitude);
            }

            m_textDebug.text = $"{cnt++}";

            m_textLatitude.text = $"Latitude: {m_lastLocationInfo.latitude}";
            m_textLongitude.text = $"Longitude: {m_lastLocationInfo.longitude}";
            m_textAltitude.text = $"Altitude: {m_lastLocationInfo.altitude}";

            m_textDistanceX.text = $"DistanceX: {m_lastDistanceX.ToString("F1")}m";
            m_textDistanceY.text = $"DistanceY: {m_lastDistanceY.ToString("F1")}m";
            m_textDistance.text = $"Distance: {m_lastDistance.ToString("F1")}m";

            yield return new WaitForSeconds(m_updateInterval);
        }
    }
}