using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    public Vector2 showAverageVelocity;
    public Vector2 showCurrentMouse;
    public float showDifference;

    const int THROW_MOVE_SIZE = 100;
    const float FIXED_FRAME_TIME = .02f;
    private Vector2[] throwingMovements = new Vector2[THROW_MOVE_SIZE];
    private int currentMove = 0;
    private Vector2 screenScale;

    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        screenScale = new Vector2(Screen.width / 100, Screen.height / 100);
    }
    
    void FixedUpdate()
    {
        if (PlayerController.lookingAt == 0)
        {
            if (thisBallInHand && Input.GetAxis("Fire1") == 1)
            {
                launching = true;

                throwingMovements[currentMove] = Input.mousePosition / screenScale;
                currentMove++;
                if (currentMove >= THROW_MOVE_SIZE)
                    currentMove = 0;

                startMousePos = Input.mousePosition;
                startTime = Time.time;
            }

            if(thisBallInHand && launching && Input.GetAxis("Fire1") == 0)
            {
                Vector2 averageVelocity = getAverageVelocity();

                //Vector2 CurrentMousePos = Input.mousePosition;
                //showCurrentMouse = CurrentMousePos;
                //Vector2 difference = CurrentMousePos - startMousePos;
                //showDifVec = difference;
                //float distance = difference.magnitude;
                //showDifference = distance;
                //showDistanceX = difference.x;
                //showDistanceY = difference.y;
                //float finalTime = Time.time - startTime;
                //ballSpeed = Mathf.Abs((distance / finalTime) * speedModifyer);
                showAverageVelocity = averageVelocity;
                ballSpeed = Mathf.Sqrt(averageVelocity.magnitude);
                LaunchBall();
                GetComponent<Rigidbody>().angularVelocity = new Vector3(averageVelocity.y * speedModifyer2, 0f, (-1) * averageVelocity.x * speedModifyer2);
                showAngularVel = GetComponent<Rigidbody>().angularVelocity;
                launching = false;
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

    private Vector2 getAverageVelocity()
    {
        print("in average velocity");
        Vector2 result = new Vector2(0, 0);
        Vector2 temp = throwingMovements[0] - throwingMovements[1];
        print(temp);
        int nullCount = 0;
        for(int i = 0; i < THROW_MOVE_SIZE - 1; i++)
        {
            Vector2 difference = throwingMovements[i + 1] - throwingMovements[i];
            result += difference;
            if (difference.magnitude == 0)
                nullCount++;
        }
        print(result);
        return (result / (THROW_MOVE_SIZE - 1 - nullCount)) / FIXED_FRAME_TIME;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "PlayArea")
        {
            AkSoundEngine.PostEvent("Ballhit", gameObject);
        }
    }
}
