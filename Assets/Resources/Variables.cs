using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variables : MonoBehaviour
{
    private string serverUrl = "";
    public string deviceUUID = "unity";
    private bool connEnable = false;
    public string ip = "local";
    public string ipInfo = "";
    private string lang = "en";
    private List<string> perms = new List<string>() {};
    public Material matFont;
    public int fontSize;
    public Font defaultFont;
    public Sprite uisprite;

    public Variables(string serverString){
        serverUrl = serverString;        
        deviceUUID = SystemInfo.deviceUniqueIdentifier.ToString();
        //FB.Init(InitCallback);
    }
}
