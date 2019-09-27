using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTriggerEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start colision");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider cInfo){
        Debug.Log("Ok trigger");
            Debug.Log("Detected collision between " + gameObject.name + " and " + cInfo.name);
            // Debug.Log("There are " + cInfo.contacts.Length + " point(s) of contacts");
            // Debug.Log("Their relative velocity is " + cInfo.relativeVelocity);
        }

    void OnCollisionEnter(Collision cInfo){
        Debug.Log("Ok collision");
            // Debug.Log("Detected collision between " + gameObject.name + " and " + cInfo.name);
            // Debug.Log("There are " + cInfo.contacts.Length + " point(s) of contacts");
            // Debug.Log("Their relative velocity is " + cInfo.relativeVelocity);
        }
}
