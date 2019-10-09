using UnityEngine;

public class TTriggerEvent : MonoBehaviour{
    void OnTriggerEnter(Collider cInfo){
        // Debug.Log("Detected collision between " + gameObject.name + " and " + cInfo.name);
        // main = Camera.main.GetComponent<Main>();
        string[] data = {gameObject.name, cInfo.name};
        Camera.main.SendMessage("triggerCollision", data);
        // main.triggerCollision(cInfo);
        // Debug.Log("There are " + cInfo.contacts.Length + " point(s) of contacts");
        // Debug.Log("Their relative velocity is " + cInfo.relativeVelocity);
        }
}
