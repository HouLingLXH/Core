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

    //防止object 被引用，在清理时不能清理
    public static void ClearAllData()
    {
        allAssetBundle = null;
        allGameObject = null;
        InHierarchyGo = null;
        OutHierarchyGo = null;
        assetMaterial = null;
        assetTexture2D = null;
        assetMesh = null;
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
            case c_Others: ShowAllObject(); break;
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
            case c_Texture: ShowAllTexture2D(); break;
            case c_Mesh: ShowAllMesh(); break;
        }
    }
    //材质资源
    static private List<Object> assetMaterial =new List<Object>();//所有Material
    //图片资源
    static private List<Object> assetTexture2D = new List<Object>();//所有Texture
    //网格资源
    static private List<Object> assetMesh = new List<Object>();//所有Mesh

    //更新美术资源列表
    private void UpdateArtAsset()
    {
        switch (assetToolbarSelect)
        {
            case c_Material:
                assetMaterial = ObjectManager.GetSomeObject<Material>();
                break;
            case c_Texture:
                //由于unity内置的Texture2D 太多，导致无法管理。所以要想管理，必须添加自己的标记，进行筛选
                assetTexture2D = ObjectManager.GetSomeObject<Texture2D>();
                break;
            case c_Mesh:
                assetMesh = ObjectManager.GetSomeObject<Mesh>();
                break;
        }
    }

    //展示所有材质
    private void ShowAllMaterial()
    {
        ShowSomeObjectInfo(assetMaterial);
    }

    //展示所有Texture
    private void ShowAllTexture2D()
    {
        ShowSomeObjectInfo(assetTexture2D);
    }

    //展示所有Mesh
    private void ShowAllMesh()
    {
        // gui绘制需要mesh，如果绘制mesh，就会再次增加mesh，导致指数增长
        //ShowSomeObjectInfo(assetMesh);
        GUILayout.Label("mesh 总数：" + assetMesh.Count);
        
    }

    #endregion

    #region 展示所有Obj
    private void ShowAllObject()
    {
        ShowSomeObjectInfo(allObject);
    }
    #endregion

    #endregion

    #region 工具方法

    //展示一个Object 信息
    private void ShowOneObjectInfo(Object obj,int index = 0)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(index.ToString(),GUILayout.Width(50));
        if (obj != null)
        {
            GUILayout.Label(". ID:  " + obj.GetInstanceID(),GUILayout.Width(150) );
            GUILayout.Label(" 名称： " + obj.name, GUILayout.Width(200));
            GUILayout.Label(" 类型： " + obj.GetType(), GUILayout.Width(200));
            EditorGUILayout.ObjectField(obj, typeof(Object), true);

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
            //展示部分Object的数量
            GUILayout.Label("总数： " + someObject.Count);
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
