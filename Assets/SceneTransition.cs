using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition instance;
    public Image image;
    public float speed;

    Action callback;
    public SceneTransition()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator FadeOut()
    {
        Color c = image.color;
        for (float ft = 0; ft <= 1f; ft += 0.05f)
        {
            c.a = ft;
            image.color = c;
            yield return new WaitForSeconds(1 / speed);
        }
        c.a = 1f;
        image.color = c;
        callback();
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        Color c = image.color;
        for (float ft = 1f; ft >= 0; ft -= 0.1f)
        {
            c.a = ft;
            image.color = c;
            yield return new WaitForSeconds(1 / speed);
        }
        c.a = 0f;
        image.color = c;
        coroutine = null;
    }

    Coroutine coroutine = null;
    public void StartTransition(Action callback)
    {
        if (coroutine == null)
        {
            this.callback = callback;
            coroutine = StartCoroutine(FadeOut());
        }
    }

    public void FadeOut(Action callback)
    {
        callback();
    }
}
