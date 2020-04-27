using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIOClient;

public class hrscript : MonoBehaviour
{
    // Start is called before the first frame update
    string url = "http://127.0.0.1:999/";
    public static Client Socket { get; private set; }
    public Text txt;

    void Awake()
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
    }


    void Start()
    {
        txt = GameObject.Find("HRtext").GetComponent<Text>();
        txt.text = "called";
        Debug.Log("Start called");
        Socket.Emit("Msg", "heeellllo");
        Socket.On("MsgRes", (data) =>
        {
            Debug.Log(data.Json.args[0]);
            txt.text = "heart rate changed: " + data.Json.args[0];
        });
    }

    public void test()
    {
        //        Socket.Emit
    }

    // Update is called once per frame
    void Update()
    {
        txt = GameObject.Find("HRtext").GetComponent<Text>();
        Socket.On("hr", (data) =>
        {
            Debug.Log(data.Json.args[0]);
            txt.text = "heart rate changed: " + data.Json.args[0];
        });
    }
}