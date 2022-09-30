using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timer : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void countDown()
    {
        anim.Play("countDown");
    }

    public void idle()
    {
        anim.Play("idle");
    }
}
