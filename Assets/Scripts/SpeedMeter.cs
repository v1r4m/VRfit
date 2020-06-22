using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedMeter : MonoBehaviour
{
    Vector3 lastPos;
    public Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (
                float.IsNaN(velocity.x) || float.IsInfinity(velocity.x) ||
                float.IsNaN(velocity.y) || float.IsInfinity(velocity.y) ||
                float.IsNaN(velocity.z) || float.IsInfinity(velocity.z)
            )
            velocity = Vector3.zero;

        velocity *= 0.95f;
        if(Time.deltaTime != 0)
            velocity += ((transform.position - lastPos)/Time.deltaTime) * 0.05f;
        lastPos = transform.position;
    }
}
