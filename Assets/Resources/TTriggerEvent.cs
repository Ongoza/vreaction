using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTriggerEvent : MonoBehaviour{
    public Main main;
    // Start is called before the first frame update
    void Start(){ main = Camera.main.GetComponent<Main>();}

    void OnTriggerEnter(Collider cInfo){
        // Debug.Log("Detected collision between " + gameObject.name + " and " + cInfo.name);
        main.triggerCollision(gameObject,cInfo.name);
        // Debug.Log("There are " + cInfo.contacts.Length + " point(s) of contacts");
        // Debug.Log("Their relative velocity is " + cInfo.relativeVelocity);
        }
}
