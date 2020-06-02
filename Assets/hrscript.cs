using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIOClient;
using System.Diagnostics;
using TMPro;

public class hrscript : MonoBehaviour
    {
    private float thres;
    private string datastring;
    private float dataint;
    private int datashow;
    List<string> buffer = new List<string>();
    public bool hrOverride = false;
    public float hrOverrideValue = 100;
    // Start is called before the first frame update
    //        string url = "http://127.0.0.1:999/";
    //       public static Client Socket { get; private set; }

        /*        void Awake()
                {
                    Debug.Log("Awake called");
                    Socket = new Client(url);
                    Socket.Opened += SocketOpened;

                    Socket.Connect();
                    Debug.Log("Socket connect function");
                }
                private void SocketOpened(object sender, System.EventArgs e)
                {
                    Debug.Log("Socket Opened");
                }*/




        void Start()
        {

        /*            txt = GameObject.Find("HRtext").GetComponent<Text>();
                    txt.text = "called";
                    Debug.Log("Start called");
                    Socket.Emit("Msg", "heeellllo");
                    Socket.On("MsgRes", (data) =>
                    {
                        Debug.Log(data.Json.args[0]);
                    });*/
        /*            SocketManager.Socket.On("MsgRes", (data) =>
                    {
                        Debug.Log(data.Json.args[0]);
                    });*/
            
                SocketManager.Socket.On("hr", (data) =>
                {
                    if (hrOverride) return;
                    UnityEngine.Debug.Log("hr raw: " + data.Json.args[0]);
//                        txt.text = "heart rate changed: " + data.Json.args[0];
                    datastring = data.Json.args[0].ToString();
                    datashow = (Convert.ToInt32(datastring));
                    dataint = (Convert.ToInt32(datastring));
                    thres = dataint;
                    UnityEngine.Debug.Log("hr: " + thres);
                    NoteSpawner.hr = Convert.ToSingle(thres);
                });
                    
    }
    void Update()
    {
        if (hrOverride)
            NoteSpawner.hr = hrOverrideValue;
    }

    // Update is called once per frame
}