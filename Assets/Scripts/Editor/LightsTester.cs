using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Lights), true)]
public class LightsTester : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        Lights light = target as Lights;

        if(GUILayout.Button("Toggle")) {
            light.ToggleLight();
        }
        if(GUILayout.Button("Flicker")) {
            light.FlickerLight(0.5f, 4);
        }
    }

}