using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueDontDestoryOnLoad : MonoBehaviour
{
    static bool exists = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (exists == false)
            exists = true;
        else
            Destroy(this.gameObject);
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
