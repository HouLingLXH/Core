using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetManagerWindow : EditorWindow {

    static AssetManagerWindow m_assetManagerWindow;

    static Dictionary<int, Object> m_allObjUsedAsset ; //所有实例对Asset的使用情况 ，key 是instanceID

    [MenuItem("Window/Asset Manager")]
    static void Open()
    {
        m_assetManagerWindow = EditorWindow.GetWindow(typeof(AssetManagerWindow)) as AssetManagerWindow;
    }

    private void OnGUI()
    {
        
    }

}
