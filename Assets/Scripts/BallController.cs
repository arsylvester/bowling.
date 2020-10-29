using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject player;
    public Transform ballHolder;
    public Transform ballReturnPosition;
    public float ballSpeed = 10.0f;
    private static Boolean anyBallInHand = false;
    private Boolean thisBallInHand = false;
    public Vector3 showVel;
    public Vector3 showAngularVel;

    public Vector2 startMousePos;
    private float startTime;
    private Boolean launching = false;
    public float speedModifyer = 0.7f;
    public float speedModifyer2;
    public float showDistanceX;
    public float showDistanceY;
    public Vector2 showDifVec;
    public Vector2 showCurrentMouse;
    public float showDifference;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Rigidbody>().useGravity = false;
    }
    
    void Update()
    {
        if (PlayerController.lookingAt == 0)
        {
            if (thisBallInHand && !launching && Input.GetAxis("Fire1") == 1)
            {
                launching = true;
                startMousePos = Input.mousePosition;
                startTime = Time.time;
            }

            if(thisBallInHand && launching && Input.GetAxis("Fire1") == 0)
            {
                Vector2 CurrentMousePos = Input.mousePosition;
                showCurrentMouse = CurrentMousePos;
                Vector2 difference = CurrentMousePos - startMousePos;
                showDifVec = difference;
                float distance = difference.magnitude;
                showDifference = distance;
                showDistanceX = difference.x;
                showDistanceY = difference.y;
                float finalTime = Time.time - startTime;
                ballSpeed = Mathf.Abs((distance / finalTime) * speedModifyer);
                LaunchBall();
                GetComponent<Rigidbody>().angularVelocity = new Vector3(ballSpeed * speedModifyer2, 0f, (-1) * (difference.x / startTime) * speedModifyer2);
                showAngularVel = GetComponent<Rigidbody>().angularVelocity;
            }
        }
        showVel = GetComponent<Rigidbody>().velocity;
    }
    
    private void OnMouseDown()
    {
        if (PlayerController.lookingAt == 1)
        {
            PickUp();
        }
    }

    public void PickUp()
    {
        if(anyBallInHand)
        {
            PlayerController.holding.PutDown();
        }
        anyBallInHand = true;
        thisBallInHand = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Collider>().enabled = false;
        this.transform.position = ballHolder.position;
        this.transform.parent = ballHolder;
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
        PlayerController.holding = this;
    }

    public void PutDown()
    {
        anyBallInHand = false;
        thisBallInHand = false;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Collider>().enabled = true;
        this.transform.position = ballReturnPosition.position;
        this.transform.parent = null;
        PlayerController.holding = null;
    }

    private void LaunchBall()
    {
        anyBallInHand = false;
        thisBallInHand = false;
        this.transform.parent = null;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Collider>().enabled = true;
        Vector3 vel = player.transform.forward * ballSpeed;
        GetComponent<Rigidbody>().velocity = vel;
        PlayerController.holding = null;
    }
}
