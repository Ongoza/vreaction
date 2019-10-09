using System;
using System.Text;
using System.Threading;
using System.Net.WebSockets;
using UnityEngine;
// /home/os/Android/Sdk/platform-tools/adb -s "0123456789ABCDEF" forward "tcp:34999" "localabstract:Unity-com.Ongoza.DianasSportClub"
public class Connection{
    Uri u = new Uri("ws://192.168.1.102:9090/data");
    ClientWebSocket cws = null;
    ArraySegment<byte> buf = new ArraySegment<byte>(new byte[1024]);
    ArraySegment<byte> request = new ArraySegment<byte>(Encoding.UTF8.GetBytes("0"));
    private float currentFrameTime;
    private bool isSendRequest = true; 
    public Connection() { 
        Connect(); 
        }

    async void Connect() {
        cws = new ClientWebSocket();
        try {
            await cws.ConnectAsync(u, CancellationToken.None);
            if (cws.State == WebSocketState.Open) Debug.Log("connected");
            SendRequest();
            GetData();
        }
        catch (Exception e) { Debug.Log("woe " + e.Message); }
    }

    public async void SendRequest() {
        currentFrameTime = Time.realtimeSinceStartup;
        await cws.SendAsync(request, WebSocketMessageType.Binary, true, CancellationToken.None);
    }

    async void GetData() {
        WebSocketReceiveResult r = await cws.ReceiveAsync(buf, CancellationToken.None);
        string tmpData = Encoding.UTF8.GetString(buf.Array, 0, r.Count);
        Debug.Log("Got: " + tmpData);
        // ServerData data = JsonUtility.FromJson<ServerData>(tmpData);
        // Debug.Log(JsonUtility.ToJson(data));
        Debug.Log(Time.realtimeSinceStartup-currentFrameTime);
        if(isSendRequest){
            SendRequest();
            GetData();
        }
    }
    public void closeConnection(){
        if(cws!=null){
            isSendRequest = false; 
            cws.Dispose();
            cws =null;
        }
    }
}

[System.Serializable] public class ServerData{
    public float[] g;
    public float[] s1;
    public float[] s2;
}