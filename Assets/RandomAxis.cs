using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAxis : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(Random.onUnitSphere, Random.Range(0,999));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
