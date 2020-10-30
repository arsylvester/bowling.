﻿using Packages.Rider.Editor.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDispenser : MonoBehaviour {

#pragma warning disable 0649 // Disable "Field is never assigned" warning for SerializeField

    [SerializeField] private Transform ballSpawnPos;
    public List<GameObject> ballList;

    [Header("Test Variables")]
    [SerializeField] private GameObject referencedBall;
    public int testBallIndex;

#pragma warning disable 0649

    public void MoveBall(GameObject ball) {
        ball.transform.position = ballSpawnPos.position;
        ball.transform.parent = ballSpawnPos;

        try {
            ball.GetComponent<Rigidbody>().useGravity = true;
            ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        } catch { }
    }

    public void SpawnBall(int index) {
        MoveBall(Instantiate(ballList[index]));
    }

    public void MoveTestBall() {
        MoveBall(referencedBall);
    }

}