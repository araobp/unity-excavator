using UnityEngine;

public class VectorTest : MonoBehaviour
{
    Arrow arrow;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        //arrow = Instantiate(arrowPrefab);
        arrow = new Arrow(Arrow.Colors.BLUE);
        rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(20F, 20F, 0F);
    }

    // Update is called once per frame
    void Update()
    {
        arrow.OrientVector(transform, rb.velocity/10F);
    }
}
