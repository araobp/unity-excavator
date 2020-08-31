using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FresnelZone : MonoBehaviour
{
    private GameObject gameObject;
    private float frequencyInMHz;
    private LineRenderer radioWave;
    private GameObject antenna1;
    private GameObject antenna2;
    private GameObject fresnelZone;
    private float distance;

    public FresnelZone(float frequencyInMHz, GameObject antenna1, GameObject antenna2, bool access = false)
    {
        gameObject = new GameObject();
        this.frequencyInMHz = frequencyInMHz;
        this.antenna1 = antenna1;
        this.antenna2 = antenna2;

        gameObject.AddComponent<LineRenderer>();
        radioWave = gameObject.GetComponent<LineRenderer>();
        radioWave.widthMultiplier = 0.1F;
        radioWave.positionCount = 2;
        radioWave.SetPosition(0, antenna1.transform.position);
        radioWave.SetPosition(1, antenna2.transform.position);

        Vector3 direction = antenna2.transform.position - antenna1.transform.position;
        distance = direction.magnitude;

        radioWave.enabled = true;

        fresnelZone = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        fresnelZone.transform.position = antenna1.transform.position;
        Destroy(fresnelZone.GetComponent<SphereCollider>());

        Material mat;
        if (access)
        {
            mat = Resources.Load("FresnelZoneColor2", typeof(Material)) as Material;
        }
        else
        {
            mat = Resources.Load("FresnelZoneColor", typeof(Material)) as Material;
        }
        fresnelZone.GetComponent<Renderer>().material = mat;

        distance = (antenna2.transform.position - antenna1.transform.position).magnitude;

    }

    private float fresnelRadius(float distance)
    {
        float lambda = 300F / frequencyInMHz;  // Speed of light = 3 * 10^8 /msec
        return Mathf.Sqrt(lambda * distance) / 2F;
    }

    public void update(float t, out float scale, out float radius)
    {
        radioWave.SetPosition(0, antenna1.transform.position);
        radioWave.SetPosition(1, antenna2.transform.position);
        Vector3 direction = antenna2.transform.position - antenna1.transform.position;
        distance = direction.magnitude;

        var scaleZ = Mathf.Lerp(0F, distance, t);
        t += 0.05f * Time.deltaTime;
        var r = fresnelRadius(scaleZ);
        var diameter = r * 2F;
        fresnelZone.transform.localScale = new Vector3(diameter, diameter, scaleZ);
        fresnelZone.transform.position = antenna1.transform.position;
        fresnelZone.transform.LookAt(antenna2.transform);
        fresnelZone.transform.Translate(0F, 0F, scaleZ / 2F);
        scale = scaleZ;
        radius = r;
    }
}

public class FresnelZoneGenerator : MonoBehaviour
{

    public float frequencyInMHz = 2400F;  // 2.4GHz
    public GameObject antenna;
    public GameObject antennaRepeater;
    public GameObject antennaTerminal1;
    public GameObject antennaTerminal2;
    public GameObject antennaTerminal3;

    public bool renderLine = true;

    private FresnelZone fresnelZone;
    private List<FresnelZone> fresnelZones = new List<FresnelZone>();
    private GameObject[] antennaTerminals;

    Text textDistance;
    Text textRadius;

    // Start is called before the first frame update
    void Start()
    {
        textDistance = GameObject.FindWithTag("fresnelDistance").GetComponent<Text>();
        textRadius = GameObject.FindWithTag("fresnelRadius").GetComponent<Text>();

        antennaTerminals = new GameObject[] { antennaTerminal1, antennaTerminal2, antennaTerminal3 };

        fresnelZone = new FresnelZone(frequencyInMHz, antenna, antennaRepeater);

        foreach (var antenna in antennaTerminals)
        {
            if (antenna != null)
            {
                fresnelZones.Add(new FresnelZone(frequencyInMHz, antennaRepeater, antenna, true));
            }
        }
    }

    static float t = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (t <= 1.0F)
        {
            t += 0.3F * Time.deltaTime;

            float scaleZ;
            float radius;
            fresnelZone.update(t, out scaleZ, out radius);
            textDistance.text = $"Distance to repeater: {Mathf.RoundToInt(scaleZ * 10F) / 10F} meters";
            textRadius.text = $"Fresnel radius: {Mathf.RoundToInt(radius * 10F) / 10F} meters";
            //Debug.Log($"Fresnel radius: {radius} meters, distance/2: {distance / 2F} meters, distance: {distance} meters");
        }
        else if (antennaTerminal1 != null && t <= 2.0F)
        {
            t += 0.5F * Time.deltaTime;

            var tt = t - 1.0F;

            foreach (var fresnelZone in fresnelZones)
            {
                float scaleZ;
                float radius;
                fresnelZone.update(tt, out scaleZ, out radius);
            }
        }
        else
        {
            float scaleZ;
            float radius;
            fresnelZone.update(1F, out scaleZ, out radius);
            foreach (var fresnelZone in fresnelZones)
            {
                fresnelZone.update(1F, out scaleZ, out radius);
            }
        }
    }

}