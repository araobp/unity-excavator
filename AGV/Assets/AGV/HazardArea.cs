using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardArea : MonoBehaviour
{
    AgvController controller;
    bool red;

    public void SetController(AgvController controller, bool red)
    {
        this.controller = controller;
        this.red = red;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject parent = transform.parent.gameObject;
        AgvController controller = parent.GetComponent<AgvController>();
        this.controller.OnHazardEnter(red, other);
    }
}
