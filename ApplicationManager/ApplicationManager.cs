using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ApplicationManager : MonoBehaviour {


    private static ApplicationManager instance;


    #region debug设置

    //当前debug 模式
    private static bool b_openDebug = true;

    //debug面板
    public GameObject debugCanvas;

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
            if (Instance.debugCanvas != null)
            {
                Instance.debugCanvas.SetActive(b_openDebug);
            }

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

    static public ApplicationManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("ApplicationManager").GetComponent<ApplicationManager>();
            }

            return instance;
        }

        set
        {
            instance = value;
        }
    }

    #endregion


    //总游戏入口
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Debug.unityLogger.logEnabled = B_openDebug;



        //正式进入游戏逻辑
        //UIManager.OpenUI<LoadingWindow>(LoadingWindow.c_assetPath);
        
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



