using UnityEngine;

public class HeightMapManipulation : MonoBehaviour
{
    public Terrain terrain;

    [Header("Embankment volume")]
    public float left = 130F;
    public float top = 160F;
    public float right = 160F;
    public float bottom = 130F;
    public float height = 80F;

    Embankment embankment;

    // Start is called before the first frame update
    void Start()
    {
        embankment = new Embankment(terrain);
        embankment.flattenTerrain(100F);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))  // Flatten
        {
            embankment.flattenTerrain(100F);
        }
        else if (Input.GetKeyDown(KeyCode.R))  // Raise embankment
        {
            embankment.generateEmbankment(left, top, right, bottom, 100F+height);
        } else if (Input.GetKeyDown(KeyCode.L))  // Lower embankment
        {
            embankment.generateEmbankment(left, top, right, bottom, 100F-height);
        }
    }
}
