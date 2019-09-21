using System;
using System.Text;
using System.Threading;
using System.Net.WebSockets;
using UnityEngine;
// /home/os/Android/Sdk/platform-tools/adb -s "0123456789ABCDEF" forward "tcp:34999" "localabstract:Unity-com.Ongoza.DianasSportClub"
public class Connection{
    Uri u = new Uri("ws://192.168.1.101:9090/data");
    ClientWebSocket cws = null;
    ArraySegment<byte> buf = new ArraySegment<byte>(new byte[1024]);

    public Connection() { 
        Connect(); 
        }

    async void Connect() {
        cws = new ClientWebSocket();
        try {
            await cws.ConnectAsync(u, CancellationToken.None);
            if (cws.State == WebSocketState.Open) Debug.Log("connected");
            SayHello();
            GetStuff();
        }
        catch (Exception e) { Debug.Log("woe " + e.Message); }
    }

    public async void SayHello() {
        ArraySegment<byte> b = new ArraySegment<byte>(Encoding.UTF8.GetBytes("hello"));
        Debug.Log("sent hello");
        await cws.SendAsync(b, WebSocketMessageType.Text, true, CancellationToken.None);
    }

    async void GetStuff() {
        WebSocketReceiveResult r = await cws.ReceiveAsync(buf, CancellationToken.None);
        string tmpData = Encoding.UTF8.GetString(buf.Array, 0, r.Count);
        Debug.Log("Got: " + tmpData);
        ServerData data = JsonUtility.FromJson<ServerData>(tmpData);
        Debug.Log(JsonUtility.ToJson(data));
        // GetStuff();
    }
}

[System.Serializable] public class ServerData{
    public float[] g;
    public float[] s1;
    public float[] s2;
}