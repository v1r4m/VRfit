using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;


[RequireComponent(typeof(LineRenderer), typeof(RectTransform))]
class GraphDrawer : MonoBehaviour
{
    int sampleCount = 100;
    public float[] hrSamples;
    public float[] hrThresSamples;
    public float hrMin = 80;
    public float hrMax = 150;
    Vector3 leftDown;
    Vector3 UpVector;
    Vector3 RightVector;

    //public Transform hrIndicator;
    bool remap = false;
    void OnResize()
    {
        Vector3[] corners = new Vector3[4];
        GetComponent<RectTransform>().GetWorldCorners(corners);//It starts bottom left and rotates to top left, then top right, and finally bottom right.
        leftDown = corners[0];
        UpVector = corners[1] - corners[0];
        RightVector = corners[3] - corners[0];

        LineRenderer lr = GetComponent<LineRenderer>();
        lr.positionCount = sampleCount;

        for (int i = 0; i < sampleCount; i++)
            lr.SetPosition(i,
                ProjectTo3D(new Vector2(
                            i / (float)sampleCount, 
                            (hrSamples[i]-hrMin)/(hrMax - hrMin)
                            )
                )
            );
    }

    private void OnRectTransformDimensionsChange()
    {
        remap = true;
    }
    private void Update()
    {
        if (remap) OnResize();
        remap = false;
    }
    void Start()
    {
        LineRenderer r = GetComponent<LineRenderer>();
        //StringBuilder sb = new StringBuilder("hr, targetSPN(difficulty), avgDelta (real difficulty), hrthres\n");

        var file = File.ReadAllLines("log.csv").Select(s => s.Split(',')).ToList();
        file.RemoveAt(0);//remove header

        var hr = file.Select(line => float.Parse(line[0])).ToList();
        var hrThres = file.Select(line => float.Parse(line[3])).ToList();

        hrSamples = new float[sampleCount];
        hrThresSamples = new float[sampleCount];

        int linesPerSample = (hrSamples.Length / file.Count) + 1;

        for (int i = 0; i < sampleCount; i++)
        {
            int from = Mathf.Max(0,
                    Mathf.Min (
                        hrSamples.Length - linesPerSample ,
                        (int) (  
                                    (float)hrSamples.Length / file.Count * ( i +0.5f)  
                        )
                    )
                );
            hrSamples[i] = (float)Enumerable.Range(from, linesPerSample).Average(q => hr[q]);
            hrThresSamples[i] = (float)Enumerable.Range(from, linesPerSample).Average(q => hrThres[q]);
        }

        OnResize();
    }

    Vector3 ProjectTo3D(Vector2 coord)
    {
        return leftDown + Mathf.Max(coord.x, 0) * RightVector + Mathf.Max(coord.y, 0) * UpVector;
    }
}
