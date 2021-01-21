using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDirector : MonoBehaviour
{
    public GameObject character;

    Animator animator;

    private void Start()
    {
        animator = character.GetComponent<Animator>();
    }

    public void Turn()
    {
        animator.SetTrigger("Turn");
    }

    public void Jump()
    {
        animator.SetTrigger("Jump");
    }

    public void Dance()
    {
        animator.SetTrigger("Dance");
    }

    public void Kick()
    {
        animator.SetTrigger("Kick");
    }
}
