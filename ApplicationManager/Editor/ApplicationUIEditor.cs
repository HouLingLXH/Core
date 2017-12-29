using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ApplicationManager))]
public class ApplicationUIEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ApplicationManager myScript = (ApplicationManager)target;

        ApplicationManager.B_openDebug = GUILayout.Toggle(ApplicationManager.B_openDebug, "打印debug！");
        ApplicationManager.B_useBundle =  GUILayout.Toggle(ApplicationManager.B_useBundle, "使用AssetBundle 加载！");
    }
}
