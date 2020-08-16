using UnityEngine;

public class Arrow
{
    GameObject arrow;
    float shaftThickness;

    public enum Colors {
        RED,
        GREEN,
        BLUE,
        BLACK
    };

    public Arrow(Colors color, float headsize=1F, float shaftThickness=1F)
    {
        this.shaftThickness = shaftThickness;

        Material mat = Resources.Load("Materials/Red") as Material;

        GameObject arrowPrefab = Resources.Load("Prefabs/Arrow") as GameObject;
        arrow = Object.Instantiate(arrowPrefab);

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

        Transform arrowHeadTransform = arrow.transform.GetChild(0);
        Transform arrowShaftTransform = arrow.transform.GetChild(1);

        arrowHeadTransform.GetComponent<Renderer>().material = mat;
        arrowHeadTransform.localScale = new Vector3(headsize, headsize, headsize);
        arrowShaftTransform.localScale = new Vector3(shaftThickness, shaftThickness, 1F);
    }

    public void OrientVector(Transform vectorOrigin, Vector3 vector)
    {

        Vector3 startPoint = vectorOrigin.position;
        Vector3 endPoint = vector + vectorOrigin.position;
        Vector3 direction = endPoint - startPoint;

        float length = direction.magnitude * 10F - 2F;  // 1/10 scale, subtract the cone length
        arrow.transform.position = endPoint;
        arrow.transform.GetChild(1).transform.localScale = new Vector3(shaftThickness, shaftThickness, length);
        arrow.transform.LookAt(vectorOrigin);
    }

}
