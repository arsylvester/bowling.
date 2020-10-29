using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BallDispenser), true)]
public class BallDispenserEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        BallDispenser dispenser = target as BallDispenser;

        if(GUILayout.Button("Spawn Test Ball")) {
            dispenser.SpawnBall(dispenser.testBallIndex);
        }
        if(GUILayout.Button("Spawn Random Test Ball")) {
            dispenser.SpawnBall(Random.Range(0, dispenser.ballList.Count - 1));
        }
        if(GUILayout.Button("Move Referenced Ball")) {
            dispenser.MoveTestBall();
        }
    }

}