using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Gender
{
    F, M
}
public class ArgsGetter : MonoBehaviour
{
    static public int Age { get; private set; }
    static public int Weight { get; private set; }
    static public Gender Gender_ { get; private set; }
    static public float K {
            get {
            return Gender_ == Gender.M ?
                -55.0969f + 0.1988f * Weight + 0.2017f * Age :
                -20.4022f - 0.1263f * Weight + 0.074f * Age;
        }
    }
    static public float Q
    {
        get
        {
            return Gender_ == Gender.M ? 0.6309f : 0.4472f;
        }
    }

    //M = ( -55.0969 + 0.1988W +0.2017A + 0.6309HR )/ 4.184 x T
    //        ------------------k---------------   ---Q---
    //F  = (-20.4022 - 0.1263W + 0.074A  + 0.4472HR ) / 4.184) x T

    public Counter age;
    public Counter weight;
    public TMPro.TextMeshProUGUI gender;
    
    public void UpdateValue()
    {
        Age = age.Value;
        Weight = weight.Value;
        Gender_ = gender.text == "M" ? Gender.M : Gender.F;
    }
    public float GetKcal(float timeminute, float meanhr)
    {
        return (K + meanhr * Q) / 4.184f * timeminute;
    }
}
