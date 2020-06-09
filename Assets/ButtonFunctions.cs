using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctions : MonoBehaviour
{
    public enum BtnType
    {
        None,
        LoadScene,
        LoadGameScene,
        AddToCounter,
        SetText,
        ToggleGender
    }
    public BtnType function;

    public int arg_int = 0;
    public string arg_str = "";
    public Transform arg_transform;

    public void OnClick()
    {
        switch (function)
        {
            case BtnType.LoadScene:
                UnityEngine.SceneManagement.SceneManager.LoadScene(arg_str);
                break;
            case BtnType.None:
                break;
            case BtnType.AddToCounter:
                arg_transform.GetComponent<Counter>().Value += arg_int;
                break;
            case BtnType.SetText:
                arg_transform.GetComponent<TMPro.TextMeshProUGUI>().text = arg_str;
                break;
            case BtnType.ToggleGender:
                var tmp = arg_transform.GetComponent<TMPro.TextMeshProUGUI>();
                if(tmp.text == "M")
                    tmp.text = "F";
                else
                    tmp.text = "F";
                break;

            case BtnType.LoadGameScene:
                DoLoadScene();
                break;
        }
    }

    void DoLoadScene()
    {
        


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
