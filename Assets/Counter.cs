using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;


public class Counter : MonoBehaviour
{
    enum Mode { counter, intensity }
    [SerializeField]
    int val;
    [SerializeField]
    Mode mode = Mode.counter;
    public int max = 0;
    public int min = 100;

    public int Value {
        get { return val; }
        set
        {
            val = value;
            if (val < min)
                val = min;
            if (val > max)
                val = max;
            if (mode == Mode.counter)
                tmp.text = val.ToString();
            else if (mode == Mode.intensity)
                tmp.text = (new string[] {"", "Low", "Middle", "High" })[val];
            
        }
    }


    TextMeshProUGUI tmp;
    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
