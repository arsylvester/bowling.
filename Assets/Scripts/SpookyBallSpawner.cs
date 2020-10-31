using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyBallSpawner : MonoBehaviour {

    [SerializeField] private BallDispenser dispenser;
    [SerializeField] private GameObject ball;
    [SerializeField] private int frameNumber;

    private void Start() {
        GameStateController._instance.m_NewFrame.AddListener(() => {
            if(GameStateController.GetCurrentFrame() == frameNumber)
                dispenser.MoveBall(ball);
        });
    }

}