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

    private float currentX;
    public float minX;
    public float maxX;

    public float moveSpeed;

    public static BallController holding;

    [SerializeField] CinemachineVirtualCamera bowlingCamera;
    [SerializeField] CinemachineVirtualCamera scoreCamera;
    [SerializeField] CinemachineVirtualCamera ballsCamera;
    private CinemachineVirtualCamera currentCamera;
    private CinemachineBrain cameraBrain;

    // Start is called before the first frame update
    void Start()
    {
        currentCamera = bowlingCamera;
        currentCamera.gameObject.SetActive(true);
        cameraBrain = GetComponentInChildren<CinemachineBrain>();
        currentX = transform.position.x;
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
                ChangeCamera(ballsCamera);
            }

            if(Input.GetAxis("Vertical") > 0)
            {
                lookingAt = INBETWEEN;
                toLookAt = SCORE_SCREEN;
                ChangeCamera(scoreCamera);
            }

            if(Input.GetAxis("Horizontal") != 0)
            {
                currentX += Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

                if (currentX < minX)
                    currentX = minX;
                if (currentX > maxX)
                    currentX = maxX;

                transform.position = new Vector3(currentX, transform.position.y, transform.position.z);
                bowlingCamera.transform.position = new Vector3(currentX, bowlingCamera.transform.position.y, bowlingCamera.transform.position.z);
            }
        }

        if (lookingAt == BALL_RETURN)
        {
            transform.position = new Vector3(currentX - 10.0f, transform.position.y, transform.position.z);

            if (Input.GetAxis("Switch View") < 0)
            {
                transform.position = new Vector3(currentX, transform.position.y, transform.position.z);
                toLookAt = LANE;
                lookingAt = INBETWEEN;
                ChangeCamera(bowlingCamera);
            }
        }

        if(lookingAt == SCORE_SCREEN)
        {
            if(Input.GetAxis("Vertical") < 0)
            {
                toLookAt = LANE;
                lookingAt = INBETWEEN;
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
