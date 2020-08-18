using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FresnelZoneGenerator : MonoBehaviour
{

    public float frequencyInMHz = 920F;
    public GameObject antenna1;
    public GameObject antenna2;
    public bool renderLine = true;

    private LineRenderer radioWave;

    GameObject fresnelZone;
    float distance;

    Text textDistance;
    Text textRadius;

    float fresnelRadius(float distance)
    {
        float lambda = 300F / frequencyInMHz;  // Speed of light = 3 * 10^8 /msec
        return Mathf.Sqrt(lambda * distance) / 2F;
    }

    // Start is called before the first frame update
    void Start()
    {
        textDistance = GameObject.Find("TextDistance").GetComponent<Text>();
        textRadius = GameObject.Find("TextRadius").GetComponent<Text>();

        radioWave = gameObject.AddComponent<LineRenderer>();
        radioWave.widthMultiplier = 0.1F;

        radioWave.SetPosition(0, antenna1.transform.position);
        radioWave.SetPosition(1, antenna2.transform.position);
        radioWave.enabled = true;

        Vector3 direction = antenna1.transform.position - antenna2.transform.position;
        distance = direction.magnitude;

        fresnelZone = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        fresnelZone.transform.position = antenna1.transform.position;
        fresnelZone.transform.localPosition = new Vector3(0, 0, 0);
        Destroy(fresnelZone.GetComponent<SphereCollider>());
        Material mat = Resources.Load("FresnelZoneColor", typeof(Material)) as Material;
        fresnelZone.GetComponent<Renderer>().material = mat;
    }

    static float t = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (t <= 1.0F)
        {
            var scaleZ = Mathf.Lerp(0F, distance, t);
            t += 0.2f * Time.deltaTime;
            var radius = fresnelRadius(scaleZ);
            var diameter = radius * 2F;
            fresnelZone.transform.localScale = new Vector3(diameter, diameter, scaleZ);
            fresnelZone.transform.position = antenna1.transform.position;
            fresnelZone.transform.LookAt(antenna2.transform);
            fresnelZone.transform.Translate(0F, 0F, scaleZ / 2F);
            textDistance.text = $"Distance: {Mathf.RoundToInt(scaleZ * 10F) / 10F} meters";
            textRadius.text = $"Fresnel radius: {Mathf.RoundToInt(radius * 10F) / 10F} meters";
            //Debug.Log($"Fresnel radius: {radius} meters, distance/2: {distance / 2F} meters, distance: {distance} meters");
        }
    }

}
