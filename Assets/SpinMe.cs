using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinMe : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        amount = Random.Range(0, 360);
    }
    float amount;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * amount);
    }
}
