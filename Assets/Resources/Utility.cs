using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Utility {    
    
    public static void logDebug(string str){
        Debug.Log(str);
    }

    // message dialog
    public static GameObject ShowMessage(string msg, string action, string actionLabel, Vector2 size, TextAlignmentOptions anchor, Vector2 startLoc, Main main){
        //logDebug("ShowMessage");
        Debug.Log(main.baseLoc);
        GameObject rootObj = new GameObject("rootObj");
        rootObj.transform.position = new Vector3(0,main.baseLoc,-1);
        GameObject rootCanvas = CreateCanvas("rootMenu", new Vector3(0, 0, 0), size);
        rootCanvas.transform.SetParent(rootObj.transform,false);
        GameObject panel = new GameObject("Panel");
        panel.AddComponent<CanvasRenderer>();
        Image i = panel.AddComponent<Image>();
        // i.color = new Vector4(1, 1, 1, 0.9f);
        i.color = new Vector4(0.2f, 0.2f, 0.2f, 0.7f);
        // i.sprite = main.variables.uisprite;
        // i.type = Image.Type.Sliced;
        i.material = main.variables.matText;
        RectTransform panelTransform = panel.GetComponent<RectTransform>();
        panel.transform.SetParent(rootCanvas.transform, true);
        panelTransform.localScale = new Vector3(1f, 1f, 1f);
        panelTransform.localPosition = new Vector3(0, 40, 0);
        panelTransform.sizeDelta = size;        // Debug.Log(size);
        
        CreateText(panelTransform, startLoc, new Vector2(size.x - 150, size.y), msg, 36, 0, anchor, main);
        if (!action.Equals("")) { 
            CreateButton(panelTransform, "Button", actionLabel, action, "0_10_10", new Vector3(0, -40 - size.y / 2, 0), new Vector2(300, 60), main);
        }
        panelTransform.rotation = Quaternion.AngleAxis(-180, Vector3.up);        
        // showTutorialsMenu(rootMenu);
        return rootObj;
    }

    // canavas for text messages
    public static GameObject CreateCanvas(string name, Vector3 loc, Vector2 size){
        //logDebug("CreateCanvas");
        GameObject objCanvas = new GameObject(name);
        Canvas c = objCanvas.AddComponent<Canvas>();
        c.renderMode = RenderMode.WorldSpace;
        objCanvas.AddComponent<CanvasScaler>();
        RectTransform NewCanvasRect = objCanvas.GetComponent<RectTransform>();
        GvrPointerGraphicRaycaster action = objCanvas.AddComponent<GvrPointerGraphicRaycaster>();
        //action.enabled = false;
        NewCanvasRect.localPosition = loc;
        NewCanvasRect.sizeDelta = size;
        NewCanvasRect.localScale = new Vector3(0.01f, 0.01f, 1f);
        return objCanvas;
    }

    // text object
    public static GameObject CreateText(Transform parent, Vector2 loc, Vector2 size, string message, int fontSize, int fontStyle, TextAlignmentOptions achor, Main main){
        // logDebug("CreateText");
        Debug.Log(size);
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(parent);
        RectTransform trans = textObject.AddComponent<RectTransform>();
        trans.anchoredPosition3D = new Vector3(0, 0, 0);
        trans.anchoredPosition = loc;
        trans.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        trans.localPosition.Set(0, 0, 0);
        textObject.AddComponent<CanvasRenderer>();
        // Text text = textObject.AddComponent<Text>();
        // text.supportRichText = true;
        // text.material = main.variables.matFont;
        
        // if (fontStyle == 1) { text.fontStyle = FontStyle.Bold; }
        // text.font = main.variables.defaultFont;
        
        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
        trans.sizeDelta = size;
        text.text = message;
        text.fontSize = fontSize;
        text.alignment = achor;
        //text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.color = new Color(1f, 1f, 1f);
        return textObject;
    }

    // button object
    public static GameObject CreateButton(Transform parent, string name, string label, string action1, string action2, Vector3 loc, Vector2 size, Main main){
        //logDebug("CreateButton 1");
        GameObject bt0 = new GameObject(name);
        RectTransform br = bt0.AddComponent<RectTransform>();
        br.sizeDelta = size;
        Image img = bt0.AddComponent<Image>();
        img.sprite = main.variables.uisprite;
        img.color = new Vector4(0f, 0.5f, 1f, 1);
        img.material = main.variables.matText;
        // img.type = Image.Type.Sliced;
        Button bt = bt0.AddComponent<Button>();
        bt0.transform.SetParent(parent, true);
        bt0.transform.localPosition = loc;
        // ColorBlock cb = bt0.colors;
        // cb.normalColor = new Vector4(0f, 0.4f, 1f, 1);
        // cb.highlightedColor = new Vector4(0f, 0.6f, 1f, 1);
        // bt0.colors = cb;
        //bt0.transform.rotation = Quaternion.AngleAxis(-180, Vector3.up);
        bt0.transform.localScale = new Vector3(1, 1, 1);
        CreateText(bt.transform, new Vector2(0, 0), size, label, 40, 1, TextAlignmentOptions.Midline, main);
        bt.onClick.AddListener(main.OnClickTimed);
        EventTrigger be = bt0.AddComponent<EventTrigger>();
        EventTrigger.Entry entryEnterGaze = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        entryEnterGaze.callback.AddListener((eventData) => { main.OnEnterTimed(action1, action2, true); });
        be.triggers.Add(entryEnterGaze);
        EventTrigger.Entry entryExitGaze = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        entryExitGaze.callback.AddListener((eventData) => { main.OnExitTimed(); });
        be.triggers.Add(entryExitGaze);
        //logDebug("CreateButton end");
        return bt0;
    }

    //  a text input dialog
    public static GameObject ShowKeyboard(string userLang, string Label, Main main){ // show keyboard input
        int len = 12; int width = 60; int startPosX = -342; int startPosY = 125; int i = 0; int j = 0;
        GameObject rootObj = CreateCanvas("rootKeyoard", new Vector3(0, -3, 12), new Vector2(770, 350));
        GameObject panel = new GameObject("Panel");
        panel.AddComponent<CanvasRenderer>();
        Image img = panel.AddComponent<Image>();
        img.color = new Vector4(1, 1, 1, 1f);
        // img.sprite = main.variables.uisprite;
        img.type = Image.Type.Sliced;
        RectTransform panelTransform = panel.GetComponent<RectTransform>();
        panel.transform.SetParent(rootObj.transform, true);
        panelTransform.localScale = new Vector3(1f, 1f, 1f);
        panelTransform.localPosition = new Vector3(0, 0, 0);
        panelTransform.sizeDelta = new Vector2(770, 330);
        Dictionary<string, string> keyDictionary = TextData.getKeys();
        foreach (KeyValuePair<string, string> item in keyDictionary){
            float delta = 0;
            if (item.Key.Length > 1) { width = 60 * 2; i += 1; delta = 30; }
            if (i >= len) { j++; i = 0; }
            GameObject btKey = CreateButton(panelTransform, item.Key, item.Value, "Keyboard", item.Key, new Vector3(startPosX + i * (60 + 2) - delta, startPosY - j * (60 + 2), 0), new Vector2(width, 60), main);
            Image imgKey = btKey.GetComponent<Image>();
            imgKey.color = new Vector4(0.5f, 0.5f, 0.8f, 1);
            i++;
        }

        CreateButton(rootObj.transform, "Enter", TextData.getMessage(userLang, "btnNext"), "Enter"+Label, "0_10_10", new Vector3(0, -230, 0), new Vector2(150, 60), main);
        GameObject InputMsg = new GameObject("InputMsg");
        InputMsg.transform.SetParent(rootObj.transform);
        RectTransform brInputMsg = InputMsg.AddComponent<RectTransform>();
        brInputMsg.localScale = new Vector3(1f, 1f, 1f);
        brInputMsg.sizeDelta = new Vector2(800, 240);
        brInputMsg.localPosition = new Vector3(0, 360, 0);
        Image imgInputMsg = InputMsg.AddComponent<Image>();
        imgInputMsg.sprite = main.variables.uisprite;
        imgInputMsg.color = new Vector4(0.8f, 0.8f, 0.8f, 1);
        imgInputMsg.type = Image.Type.Sliced;
        CreateText(brInputMsg, new Vector2(0, 1f), new Vector2(800, 180), TextData.getMessage(userLang, "msg"+Label), 40, 1, TextAlignmentOptions.Midline, main);
        GameObject Input = new GameObject("Input");
        Input.transform.SetParent(rootObj.transform);
        RectTransform brInput = Input.AddComponent<RectTransform>();
        brInput.localScale = new Vector3(1f, 1f, 1f);
        brInput.sizeDelta = new Vector2(800, 50);
        brInput.localPosition = new Vector3(0, 200, 0);
        Image imgInput = Input.AddComponent<Image>();
        imgInput.sprite = main.variables.uisprite;
        imgInput.color = new Vector4(0.9f, 0.9f, 0.9f, 1);
        imgInput.type = Image.Type.Sliced;
        GameObject InputText = CreateText(brInput, new Vector2(0, -0.5f), new Vector2(800, 50), "", 40, 1, TextAlignmentOptions.Midline, main);
        main.TextInput = InputText.GetComponent<Text>();
        return rootObj;
    }

    public static GameObject ShowDigitsKeyboard(string userLang, string Label, Main main)
    { // show digits keyboard input
        int len = 12; int width = 60; int startPosX = -250; int startPosY = 90; int i = 0; int j = 0;
        GameObject rootObj = CreateCanvas("rootKeyoard", new Vector3(0, -3, 12), new Vector2(770, 150));
        GameObject panel = new GameObject("Panel");
        panel.AddComponent<CanvasRenderer>();
        Image img = panel.AddComponent<Image>();
        img.color = new Vector4(1, 1, 1, 1f);
        img.sprite = main.variables.uisprite;
        img.type = Image.Type.Sliced;
        RectTransform panelTransform = panel.GetComponent<RectTransform>();
        panel.transform.SetParent(rootObj.transform, true);
        panelTransform.localScale = new Vector3(1f, 1f, 1f);
        panelTransform.localPosition = new Vector3(0, 70, 0);
        panelTransform.sizeDelta = new Vector2(770, 130);
        Dictionary<string, string> keyDictionary = new Dictionary<string, string>();
        float delta = 30; 
        for (i = 0; i < 10; i++){            
            GameObject btKey = CreateButton(panelTransform, i.ToString(), i.ToString(), "KeyboardDigit", i.ToString(), new Vector3(startPosX + i * (60 + 2) - delta, 0, 0), new Vector2(width, 60), main);
            Image imgKey = btKey.GetComponent<Image>();
            imgKey.color = new Vector4(0.5f, 0.5f, 0.8f, 1);
        }
        CreateButton(rootObj.transform, "Del", TextData.getMessage(userLang, "btnDelete"), "KeyboardDigit", "DEL", new Vector3(240, 170, 0), new Vector2(220, 60), main);
        GameObject InputMsg = new GameObject("InputMsg");
        InputMsg.transform.SetParent(rootObj.transform);
        RectTransform brInputMsg = InputMsg.AddComponent<RectTransform>();
        brInputMsg.localScale = new Vector3(1f, 1f, 1f);
        brInputMsg.sizeDelta = new Vector2(800, 190);
        brInputMsg.localPosition = new Vector3(0, 360, 0);
        Image imgInputMsg = InputMsg.AddComponent<Image>();
        imgInputMsg.sprite = main.variables.uisprite;
        imgInputMsg.color = new Vector4(0.8f, 0.8f, 0.8f, 1);
        imgInputMsg.type = Image.Type.Sliced;
        CreateText(brInputMsg, new Vector2(0, 1f), new Vector2(800, 180), TextData.getMessage(userLang, "msg" + Label), 40, 1, TextAlignmentOptions.Midline, main);

        GameObject InputMsg2 = new GameObject("InputMsg2");
        InputMsg2.transform.SetParent(rootObj.transform);
        RectTransform brInputMsg2 = InputMsg2.AddComponent<RectTransform>();
        brInputMsg2.localScale = new Vector3(1f, 1f, 1f);
        brInputMsg2.sizeDelta = new Vector2(500, 60);
        brInputMsg2.localPosition = new Vector3(-120, 235, 0);
        Image imgInputMsg2 = InputMsg2.AddComponent<Image>();
        imgInputMsg2.sprite = main.variables.uisprite;
        imgInputMsg2.color = new Vector4(0.8f, 0.8f, 0.8f, 1);
        imgInputMsg2.type = Image.Type.Sliced;

        CreateText(brInputMsg2, new Vector2(-160, 0), new Vector2(100, 60), TextData.getMessage(userLang, "msgYear"), 40, 1, TextAlignmentOptions.Midline, main);
        CreateText(brInputMsg2, new Vector2(0, 0), new Vector2(100, 60), TextData.getMessage(userLang, "msgMonth"), 40, 1, TextAlignmentOptions.Midline, main);
        CreateText(brInputMsg2, new Vector2(160, 0), new Vector2(100, 60), TextData.getMessage(userLang, "msgDay"), 40, 1, TextAlignmentOptions.Midline, main);

        GameObject Input = new GameObject("Input");
        Input.transform.SetParent(rootObj.transform);
        RectTransform brInput = Input.AddComponent<RectTransform>();
        brInput.localScale = new Vector3(1f, 1f, 1f);
        brInput.sizeDelta = new Vector2(500, 50);
        brInput.localPosition = new Vector3(-120, 170, 0);
        Image imgInput = Input.AddComponent<Image>();
        imgInput.sprite = main.variables.uisprite;
        imgInput.color = new Vector4(0.9f, 0.9f, 0.9f, 1);
        imgInput.type = Image.Type.Sliced;
        GameObject InputText = CreateText(brInput, new Vector2(0, -0.5f), new Vector2(400, 50), "XX         XX         XX", 40, 1, TextAlignmentOptions.Midline, main);
        main.TextInput = InputText.GetComponent<Text>();
        return rootObj;
    }

    // select dialog object
    public static GameObject ShowDialog(string msg, string notes, string action, string actionLabel0, string actionLabel1, Vector2 size, TextAlignmentOptions anchor, Vector2 startLoc, int baseLoc, Main main){
        //logDebug("ShowMessage");
        GameObject rootObj = new GameObject("rootObj");
        rootObj.transform.position = new Vector3(0, baseLoc, 12);
        GameObject rootCanvas = CreateCanvas("rootMenu", new Vector3(0, 0, 0), size);
        rootCanvas.transform.SetParent(rootObj.transform, false);
        GameObject panel = new GameObject("Panel");
        panel.AddComponent<CanvasRenderer>();
        Image i = panel.AddComponent<Image>();
        i.color = new Vector4(1, 1, 1, 0.9f);
        i.sprite = main.variables.uisprite;
        i.type = Image.Type.Sliced;
        RectTransform panelTransform = panel.GetComponent<RectTransform>();
        panel.transform.SetParent(rootCanvas.transform, true);
        panelTransform.localScale = new Vector3(1f, 1f, 1f);
        panelTransform.localPosition = new Vector3(0, 40, 0);
        panelTransform.sizeDelta = size;
        //panelTransform.rotation = Quaternion.AngleAxis(-180, Vector3.up);        
        CreateText(panelTransform, startLoc, new Vector2(size.x - 150, size.y), msg, 50, 0, anchor, main);
        if(!notes.Equals("")){CreateText(panelTransform, new Vector2(size.x*0.4f - 20, size.y*0.4f-20), new Vector2(size.x/4,100), notes, 40, 0, anchor, main);}
        CreateButton(panelTransform, "Button0", actionLabel0, action, "0", new Vector3(-size.x / 5, -40 - size.y / 2, 0), new Vector2(300, 60), main);
        CreateButton(panelTransform, "Button1", actionLabel1, action, "1", new Vector3(size.x / 5, -40 - size.y / 2, 0), new Vector2(300, 60), main);

        // showTutorialsMenu(rootMenu);
        return rootObj;
    }

    // porgressbar object
    private static GameObject progressBar(string name, string label, int value, Vector2 loc, Transform root, Main main) {
        GameObject gm = new GameObject(name);
        gm.transform.position = new Vector3(loc.x, loc.y, 0);
        gm.transform.SetParent(root, false);
        
        Texture2D tex2 = new Texture2D(600, 600);
        Sprite sprite = Sprite.Create(tex2, new Rect(0.0f, 0.0f, 600, 100), new Vector2(300f, 300f), 100.0f);
        //Sprite spriteBack = Sprite.Create(tex2, new Rect(0.0f, 0.0f, 600, 100), new Vector2(300f, 300f), 100.0f);
        string sValue = value.ToString();
        CreateText(gm.transform, new Vector2(820, 10), new Vector2(1000, 70), label+":", 60, 0, TextAlignmentOptions.BottomRight, main);
        // drow background
        GameObject gm1 = new GameObject(name+"_back");
        gm1.transform.position = new Vector3(-120, 0, 0);
        gm1.transform.SetParent(gm.transform, false);        
        Image img = gm1.AddComponent<Image>();
        img.sprite = sprite;
        img.material = main.variables.matFont;
        img.fillMethod = Image.FillMethod.Horizontal;
        img.type = Image.Type.Filled;
        img.fillOrigin = (int) Image.OriginHorizontal.Right;
        img.fillAmount = 1;
        img.color = new Vector4(0.68f, 0.68f, 0.7f, 1f);
        gm1.GetComponent<RectTransform>().sizeDelta = new Vector2(700, 70);
        // drow current value
        GameObject gm2 = new GameObject(name + "_value");
        gm2.transform.position = new Vector3(-120, 0, 0);
        gm2.transform.SetParent(gm.transform, false);        
        Image img2 = gm2.AddComponent<Image>();
        img2.sprite = sprite;
        img2.material = main.variables.matFont;
        img2.fillMethod = Image.FillMethod.Horizontal;
        img2.type = Image.Type.Filled;
        img2.fillOrigin = (int)Image.OriginHorizontal.Right;
        img2.fillAmount = (float) (value/100f);
        img2.color = new Vector4(0.34f, 0.41f, 0.94f, 1f);
        gm2.GetComponent<RectTransform>().sizeDelta = new Vector2(700, 70);
        //+
        CreateText(gm2.transform, new Vector2(-450, 10), new Vector2(1000, 70), sValue + "%", 60, 0, TextAlignmentOptions.BottomLeft, main);
        CreateButton(gm.transform, "?"+name,"?", "ShowHelp", name+"_"+value, new Vector3(280, 0, 0), new Vector2(65, 65), main);
        return gm;
    }

    // results panel object
    public static GameObject showResult(string userLang, int resSt, int resEn, string name1, string name2, int value1, int value2, int value3, Main main){
        Vector2 size = new Vector2(1200, 500);
        GameObject newCanvas = CreateCanvas("rootMenu", new Vector3(0, main.baseLoc, 12), size);
        GameObject panel = new GameObject("ResultPanel");
        panel.AddComponent<CanvasRenderer>();
        Image img = panel.AddComponent<Image>();
        img.sprite = main.variables.uisprite;
        img.color = new Vector4(0.7f, 0.7f, 0.75f, 0.5f);
        RectTransform panelTransform = panel.GetComponent<RectTransform>();
        panel.transform.SetParent(newCanvas.transform, true);
        panelTransform.localScale = new Vector3(1f, 1f, 1f);
        panelTransform.localRotation = Quaternion.AngleAxis(180, Vector3.up);
        panelTransform.localPosition = new Vector3(0, 0, 0);
        panelTransform.sizeDelta = new Vector2(1600, 900);        
        CreateText(panelTransform, new Vector2(0, 340), new Vector2(1400, 70), TextData.getMessage(userLang, "Res_C"), 60, 0, TextAlignmentOptions.Bottom, main);
        progressBar("Stress", TextData.getMessage(userLang, "Res_C_S"), resSt, new Vector2(-150, 240),panelTransform, main);
        progressBar("Effi", TextData.getMessage(userLang, "Res_C_E"), resEn, new Vector2(-150, 140), panelTransform, main);

        CreateText(panelTransform, new Vector2(0, 0), new Vector2(1400, 70), TextData.getMessage(userLang, "Res_T"), 60, 0, TextAlignmentOptions.Bottom, main);    
        progressBar(name1, TextData.getMessage(userLang, name1), value1, new Vector2(-150, -100), panelTransform, main);
        progressBar(name2, TextData.getMessage(userLang, name2), value2, new Vector2(-150, -200), panelTransform, main);
        progressBar("power", TextData.getMessage(userLang, "Power"), value3, new Vector2(-150, -300), panelTransform, main);

        CreateButton(panelTransform, "btnExit", TextData.getMessage(userLang, "btnExit"), "Exit", "0", new Vector3(100-size.x / 2, 20 - size.y, 0), new Vector2(300, 65), main);
        CreateButton(panelTransform, "btnRepeat", TextData.getMessage(userLang, "btnRepeat"), "Repeat", "0", new Vector3(0, 20 - size.y, 0), new Vector2(300, 65), main);
        CreateButton(panelTransform, "btnAbout", TextData.getMessage(userLang, "btnAbout"), "Next", "0", new Vector3(-100 + size.x / 2, 20 - size.y, 0), new Vector2(300, 65), main);

        return newCanvas;
    }

    public static GameObject ShowSwitchlngMenu(string userLang, Main main){
        GameObject rootObj =  ShowMessage(TextData.getMessage(userLang, "msgLangSw"), "", "", new Vector2(1200, 150), TextAlignmentOptions.Midline, new Vector2(0, 0), main);
        Dictionary<string, string> langs = TextData.getLanguages();
        int cnt  = langs.Count, j=-350;
        var enumerator = langs.GetEnumerator();
        while (enumerator.MoveNext()){
            CreateButton(rootObj.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform, "Button"+j, enumerator.Current.Value, "LangSwMenu", enumerator.Current.Key, new Vector3(j, -150, 0), new Vector2(300, 65), main);
            j += 350;
        }
        return rootObj;    
    }

    public static GameObject ShowGenderMenu(string userLang, Main main){
        GameObject rootObj = ShowMessage(TextData.getMessage(userLang, "msgGender"), "", "", new Vector2(1200, 150), TextAlignmentOptions.Midline, new Vector2(0, 0), main);
        CreateButton(rootObj.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform, "Male", TextData.getMessage(userLang, "btnMale"), "GenderSwMenu", "", new Vector3(-200, -150, 0), new Vector2(300, 65), main);
        CreateButton(rootObj.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform, "Female", TextData.getMessage(userLang, "btnFemale"), "GenderSwMenu", "", new Vector3(200, -150, 0), new Vector2(300, 65), main);
        CreateButton(rootObj.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform, "Unknown", TextData.getMessage(userLang, "btnUnknown"), "GenderSwMenu", "", new Vector3(0, -250, 0), new Vector2(600, 65), main);        
        return rootObj;
    }
}
