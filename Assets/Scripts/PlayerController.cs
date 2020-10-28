using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const int LANE = 0;
    const int BALL_RETURN = 1;
    const int SCORE_SCREEN = 2;

    public static int lookingAt = LANE;

    public Transform ballReturn;
    public Transform lane;
    public Transform scoreScreen;

    public static BallController holding;

    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(lane);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (lookingAt == LANE)
        {
            if (Input.GetAxis("Switch View") > 0)
            {
                lookingAt = BALL_RETURN;
                transform.LookAt(ballReturn);
            }

            if(Input.GetAxis("Vertical") > 0)
            {
                lookingAt = SCORE_SCREEN;
                transform.LookAt(scoreScreen);
            }
        }

        if (lookingAt == BALL_RETURN)
        {
            if(Input.GetAxis("Switch View") < 0)
            {
                lookingAt = LANE;
                transform.LookAt(lane);
            }
        }

        if(lookingAt == SCORE_SCREEN)
        {
            if(Input.GetAxis("Vertical") < 0)
            {
                lookingAt = LANE;
                transform.LookAt(lane);
            }
        }
    }
}
