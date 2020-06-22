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
    public Image Bar;
    public Transform human;
    MusicPlayer mp;
    void Start()
    {
        mp = FindObjectOfType<MusicPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        Bar.fillAmount = mp.progress;
        Vector3[] corners = new Vector3[4];
        GetComponent<RectTransform>().GetWorldCorners(corners);//It starts bottom left and rotates to top left, then top right, and finally bottom right.
        Vector3 leftUp = corners[1];
        Vector3 RightVector = corners[3] - corners[0];

        human.position = leftUp + RightVector * mp.progress;
    }
}
