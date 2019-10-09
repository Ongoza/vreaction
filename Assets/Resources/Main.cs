using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Main : MonoBehaviour{
    GameObject body;
    private Animator animator;
    private IEnumerator coroutine;
    public Variables variables;
    public Transform Target;
    public string testText = "testextMain";
    public Text TextInput; // user input field
    private string userLang = "English"; // default language
    public float baseLoc = 5.5f; //  Y location for menus
    private float defaultTime = 1f; // time in sec focus on an obj for select
    private float[] trackingTime = new float[] { 30f, 0f }; // [0] time in sec while moving are recording for each action, [1] - current timer
    private int curScene = 0; //  start scene index 
    private Material timedPointer; // pointer on an active object
    private string curfocusObj = ""; // current object in a focus
    private string curfocusType = ""; // current object type in a focus
    private int[] curfocusObjCode = new int[11] {-1,0,0,0,0,0,0,0,0,0,0 }; // a current object descritption in focus
    private float workTime; // timer for user actions
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
    GameObject init_box;
    GameObject cube;
    GameObject root_text;
    int delay = 30;
    // "leftHandForvard","rightHandForvard", "leftHandUp", "rightHandUp", "leftHandDown", "rightHandDown"
    public Dictionary<string, PoseData> listPoses = new Dictionary<string, PoseData>();

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
        // baseLoc = 5.5f;
        body = GameObject.Find("bodyEric");
        // animator = body.GetComponent<Animator>();
        // animator.SetIKPosition(AvatarIKGoal.LeftFoot, new Vector3(0.1f,0.1f,0.1f));
        init_box = GameObject.Find("init_box");
        Rigidbody rb = init_box.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.mass = 0;
        init_box.AddComponent<TTriggerEvent>();
        // cube = GameObject.Find("Cube-glow");
        coroutine = CreateCube();
        StartCoroutine(coroutine);
        variables = new Variables("test");
        string lng = Application.systemLanguage.ToString();
        // ps.Stop;
        // ps.main.startDelay = 5.0f;
        // if (TextData.isLanguge(lng)){userLang = lng;}
        // userLang = "Spanish";

        listPoses["leftHandForvard"] = new PoseData(){ 
            id = "leftHandForvard"
            , tex = Resources.Load<Texture2D>("hand_left")
            , message = TextData.getMessage(userLang, "leftHandForvard")
            , boxDataPos = new Vector3(1.13f,2.87f,3.25f)};
        listPoses["rightHandForvard"] = new PoseData(){ 
            id = "rightHandForvard"
            , tex = Resources.Load<Texture2D>("hand_right")
            , message = TextData.getMessage(userLang, "rightHandForvard")
            , boxDataPos = new Vector3(0.51f,2.87f,3.25f)};
        // ,["rightHandForvard"]=,
        // ,["leftHandUp"]=,
        // ,["rightHandUp"]=,
        // ,["leftHandDown"]=,
        // ,["rightHandDown"]=
        List<string> keyList = new List<string>(bodyParts.Keys);
        GameObject camTimedPointer = GameObject.Find("GvrReticlePointer");
        timedPointer = camTimedPointer.GetComponent<Renderer>().material;
        // foreach(string key in keyList){ bodyParts[key] = FindChild(body.transform,key);}
        // if(bodyParts["upperarm_l"]!= null){
        //     bodyParts["upperarm_l"].Rotate(0, 0, 90, Space.Self);
        //     bodyParts["upperarm_r"].Rotate(0, 0, 90, Space.Self);
        // }
        // Debug.Log(body.transform.RightHand);
        
        animator = body.GetComponent<Animator>();
        // Transform arm = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
        // Debug.Log(arm);
        // arm.LookAt(init_box.transform.position);
        // Vector3 Offset = new Vector3(0,90,90);
        // arm.rotation = arm.rotation * Quaternion.Euler(Offset);
        // obj.Rotate(0,0,90);
        // Quaternion handRotation = Quaternion.LookRotation(objToAimAt.position - new Vector3(0,0,90));
        // animator.SetIKRotationWeight(animator.AvatarIKGoal.RightHand, 1.0f);
        // animator.SetIKRotation(animator.AvatarIKGoal.RightHand, handRotation);

         StartCoroutine(animationBodyPart(animator.GetBoneTransform(HumanBodyBones.LeftUpperArm), new Vector3(0,0,90)));
        // Transform characterHips = animator.GetBoneTransform(HumanBodyBones.Hips); 
        Debug.Log(baseLoc);
        // doKnuckle(bodyParts["hand_l"]);
        // doKnuckle(bodyParts["hand_r"]);
        
        // GameObject msgObj2 = Utility.ShowMessage("selAllCol", "", TextData.getMessage(userLang, "btnStart"), new Vector2(1200, 200), TextAlignmentOptions.Midline, new Vector2(0, 20), this);
        con = new Connection();
        // initPose();
        // drawLines();
    }

    // Update is called once per frame
    void Update(){
        // if(bodyParts["upperarm_l"]!= null){
        //  bodyParts["upperarm_l"].Rotate(0, 1, 0, Space.Self);
        //  bodyParts["upperarm_r"].Rotate(0, 1, 0, Space.Self);
        // }
        // if(trSpeeedMesure){
        //      float movementPerFrame = Vector3.Distance (PreviousFramePosition, transform.position) ;
        //  Speed = movementPerFrame / Time.deltaTime;
        // }
        if (isTimer){ // display selecting pointer
            //Debug.Log("onUpdate main ");
            workTime -= Time.deltaTime;
            if (workTime <= 0) { OnClickTimed();
            }else{ if (timedPointer){ timedPointer.SetFloat("_Angle", (1f - workTime / defaultTime) * 360); } }
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
        // GameObject gm = GameObject.Find(curfocusObj);        
        // if (gm) {
        //     curfocusObjCode[5] = Mathf.RoundToInt(gm.transform.position.x * precisionDec);
        //     curfocusObjCode[6] = Mathf.RoundToInt(gm.transform.position.y * precisionDec);
        //     curfocusObjCode[7] = Mathf.RoundToInt(gm.transform.position.z * precisionDec);
        //     curfocusObjCode[8] = Mathf.RoundToInt(gm.transform.eulerAngles.x * precisionDec);
        //     curfocusObjCode[9] = Mathf.RoundToInt(gm.transform.eulerAngles.y * precisionDec);
        //     curfocusObjCode[10] = Mathf.RoundToInt(gm.transform.eulerAngles.z * precisionDec);
        // }
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
        Debug.Log("clickSelectEvent=" + curfocusType + "="+ curfocusObj);
        switch (curfocusType) {
            case "click": // start next scena
                Debug.Log("click="+ curfocusObj);
                StartCoroutine(Boom(cube));
                break;
            default: Debug.Log("clickSelectEvent not found action for " + name); break;
        }
        OnExitTimed();
    }

      IEnumerator Boom(GameObject gm){
        GameObject myInstance = null;
        StopCoroutine(coroutine);
        Debug.Log(gm);
        if(gm){
            myInstance = Instantiate(Resources.Load("BoomBall")) as GameObject;
            if(gm){ myInstance.transform.localPosition = gm.transform.position; Destroy(gm);}
            // ParticleSystem ps = myInstance.GetComponent<ParticleSystem>();
        }
        yield return new WaitForSeconds(1);
        Debug.Log("touch destoy");
        if(myInstance){Destroy(myInstance);}
    }

     IEnumerator CreateCube(){
        cube = Instantiate(Resources.Load("Cube-glow")) as GameObject;
        cube.transform.position = new Vector3(-6,2,3);
        Vector3 endPos = new Vector3(6,2,3);
        float t = 0;
        while (t <= 0.02f) {
            // Debug.Log(t);
            if(cube){
                t += 0.0001f; // Goes from 0 to 1, incrementing by step each time
                cube.transform.position = Vector3.Lerp(cube.transform.position, endPos, t); // Move objectToMove closer to b
                yield return new WaitForFixedUpdate();
            }
        }
        Debug.Log("self destoy");
        if(cube){ StartCoroutine(Boom(cube)); }
     }

    private void initPose(){
       init_box.transform.position = listPoses["leftHandForvard"].boxDataPos;
       init_box.GetComponent<Renderer>().material.SetTexture("_MainTex", listPoses["leftHandForvard"].tex);
    //    root_text = Utility.ShowMessage(TextData.getMessage(userLang, "selAllCol"), "click", TextData.getMessage(userLang, "btnStart"), new Vector2(1200, 200), TextAlignmentOptions.Midline, new Vector2(0, -20), this);
    root_text = Utility.ShowMessage(listPoses["leftHandForvard"].message, "poseInitReset", TextData.getMessage(userLang, "btn_poseInitReset"), new Vector2(1200, 200), TextAlignmentOptions.Midline, new Vector2(0, -20), this);
    }
    
    // public void triggerCollision(Collider cInfo){
    //     Debug.Log(cInfo.gameObject.name+" touch to "+cInfo.name);
    // }
    public void triggerCollision(string[] names){
    //    Debug.Log(cInfo.gameObject.name+" 2touch to "+cInfo.name+" "+cInfo.ToString()+" ");
    Debug.Log(names[1]+" touch to "+names[0]);
    }

    IEnumerator animationBodyPart(Transform pos, Vector3 endAngles){
        Vector3 startAngles = pos.localEulerAngles;
        float currCountdownValue = 100f;
        // Transform transform = part.transform;
        float steps = 100;
        float stepX = (endAngles.x -startAngles.x)/steps;
        float stepY = (endAngles.y -startAngles.y)/steps;
        float stepZ = (endAngles.z -startAngles.z)/steps;
        Quaternion step = Quaternion.Euler(1,0,1);
        Debug.Log("coron = "+steps+", "+startAngles+", "+ endAngles);
        while (steps > 0){
            // pos.localRotation = step;
            // pos.Rotate(step.x, step.y, step.z, Space.Self);
            // pos.rotation = pos.rotation * step;
            pos.Rotate(0,0,stepZ,Space.Self);
            pos.Rotate(1,0,0,Space.Self);
            steps--;
            yield return null;
        }
    }
    void OnAnimatorIK(int layerIndex){
        // if (enabledIK){
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
            animator.SetIKRotation(AvatarIKGoal.RightHand, init_box.transform.rotation);
 
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
            animator.SetIKPosition(AvatarIKGoal.RightHand, init_box.transform.position);
        // }else{
        //     animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.0f);
        //     animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0.0f);
        // }
    // void onAnimatorIK(){
    //     // animator.SetIKRotationWeight(init_box.transform,1f);
    //     animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,1f);
    //     animator.SetIKPosition(AvatarIKGoal.LeftHand,init_box.transform.position);
    //     // animator.SetIKRotation();
    }
    private void drawLines(){
        // Gizmos.DrawCube(transform.position, new Vector3(2, 1, 1));
        Vector3[] positions = new Vector3[]{
            new Vector3(0,0,0)
            ,new Vector3(1,0,0)
            ,new Vector3(1,1,0)
            ,new Vector3(1,2,0)
            ,new Vector3(1,2,1)
        };
    // Gizmos.DrawLine(positions[0],positions[1]);
        // Handles.DrawPolyLine(positions);
    }
    void OnApplicationQuit()
    {
        con.closeConnection();
    }
}


public class PoseData{
    public string id;
    public string message;
    public Texture tex;
    public Vector3 boxDataPos;
    public Vector3 boxDataRot;
    public float[] handData = new float[8];
}
