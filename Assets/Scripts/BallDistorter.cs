using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDistorter : MonoBehaviour {

    [SerializeField] private List<MeshRenderer> balls = new List<MeshRenderer>();
    public static float distortionRate;

    private void Start() {
        foreach(MeshRenderer ball in balls)
            ball.sharedMaterial.SetFloat("Vector1_75BF3378", 0f);

        GameStateController._instance.m_NewFrame.AddListener(() => {
            /*foreach(MeshRenderer ball in balls) {
                ball.sharedMaterial.SetFloat("Vector1_75BF3378", GameStateController.GetCurrentFrame() * distortionRate);
            }*/
            distortionRate = GameStateController.GetCurrentFrame() * 0.1f;
        });
    }

}