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
    static public int Intensity { get; private set; }
    static public float musicLength = 4 + 16 / 60f;

    static public float HRMax { get { return Gender_ == Gender.M ? 213.6f - Age * 0.79f : 208.8f - 0.72f * Age; } }
    static public float TargetHR {
        get {
            switch (Intensity)
            {
                case 3:
                    return HighIntenstyTargetHR;
                case 2:
                    return MidIntenstyTargetHR;
                case 1:
                    return LowIntenstyTargetHR;
            }
            return HighIntenstyTargetHR;
        }
    }
    static public float HighIntenstyTargetHR { get { return HRMax * 0.85f;  } }
    static public float MidIntenstyTargetHR { get { return HRMax * 0.75f; } }
    static public float LowIntenstyTargetHR { get { return HRMax * 0.65f; } }
    
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
    public Counter intensity;
    public TMPro.TextMeshProUGUI gender;
    
    public void UpdateValue()
    {
        Age = age.Value;
        Weight = weight.Value;
        Gender_ = gender.text == "M" ? Gender.M : Gender.F;
        Intensity = intensity.Value;
    }
    static public float GetKcal(float timeminute, float? meanhr = null)
    {
        return (K + meanhr ?? musicLength * Q) / 4.184f * timeminute;
    }
}
