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
    private static Boolean anyBallInHand = false;
    private Boolean thisBallInHand = false;

    private Boolean launching = false;
    private float ballSpeed;
    public float speedModifyer = 4.0f;
    public float speedModifyer2;

    const int THROW_MOVE_SIZE = 25;
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
                //print(throwingMovements[currentMove]);
                incrementCurrentMove();
            }

            if(thisBallInHand && launching && Input.GetAxis("Fire1") == 0)
            {
                // get mouse velocity
                Vector2 averageVelocity = getAverageVelocity();
                print(averageVelocity);
                
                // get ball speed from mouse velocity
                float modifiedVectorMagnitude = Mathf.Sqrt(Mathf.Pow(averageVelocity.x / 2, 2f) + Mathf.Pow(averageVelocity.y, 2f));

                ballSpeed = Mathf.Log(Mathf.Pow(modifiedVectorMagnitude + 0.5f, 1.5f), 2) * speedModifyer;
                print(ballSpeed);

                LaunchBall();

                // set ball curve
                GetComponent<Rigidbody>().angularVelocity = new Vector3(averageVelocity.y * speedModifyer2 / 2, 0f, (-1) * averageVelocity.x * speedModifyer2);
                print(GetComponent<Rigidbody>().angularVelocity);
                launching = false;
            }
        }
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
        Vector2 result = new Vector2(0, 0);
        int nullCount = 0;
        for(int i = 0; i < THROW_MOVE_SIZE - 1; i++)
        {
            int move1 = currentMove;
            int move2 = incrementCurrentMove();
            if (throwingMovements[move1] == Vector2.zero || throwingMovements[move2] == Vector2.zero)
            {
                nullCount++;
            }
            else
            {
                Vector2 difference = throwingMovements[move2] - throwingMovements[move1];
                //print(throwingMovements[move2] + " - " + throwingMovements[move1] + " = " + difference);
                result += difference;
                if (difference == Vector2.zero)
                    nullCount++;
            }
        }
        //print(result);
        return result / (THROW_MOVE_SIZE - 1 - nullCount);
    }

    private int incrementCurrentMove()
    {
        currentMove++;
        if (currentMove >= THROW_MOVE_SIZE)
            currentMove = 0;
        return currentMove;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "PlayArea")
        {
            AkSoundEngine.PostEvent("Ballhit", gameObject);
        }
        if (collision.gameObject.tag == "BowlingBall")
        {
            AkSoundEngine.PostEvent("BallColide", gameObject);
        }
    }
}
