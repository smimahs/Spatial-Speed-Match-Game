using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class result : MonoBehaviour
{

    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void showCorrect()
    {
        anim.Play("showCorrect");
    }

    public void showWrong()
    {
        anim.Play("showWrong");
    }

    public void idle()
    {
        anim.Play("idle");
    }
}
