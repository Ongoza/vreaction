using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Main : MonoBehaviour{
    GameObject body;
    public Variables variables;
    public Text TextInput; // user input field
    private string userLang = "English"; // default language
    public float baseLoc = 2; //  Y location for menus
    private float defaultTime = 1f; // time in sec focus on an obj for select
    private float[] trackingTime = new float[] { 30f, 0f }; // [0] time in sec while moving are recording for each action, [1] - current timer
    private int curScene = 0; //  start scene index 
    private Material timedPointer; // pointer on an active object
    private string curfocusObj = ""; // current object in a focus
    private string curfocusType = ""; // current object type in a focus
    private int[] curfocusObjCode = new int[11] {-1,0,0,0,0,0,0,0,0,0,0 }; // a current object descritption in focus
    private float workTime; // timer for user actions
    private bool  animVR = false; // tr for update if need an animation
    private bool isTimer = false; // display select cursor 
    public int precisionDec = 1000; // number dec after point in head movement control saved action: 1 - 0, 10-0.0, 100 - 0.00, 1000 -0.000
        public int[,] testsConfig = new int[2,6] { // testsConfig[x,0] - (0-4) type index of an object for search in the first test
        { 4, 4, 5, 0, 0, 0 }, // testsConfig[x,1] - (0-7) index of a selected object color
        { 4, 4, 5, 0, 0, 20 }}; // testsConfig[x,2] total number of objects for selection in tests
                               // testsConfig[x,3] total number of right founeded objects in a selection in test
                               // testsConfig[x,4] total number of all founeded objects in a selection test
                               // testsConfig[curTestIndex,5] time limit for the second test in sec. 0 - unlimit
    public int curTestIndex = 0; //a current test number
    Connection con;
    GameObject testObj;
    int delay = 30;

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
        variables = new Variables("test");
        string lng = Application.systemLanguage.ToString();
        // if (TextData.isLanguge(lng)){userLang = lng;}
        // userLang = "Spanish";
        List<string> keyList = new List<string>(bodyParts.Keys);
        foreach(string key in keyList){ bodyParts[key] = FindChild(body.transform,key);}
        if(bodyParts["upperarm_l"]!= null){
            bodyParts["upperarm_l"].Rotate(0, 0, 90, Space.Self);
            bodyParts["upperarm_r"].Rotate(0, 0, 90, Space.Self);
        }
        // Transform characterHips = animator.GetBoneTransform(HumanBodyBones.Hips); 
        Debug.Log(bodyParts);
        doKnuckle(bodyParts["hand_l"]);
        doKnuckle(bodyParts["hand_r"]);
        GameObject msgObj2 = Utility.ShowMessage(TextData.getMessage(userLang, "selAllCol"), "click", TextData.getMessage(userLang, "btnStart"), new Vector2(1200, 200), TextAlignmentOptions.Midline, new Vector2(0, -20), this);
        // GameObject msgObj2 = Utility.ShowMessage("selAllCol", "", TextData.getMessage(userLang, "btnStart"), new Vector2(1200, 200), TextAlignmentOptions.Midline, new Vector2(0, 20), this);
        // con = new Connection();
    }

    // Update is called once per frame
    void Update(){
        // if(bodyParts["upperarm_l"]!= null){
        //  bodyParts["upperarm_l"].Rotate(0, 1, 0, Space.Self);
        //  bodyParts["upperarm_r"].Rotate(0, 1, 0, Space.Self);
        // }
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
    void doKnuckle(Transform hand){
        if(hand!=null){ 
            hand.GetChild(0).Rotate(0, 0, -80, Space.Self); hand.GetChild(0).GetChild(0).Rotate(0, 0, -150, Space.Self);
            hand.GetChild(1).Rotate(0, 0, -80, Space.Self);hand.GetChild(1).GetChild(0).Rotate(0, 0, -150, Space.Self);
            hand.GetChild(2).Rotate(0, 0, -80, Space.Self);hand.GetChild(2).GetChild(0).Rotate(0, 0, -150, Space.Self);
            hand.GetChild(3).Rotate(0, 0, -80, Space.Self);hand.GetChild(3).GetChild(0).Rotate(0, 0, -150, Space.Self);
            hand.GetChild(4).GetChild(0).Rotate(0, 0, -80, Space.Self);hand.GetChild(3).GetChild(0).GetChild(0).Rotate(0, 0, -120, Space.Self);
            // GameObject plane  = GameObject.CreatePrimitive(PrimitiveType.Plane);
            // plane.transform.SetParent(hand);
            // plane.transform.Rotate(0,0,-90);
            // plane.transform.position = new Vector3(0, 0.5f, 0);
            // plane.transform.localScale = new Vector3(0.005f, 1, 0.01f);
        }
        

    }

           // on the cusror over an active object in a scena
    public void OnEnterTimed(string type, string name, bool isButton){
        Debug.Log("onEnterTimed main " + name + "=" + type+" butt="+ isButton);
        Utility.logDebug("EnterTimed");
        isTimer = true;
        workTime = defaultTime;
        if (timedPointer) { timedPointer.SetFloat("_Angle", 0); }
        curfocusObj = name;
        curfocusType = type;
        string[] arrName = name.Split('_');
        curfocusObjCode = new int[11] { -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int.TryParse(arrName[0], out curfocusObjCode[0]);        
        if (!isButton){
            int.TryParse(arrName[1], out curfocusObjCode[1]);
            int.TryParse(arrName[2], out curfocusObjCode[2]);
            curfocusObjCode[3] = testsConfig[curTestIndex, 0];
            curfocusObjCode[4] = testsConfig[curTestIndex, 1];            
        }
        GameObject gm = GameObject.Find(curfocusObj);        
        if (gm) {
            curfocusObjCode[5] = Mathf.RoundToInt(gm.transform.position.x * precisionDec);
            curfocusObjCode[6] = Mathf.RoundToInt(gm.transform.position.y * precisionDec);
            curfocusObjCode[7] = Mathf.RoundToInt(gm.transform.position.z * precisionDec);
            curfocusObjCode[8] = Mathf.RoundToInt(gm.transform.eulerAngles.x * precisionDec);
            curfocusObjCode[9] = Mathf.RoundToInt(gm.transform.eulerAngles.y * precisionDec);
            curfocusObjCode[10] = Mathf.RoundToInt(gm.transform.eulerAngles.z * precisionDec);
        }
        //Debug.Log(name+"="+string.Join(";", curfocusObjCode));        
    }

    // on the cusror move out of over an active object in a scena
    public void OnExitTimed(){
        Debug.Log("ExitTimed main");
        Utility.logDebug("ExitTimed");
        isTimer = false;
        curfocusObj = "";
        curfocusType = "";
        curfocusObjCode = new int[11] { -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        workTime = defaultTime;
        if (timedPointer) { timedPointer.SetFloat("_Angle", 360); }
        trackingTime[1] = 0f;
    }

    public void OnClickTimed(){ClickSelectEvent();}

    // on the cusror timed click on an active object in a scena
    private void ClickSelectEvent(){
        isTimer = false; 
    }
}
