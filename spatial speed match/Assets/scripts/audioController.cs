using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioController : MonoBehaviour
{
    public List<AudioClip> audio = new List<AudioClip>();
    public AudioSource audioSource;

    public static audioController instance;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void wrongAns()
    {
        audioSource.clip = (audio[0]);
        audioSource.Play();
    }

    public void rightAns()
    {
        audioSource.clip = (audio[1]);
        audioSource.Play();
    }    

    public void endGame()
    {
        audioSource.clip = (audio[4]);
        audioSource.Play();
    }

    public void counter()
    {
        audioSource.clip = (audio[2]);
        audioSource.Play();
    }
    public void popup()
    {
        audioSource.clip = (audio[3]);
        audioSource.Play();
    }
}
