using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ApplicationManager : MonoBehaviour {

    #region debug设置

    //当前debug 模式
    private static bool b_openDebug = true;

    //获取当前debug状态

    public static bool B_openDebug
    {
        get
        {
            return b_openDebug;
        }
        set
        {
            b_openDebug = value;
            Debug.unityLogger.logEnabled = b_openDebug;
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
        set
        {
            b_useBundle = value;
        }
    }

    #endregion


    private void Reset()
    {
        
    }

    private FPSTool fpsTool;
    private void InitFPSTool()
    {
        if (fpsTool == null)
        {
            fpsTool = gameObject.AddComponent<FPSTool>();
            fpsTool.SetState(B_openDebug);
        }
    }



    //总游戏入口
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Debug.unityLogger.logEnabled = B_openDebug;





        //正式进入游戏逻辑
        UIManager.OpenUI<LoadingWindow>(LoadingWindow.c_assetPath);
        
    }



    #region 测试代码
    private void Update()
    {
        //随时可以切换场景，用于测试
        if (Input.GetKeyDown(KeyCode.R))
        {
            UIManager.OpenUI<LevelWindow>(LevelWindow.c_assetPath);
        }

    }
    #endregion

}



