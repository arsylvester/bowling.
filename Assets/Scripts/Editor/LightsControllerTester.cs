using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LightsController), true)]
public class LightsControllerTester : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        LightsController lights = target as LightsController;

        if(GUILayout.Button("Flicker Random Light")) {
            lights.FlickerRandomLight();
        }
    }

}