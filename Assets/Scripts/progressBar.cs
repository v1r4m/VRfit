using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class progressBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite Bar;
    float timer = 0.0f;
    float newx;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime/600;
        newx = timer;

        transform.localScale = new Vector3(timer, 0.1f, 0.1f);
    }
}
