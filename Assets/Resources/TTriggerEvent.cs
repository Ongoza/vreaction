using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    void OnCollisionEnter(Collision collisionInfo){
            Debug.Log("Detected collision between " + gameObject.name + " and " + collisionInfo.collider.name);
            Debug.Log("There are " + collisionInfo.contacts.Length + " point(s) of contacts");
            Debug.Log("Their relative velocity is " + collisionInfo.relativeVelocity);
        }
}
