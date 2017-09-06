using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectManagerWindow : EditorWindow {


 
    #region GUI
    static private Vector2 scrollView; //下拉区域
    static readonly private string[] toolbarStrings = new string[] {"AssetBundle","Art Asset","GameObject","Others" };
    static private int toolbarSelect; //当前选择的tool 
    const int c_AssetBunld = 0;
    const int c_ArtAsset = 1;
    const int c_GameObject = 2;
    const int c_Others = 3;

    #endregion

    [MenuItem("Window/Object Manager")]
    public static void Open()
    {
        GetWindow(typeof(ObjectManagerWindow));
    }

    private void OnGUI()
    {
        FindObjectByID();
        Bit_UpdateAllObjectInfo();
        ShowObjectInfo();
    }

    #region UI内方法


    #region 根据ID 查找Obj
    static private List<Object> allObject; //工程中所有Object
    int aimObjetID; //希望寻找的Object ID
    Object findAim;//希望寻找的Object
    //展示所有Obj 信息
    void FindObjectByID()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("请输入你要寻找的Object的Instance ID：", GUILayout.Width(300));
        aimObjetID = int.Parse(GUILayout.TextField(aimObjetID.ToString()));

        if (GUILayout.Button("查找"))
        {
            findAim = ObjectManager.GetObjectFromID(aimObjetID);
        }
        GUILayout.EndHorizontal();

        if (findAim != null)
        {
            EditorGUILayout.ObjectField(findAim, typeof(Object), true);
            GUILayout.Label(findAim.hideFlags.ToString());
        }
        else
        {
            GUILayout.Label("项目中没有Object:  " + aimObjetID);
        }

        GUILayout.Space(30);

    }
    #endregion

    //更新按钮  更新所要Object 信息
    void Bit_UpdateAllObjectInfo()
    {
        if (GUILayout.Button("刷新Object"))
        {
            ObjectManager.UpdateAllObject();
            switch (toolbarSelect)
            {
                case c_AssetBunld:
                    allAssetBundle = ObjectManager.GetSomeObject<AssetBundle>();
                    break;
                case c_ArtAsset:
                    UpdateArtAsset();
                    break;
                case c_GameObject:
                    allGameObject = ObjectManager.GetSomeObject<GameObject>();
                    InHierarchyGo.Clear();
                    OutHierarchyGo.Clear();
                    for (int i = 0; i < allGameObject.Count; i++)
                    {
                        if ((allGameObject[i] as GameObject).activeInHierarchy)
                        {
                            InHierarchyGo.Add(allGameObject[i]);
                        }
                        else
                        {
                            OutHierarchyGo.Add(allGameObject[i]);
                        }
                    }
                    break;
                case c_Others:
                    Object[] l_allObject = ObjectManager.GetAllObject();
                    if (l_allObject != null)
                    {
                        allObject = new List<Object>(l_allObject);
                    }
                    break;
            }
        }
    }

    // 展示Object 信息
    void ShowObjectInfo()
    {
        toolbarSelect = GUILayout.Toolbar(toolbarSelect, toolbarStrings);
        scrollView = GUILayout.BeginScrollView(scrollView, false, false);
        switch (toolbarSelect)
        {
            case c_AssetBunld: ShowAssetBundle(); break;
            case c_ArtAsset: ShowAllArtAsset(); break;
            case c_GameObject: ShowAllGameObject(); break;
            case c_Others: ; break;
        }
        GUILayout.EndScrollView();
    }

    #region 展示所有assetBundle
    static private List<Object> allAssetBundle;//所有assetbundle
    //展示所有AssetBundle
    void ShowAssetBundle()
    {
        ShowSomeObjectInfo(allAssetBundle);
    }
    #endregion

    #region 展示所有GameObject
    static private List<Object> allGameObject;//所有GameObject
    static private List<Object> InHierarchyGo = new List<Object>(); //activeInHierarchy == true
    static private List<Object> OutHierarchyGo = new List<Object>(); //activeInHierarchy == false

    static readonly private string[] goToolbarStrings = new string[] { "InHierarchy", "OutHierarchy", "All"};
    static private int goToolbarSelect; //当前选择的tool
    
    const int c_InHierarchy = 0;
    const int c_OutHierarchy = 1;
    const int c_AllGameObject = 2;

    //展示所有GameObject
    private void ShowAllGameObject()
    {
        goToolbarSelect = GUILayout.Toolbar(goToolbarSelect, goToolbarStrings);
        switch (goToolbarSelect)
        {
            case c_InHierarchy:ShowSomeObjectInfo(InHierarchyGo); break;
            case c_OutHierarchy:ShowSomeObjectInfo(OutHierarchyGo); break;
            case c_AllGameObject: ShowSomeObjectInfo(allGameObject); break;
        }

        
    }

    #endregion

    #region 展示所有Asset 包括 Mesh 、 Material  、Texture

    static readonly private string[] assetToolbarStrings = new string[] { "Material", "Texture", "Mesh" };
    static private int assetToolbarSelect = 0; //当前选择的tool

    const int c_Material = 0;
    const int c_Texture = 1;
    const int c_Mesh = 2;

    //展示所有美术资源
    private void ShowAllArtAsset()
    {
        assetToolbarSelect = GUILayout.Toolbar(assetToolbarSelect, assetToolbarStrings);
        switch (assetToolbarSelect)
        {
            case c_Material: ShowAllMaterial(); break;
            case c_Texture: ShowAllTexture(); break;
            case c_Mesh: ShowAllMesh(); break;
        }
    }

    //更新美术资源列表
    private void UpdateArtAsset()
    {
        switch (assetToolbarSelect)
        {
            case c_Material:
                break;
            case c_Texture:
                break;
            case c_Mesh:
                break;
        }
    }

    //展示所有材质
    private void ShowAllMaterial()
    {

    }

    //展示所有Texture
    private void ShowAllTexture()
    {
    }

    //展示所有Mesh
    private void ShowAllMesh()
    {

    }

    #endregion


    #endregion

    #region 工具方法
    //展示一个Object 信息
    private void ShowOneObjectInfo(Object obj,int index = 0)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(index.ToString(),GUILayout.Width(30));
        if (obj != null)
        {
            GUILayout.Label(". ID:  " + obj.GetInstanceID(),GUILayout.Width(150) );
            GUILayout.Label(" 名称： " + obj.name, GUILayout.Width(200));
            EditorGUILayout.ObjectField(obj, typeof(Object), false);
            if (obj.GetType() == typeof(GameObject))
            {
                GUILayout.Label("activeInHierarchy: " + (obj as GameObject).activeInHierarchy);
            }
        }
        
        //EditorGUILayout.ObjectField(obj,typeof(object),true);
        GUILayout.EndHorizontal();
    }

    //展示某一部分Object
    private void ShowSomeObjectInfo(List<Object> someObject)
    {
        if (someObject != null)
        {
            GUILayout.BeginVertical();
            for (int i = 0; i < someObject.Count; i++)
            {
                ShowOneObjectInfo(someObject[i],i);

            }
            GUILayout.EndVertical();
        }
       
    }


    #endregion


}
