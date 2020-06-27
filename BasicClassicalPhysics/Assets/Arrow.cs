using UnityEngine;

public class Arrow: MonoBehaviour
{
    GameObject arrow;

    public enum Colors {
        RED,
        GREEN,
        BLUE,
        BLACK
    };

    public Arrow(Colors color)
    {
        Material mat = Resources.Load("Materials/Red") as Material;

        GameObject arrowPrefab = Resources.Load("Prefabs/Arrow") as GameObject;
        arrow = Instantiate(arrowPrefab);

        switch (color)
        {
            case Colors.RED:
                mat = Resources.Load("Materials/Red") as Material;
                break;
            case Colors.GREEN:
                mat = Resources.Load("Materials/Green") as Material;
                break;
            case Colors.BLUE:
                mat = Resources.Load("Materials/Blue") as Material;
                break;
            case Colors.BLACK:
                mat = Resources.Load("Materials/Black") as Material;
                break;
        }

        arrow.transform.GetChild(0).GetComponent<Renderer>().material = mat;
    }

    public void OrientVector(Transform vectorOrigin, Vector3 vector)
    {

        Vector3 startPoint = vectorOrigin.position;
        Vector3 endPoint = vector + vectorOrigin.position;
        Vector3 direction = endPoint - startPoint;

        float length = direction.magnitude * 10F - 2F;  // 1/10 scale, subtract the cone length
        arrow.transform.position = endPoint;
        arrow.transform.GetChild(1).transform.localScale = new Vector3(1F, 1F, length);
        arrow.transform.LookAt(vectorOrigin);
    }

}
