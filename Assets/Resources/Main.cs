using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour{
    Connection con ;
    GameObject body;
    Dictionary<string, Transform> bodyParts = new Dictionary<string, Transform>(){
        { "hip", null}
        ,{ "spine_01", null}
        ,{ "spine_02", null}
        ,{ "spine_03", null}
        ,{ "neck", null}
        ,{ "head", null}
        ,{ "shoulder_l", null}
        ,{ "upperarm_l", null}
        ,{ "lowerarm_l", null}
        ,{ "hand_l", null}
        ,{ "shoulder_r", null}
        ,{ "upperarm_r", null}
        ,{ "lowerarm_r", null}
        ,{ "hand_r", null}
        ,{ "upperleg_l", null}
        ,{ "lowerleg_l", null}
        ,{ "foot_l", null}
        ,{ "upperleg_r", null}
        ,{ "lowerleg_r", null}
        ,{ "foot_r", null}
    };
    
    // Start is called before the first frame update
    void Start(){
        // animator = GetComponent<Animator>();
        body = GameObject.Find("bodyEric");
        List<string> keyList = new List<string>(bodyParts.Keys);
        foreach(string key in keyList){ bodyParts[key] = FindChild(body.transform,key);}
        if(bodyParts["upperarm_l"]!= null){
            bodyParts["upperarm_l"].Rotate(0, 0, 90, Space.Self);
            bodyParts["upperarm_r"].Rotate(0, 0, 90, Space.Self);
        }
        // Transform characterHips = animator.GetBoneTransform(HumanBodyBones.Hips); 
        Debug.Log(bodyParts);
        // con = new Connection();
    }

    // Update is called once per frame
    void Update(){
        if(bodyParts["upperarm_l"]!= null){
         bodyParts["upperarm_l"].Rotate(0, 1, 0, Space.Self);
         bodyParts["upperarm_r"].Rotate(0, 1, 0, Space.Self);
        }
    }

    Transform FindChild(Transform aParent, string aName){
        if (aParent == null) return null;
        Transform result = aParent.Find(aName);
        if (result != null) return result;
            foreach (Transform child in aParent){
                result = FindChild(child, aName);
                if (result != null) return result;
             }
             return null;
         }
}
