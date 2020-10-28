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

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().useGravity = false;
    }
    
    void Update()
    {
        if (PlayerController.lookingAt == 0)
        {
            if (thisBallInHand && Input.GetAxis("Fire1") == 1)
            {
                LaunchBall();
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
        GetComponent<Collider>().enabled = false;
        this.transform.position = ballHolder.position;
        this.transform.parent = ballHolder.parent;
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        PlayerController.holding = this;
    }

    public void PutDown()
    {
        anyBallInHand = false;
        thisBallInHand = false;
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

    /*
    // Update is called once per frame
    
   /* 
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !inHand)
        {
            if (Input.GetAxis("Interact") == 1)
                PickUp();
        }
    }
   
    private void OnMouseDown()
    {
        if (PlayerController.ballSelect)
        {
            if(!anyBallInHand)
                PickUp();
        }
    }

    

    

    
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayArea"))
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>(), true);
        }
    }
    */
}
