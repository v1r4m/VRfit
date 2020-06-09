using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EstCal : MonoBehaviour
{
    TextMeshProUGUI tmp;
    ArgsGetter ag;
    [SerializeField]
    TextMeshProUGUI subText;
    
    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        ag = FindObjectOfType<ArgsGetter>();
    }

    // Update is called once per frame
    void Update()
    {
        ag.UpdateValue();
        tmp.text = "Estimated Calorie: " + ArgsGetter.GetKcal(ArgsGetter.TargetHR) + "Kcal";
        switch (ArgsGetter.Intensity)
        {
            case 1:
                subText.text = "저강도로 오래 운동을 하면 체중 감소에 좋습니다.";
                break;
            case 2:
                subText.text = "중강도 운동은 심장과 근육을 단련하는 데에 좋습니다.";
                break;
            case 3:
                subText.text = "고강도 운동은 심장과 근육을 더욱 더 단련합니다.\n주의해서 플레이하세요.";
                break;
            default:
                subText.text = "";
                break;
        }
        
    }
}

