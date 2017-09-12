using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ParticleDelayTool))]
public class ParticalDelayToolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ParticleDelayTool myScript = (ParticleDelayTool)target;
        if (GUILayout.Button("Save DelayTime"))
        {
            myScript.Add();
        }
    }
}

