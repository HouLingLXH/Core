using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetBundleConfigWindow : EditorWindow {

    static Dictionary<int, List<string>> allObjUsedBundle; //所有Asset -> Assetbundle 情况
    static Dictionary<string, List<int>> allBundleUsedInfo; //所有Assetbundle -> Asset 的情况

    #region toolBar 相关
    static readonly string[] toolbarStrings = new string[] { "Object -> Bundle", "Bundle -> Object" };
    const int c_selectObject = 0;
    const int c_selectBundle = 1;
    private static int toolbarSelected; //当前选择的toolbar
    #endregion

    private Vector2 scrollViewVector;
     

    [MenuItem("Window/AssetBundle Manager")]
    public static void Open()
    {
        EditorWindow.GetWindow(typeof(AssetBundleConfigWindow));

    }

    private void OnGUI()
    {
        GUILayout.Space(25);
        if (GUILayout.Button("生成所有AssetBundle到：" + AssetBundleManager.c_streamingAssetsPathName))
        {
            if (EditorUtility.DisplayDialog(
                "生成",
                "你确定重新生成所有Bundle到" + AssetBundleManager.c_streamingAssetsPathName + "下吗？",
                "确定",
                "取消"
                ))
            {
                string path = AssetBundleManager.c_streamingAssetsPathName;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
                AssetDatabase.Refresh();
            }

           
        }

        GUILayout.Space(50);

        toolbarSelected = GUILayout.Toolbar(toolbarSelected, toolbarStrings);

        scrollViewVector = GUILayout.BeginScrollView(scrollViewVector,false,false,GUILayout.Width(Screen.width));

        switch (toolbarSelected)
        {
            case c_selectObject:
                ShowAllAssetObject();
                break;
            case c_selectBundle:
                ShowAllAssetBundleInfo();
                break;
        }

        GUILayout.EndScrollView();


    }

    //展示所有Asset -> bundle使用情况
    static void ShowAllAssetObject()
    {
        allObjUsedBundle = AssetBundleManager.GetAllAssetUsedBundle();
        GUILayout.BeginVertical();
        int index = 1;
        foreach (var item in allObjUsedBundle)
        {
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Label(index +". ID:  " + item.Key.ToString());
            Object obj = GetObjectFromID(item.Key);
            
            GUILayout.Label(" 对象： ");
            EditorGUILayout.ObjectField(obj, typeof(Object), true);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Space(30);
            GUILayout.BeginVertical();
            GUILayout.Label("关联的AssetBundle：");
            for (int i = 0; i < item.Value.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(100);
                GUILayout.Label("<"+ i +">"+item.Value[i]);
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            index++;
        }

        GUILayout.BeginVertical();
        allObjUsedBundle = null;
    }

    //展示所有bundle -> Asset 的信息
    static void ShowAllAssetBundleInfo()
    {
        allObjUsedBundle = AssetBundleManager.GetAllAssetUsedBundle();
        if (allBundleUsedInfo == null)
        {
            allBundleUsedInfo = new Dictionary<string, List<int>>();
        }
        foreach (var item in allObjUsedBundle)
        {
            for(int i = 0;i<item.Value.Count;i++)
            {
                if (!allBundleUsedInfo.ContainsKey(item.Value[i]))
                {
                    allBundleUsedInfo.Add(item.Value[i], new List<int>());
                }
                if (!allBundleUsedInfo[item.Value[i]].Contains(item.Key))
                {
                    allBundleUsedInfo[item.Value[i]].Add(item.Key);
                }
            }
        }

        GUILayout.BeginVertical();
        int index = 1;
        foreach (var item in allBundleUsedInfo)
        {
            GUILayout.Space(20);
            GUILayout.Label(index + ". Assetbundle:  " + item.Key);
            if (GUILayout.Button("卸载此bundle"))
            {
                AssetBundleManager.UnloadOneAsset(item.Key,true);
            }

            GUILayout.BeginHorizontal();
            GUILayout.Space(100);
            GUILayout.BeginVertical();

            for (int i = 0; i < item.Value.Count; i++)
            {
                Object obj = GetObjectFromID(item.Value[i]);
                EditorGUILayout.ObjectField(obj, typeof(Object), true);
            }

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();



            index++;
        }
        GUILayout.EndVertical();

        allObjUsedBundle = null;

    }

    //通过Instance ID 查找 Obj
    static Object GetObjectFromID(int id)
    {
        foreach (Object o in Resources.FindObjectsOfTypeAll(typeof(Object)))
        {
            if (o.GetInstanceID() == id)
            {
                return o;
            }
        }
        return null;
    }

}
