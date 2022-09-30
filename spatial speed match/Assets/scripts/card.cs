using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class card : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void entercurrent()
    {
        anim.Play("enterCurrent");
    }
    public void enter()
    {
        anim.Play("enter");
    }
    public void enterfill()
    {
        anim.Play("enterfill");
    }

    public void exit()
    {
        anim.Play("exit");
    }

    public void idle()
    {
        anim.Play("idle");
    }
    public void enterEndPanel()
    {
        anim.Play("enterEndPanel");
    }

    public void exitEndPanel()
    {
        anim.Play("exitEndPanel");
    }

    public void rotateCard()
    {
        anim.Play("rotatecard");
    }
    
}
