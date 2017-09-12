using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    static UIManager instance;
    static GameObject go_uiManager;

    static public UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                go_uiManager = Go_UIManager;
            }

            return instance;
        }
    }

    static public GameObject Go_UIManager
    {
        get
        {
            if (go_uiManager == null)
            {
                go_uiManager = new GameObject("go_uiManager");
                instance = go_uiManager.AddComponent<UIManager>();
            }
            return go_uiManager;
        }
    }


    static public T OpenUI<T>(string assetPath) where T : UIBase
    {
        GameObject UIObj = GameObjectManager.CreatGameObjectByPool(assetPath, typeof(T).ToString());
        UIObj.transform.SetParent(Go_UIManager.transform);

        T UI = UIObj.GetComponent<T>();
        UI.OnOpen();

        return UI;
    }


}
