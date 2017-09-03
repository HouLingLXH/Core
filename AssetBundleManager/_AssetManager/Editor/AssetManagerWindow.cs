using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetManagerWindow : EditorWindow {

    static AssetManagerWindow m_assetManagerWindow;

    #region toolbar 相关
    static readonly string[] toolbarStrings = new string[] { "Asset -> Obj", "Obj -> Asset" }; //标签内容
    static int toolbarSelected;//当前页签选择
    const int c_asset2Obj = 0;//"Asset -> Obj"
    const int c_obj2Asset = 1;//"Obj -> Asset"
    #endregion

    #region GUI相关
    static Vector2 scrollView;//滑动区域

    #endregion

    static Dictionary<string, Object> m_allLoadedAsset;//所有加载了的Asset  名称 -> Obj 此处就要求，所有asset 的名称必须唯一
    static Dictionary<Object, string> m_allLoadedAsset2;//所有加载了的Asset  Obj -> 名称 

    static Dictionary<Object, List<int>> m_allAssetBeUsedByObj; //所有asset的被引用情况
    static Dictionary<int, Object> m_allObjUsedAsset ; //所有实例对Asset的使用情况 ，key 是instanceID


    [MenuItem("Window/Asset Manager")]
    static void Open()
    {
        m_assetManagerWindow = EditorWindow.GetWindow(typeof(AssetManagerWindow)) as AssetManagerWindow;
    }

    private void OnGUI()
    {
        //从assetmanager 获取所有数据
        GetDataFromAssetManager();

        //展示内容
        Draw();


    }

    #region GUI内方法

    //从assetmanager 获取所有数据
    private void GetDataFromAssetManager()
    {
        m_allLoadedAsset = AssetManager.GetAllLoadedAsset();
        m_allLoadedAsset2 = AssetManager.GetAllLoadedAsset2();

        m_allAssetBeUsedByObj = AssetManager.GetUsedAssetByObj();
        m_allObjUsedAsset = AssetManager.GetObjUseAsset();
    }

    //绘制ui 以及 内容
    private void Draw()
    {
        toolbarSelected = GUILayout.Toolbar(toolbarSelected, toolbarStrings);
        scrollView = GUILayout.BeginScrollView(scrollView, false, false);
        switch(toolbarSelected)
        {
            case c_asset2Obj: ShowAsset2Obj(); break;
            case c_obj2Asset: ShowObj2Asset(); break;
        }
        GUILayout.EndScrollView();
    }

    #endregion

    #region Show 内方法
    //展示 asset -> Obj
    private void ShowAsset2Obj()
    {
        int index = 0;
        foreach (var item in m_allAssetBeUsedByObj)
        {
            Object asset = item.Key;
            if (asset == null)
            {
                continue;
            }
            GUILayout.Label(asset.name + "  ID: " + asset.GetInstanceID() + "  类型：" + asset.GetType());
            if (GUILayout.Button("删除此资源"))
            {
                AssetManager.UnloadOneAsset(asset);
            }
            GUILayout.BeginHorizontal();
            GUILayout.Space(30);
            GUILayout.BeginVertical();
            for (int i = 0; i < item.Value.Count; i++)
            {
                GUILayout.Label(index + ".  ID:" + item.Value[i]);
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            index++;
        }
    }


    //展示 Obj -> Asset
    private void ShowObj2Asset()
    {
        GUILayout.BeginVertical();
        int index = 0;
        foreach(var item in m_allObjUsedAsset)
        {
            GUILayout.Label(index +".  ID: " + item.Key + "  使用：" + item.Value);
            index++;
        }
        GUILayout.EndVertical();

    }


    

    #endregion

}
