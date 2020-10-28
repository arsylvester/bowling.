using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    const int INBETWEEN = -1;
    const int LANE = 0;
    const int BALL_RETURN = 1;
    const int SCORE_SCREEN = 2;

    public static int lookingAt = LANE;
    private int toLookAt;

    public Transform ballReturn;
    public Transform lane;
    public Transform scoreScreen;

    public static BallController holding;

    [SerializeField] CinemachineVirtualCamera bowlingCamera;
    [SerializeField] CinemachineVirtualCamera scoreCamera;
    [SerializeField] CinemachineVirtualCamera ballsCamera;
    private CinemachineVirtualCamera currentCamera;
    private CinemachineBrain cameraBrain;

    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(lane);
        currentCamera = bowlingCamera;
        currentCamera.gameObject.SetActive(true);
        cameraBrain = GetComponentInChildren<CinemachineBrain>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(lookingAt == INBETWEEN && !cameraBrain.IsBlending)
        {
            lookingAt = toLookAt;
        }

        if (lookingAt == LANE)
        {
            if (Input.GetAxis("Switch View") > 0)
            {
                toLookAt = BALL_RETURN;
                lookingAt = INBETWEEN;
                transform.LookAt(ballReturn);
                ChangeCamera(ballsCamera);
            }

            if(Input.GetAxis("Vertical") > 0)
            {
                lookingAt = INBETWEEN;
                toLookAt = SCORE_SCREEN;
                transform.LookAt(scoreScreen);
                ChangeCamera(scoreCamera);
            }
        }

        if (lookingAt == BALL_RETURN)
        {
            if(Input.GetAxis("Switch View") < 0)
            {
                toLookAt = LANE;
                lookingAt = INBETWEEN;
                transform.LookAt(lane);
                ChangeCamera(bowlingCamera);
            }
        }

        if(lookingAt == SCORE_SCREEN)
        {
            if(Input.GetAxis("Vertical") < 0)
            {
                toLookAt = LANE;
                lookingAt = INBETWEEN;
                transform.LookAt(lane);
                ChangeCamera(bowlingCamera);
            }
        }
    }

    void ChangeCamera(CinemachineVirtualCamera newCamera)
    {
        newCamera.gameObject.SetActive(true);
        currentCamera.gameObject.SetActive(false);
        currentCamera = newCamera;
    }
}
