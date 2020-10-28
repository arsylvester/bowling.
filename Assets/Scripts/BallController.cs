using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject player;
    public Transform ballHolder;
    public float ballSpeed = 10.0f;
    private Boolean inHand = false;
    public Vector3 showVel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inHand && Input.GetAxis("Fire1") == 1)
            launchBall();
        showVel = GetComponent<Rigidbody>().velocity;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !inHand)
        {
            if (Input.GetAxis("Interact") == 1)
                PickUp();
        }
    }

    public void PickUp()
    {
        inHand = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Collider>().enabled = false;
        this.transform.position = ballHolder.position;
        this.transform.parent = ballHolder.parent;
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    }

    private void launchBall()
    {
        inHand = false;
        this.transform.parent = null;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Collider>().enabled = true;
        Vector3 vel = player.transform.forward * ballSpeed;
        GetComponent<Rigidbody>().velocity = vel;
    }
}
