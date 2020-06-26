using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector : MonoBehaviour
{
    public GameObject arrowPrefab;

    private GameObject arrow;
    private Rigidbody rb;

    void OrientVector(GameObject arrow, Transform transform, Vector3 vector)
    {
        Vector3 startPoint = transform.position;
        Vector3 endPoint = vector + transform.position;
        Vector3 direction = endPoint - startPoint;

        float length = direction.magnitude * 10F - 2F;  // 1/10 scale, subtract the cone length
        arrow.transform.position = endPoint;
        arrow.transform.GetChild(1).transform.localScale = new Vector3(1F, 1F, length);
        arrow.transform.LookAt(transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        arrow = Instantiate(arrowPrefab);
        rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(20F, 20F, 0F);
    }

    // Update is called once per frame
    void Update()
    {
        OrientVector(arrow, transform, rb.velocity/10F);
    }
}
