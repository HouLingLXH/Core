using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour {

    #region debug设置

    //当前debug 模式
    private static bool b_openDebug = false;

    //获取当前debug状态

    public static bool B_openDebug
    {
        get
        {
            return b_openDebug;
        }
    }
    #endregion


    #region bundle设置
    //是否使用bundle加载
    private static bool b_useBundle = true;

    public static bool B_useBundle
    {
        get
        {
            return b_useBundle;
        }
    }

    #endregion


    private void Start()
    {
        Debug.unityLogger.logEnabled = B_openDebug;
        UIManager.OpenUI<LoadingWindow>("ui/loadingwindow");
    }

}



