using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer instance;

    public AudioClip mainTheme, click, click2, hit, attack;

    private AudioSource[] src;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        src = GetComponents<AudioSource>();
        StartMainTheme();
    }

    public void StartMainTheme()
    {
        src[0].clip = mainTheme;
        src[0].loop = true;
        src[0].Play();
    }

    public void PlayClick()
    {
        src[1].pitch = Random.Range(0.9f, 1.1f);
        src[1].PlayOneShot(click);
    }

    public void PlayClick2()
    {
        src[1].pitch = Random.Range(0.9f, 1.1f);
        src[1].PlayOneShot(click2);
    }

    public void PlayHit()
    {
        src[1].pitch = Random.Range(0.8f, 1.2f);
        src[1].PlayOneShot(hit);
    }

    public void PlayAttack()
    {
        src[2].pitch = Random.Range(0.8f, 1.2f);
        src[2].PlayOneShot(attack);
    }

}
