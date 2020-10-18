using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FreeFall : MonoBehaviour
{
    public float height = 30F;

    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        GameObject o = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        o.AddComponent<Rigidbody>();
        float center = o.transform.localScale.y / 2F;
        o.transform.position = new Vector3(0F, height + center, 0F);

        Text text = GameObject.Find("TextHeight").GetComponent<Text>();
        startTime = Time.time;
        text.text = $"Height: {height} m";

        GameObject.Find("ButtonClose").GetComponent<Button>().onClick.AddListener(
            delegate
            {
                GoHome();
            }
        );
    }

    private void OnCollisionEnter(Collision collision)
    {
        float elapsedTime = Time.time - startTime;
        Text text = GameObject.Find("TextElapsedTime").GetComponent<Text>();
        text.text = $"Elapsed time: {elapsedTime} sec";
    }

    private void GoHome()
    {
        SceneManager.LoadScene("Scenes/Menu");
    }

}
