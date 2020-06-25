using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSinglePendulum : MonoBehaviour
{

    public void OnSinglePendulumClick()
    {
        SceneManager.LoadScene("Pendulum");
    }
}
