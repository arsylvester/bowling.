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

    private float currentRotY;
    public float minRotY;
    public float maxRotY;

    public float rotationSpeed;

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
        currentRotY = transform.rotation.y;
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
        
        if(lookingAt != INBETWEEN)
        {
            if (lookingAt != LANE && Input.GetAxis("Vertical") > 0)
            {
                transform.position = new Vector3(currentX, transform.position.y, transform.position.z);
                lookingAt = INBETWEEN;
                toLookAt = LANE;
                ChangeCamera(bowlingCamera);
            }

            if (lookingAt != BALL_RETURN && Input.GetAxis("Switch View") > 0)
            {
                transform.position = new Vector3(currentX - 10.0f, transform.position.y, transform.position.z);
                toLookAt = BALL_RETURN;
                lookingAt = INBETWEEN;
                ChangeCamera(ballsCamera);
            }

            if (lookingAt != SCORE_SCREEN && Input.GetAxis("Switch View") < 0)
            {
                lookingAt = INBETWEEN;
                toLookAt = SCORE_SCREEN;
                ChangeCamera(scoreCamera);
            }
        }

        if (lookingAt == LANE)
        {
            if (Input.GetAxis("Horizontal") != 0)
            {
                currentX += Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

                if (currentX < minX)
                    currentX = minX;
                if (currentX > maxX)
                    currentX = maxX;

                transform.position = new Vector3(currentX, transform.position.y, transform.position.z);
                bowlingCamera.transform.position = new Vector3(currentX, bowlingCamera.transform.position.y, bowlingCamera.transform.position.z);
            }

            if(Input.GetAxis("Fire2") == 1)
            {
                if (Input.GetAxis("Mouse X") != 0)
                {
                    currentRotY += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

                    if (currentRotY < minRotY)
                        currentRotY = minRotY;
                    if (currentRotY > maxRotY)
                        currentRotY = maxRotY;

                    transform.rotation = Quaternion.Euler(transform.rotation.x, currentRotY, transform.rotation.z);
                    bowlingCamera.transform.rotation = Quaternion.Euler(bowlingCamera.transform.rotation.x, currentRotY, bowlingCamera.transform.rotation.z);
                }
            }

            if(Input.GetAxis("Rotation Reset") != 0)
            {
                if(currentRotY > 0)
                {
                    currentRotY -= rotationSpeed * Time.deltaTime;
                }
                if(currentRotY < 0)
                {
                    currentRotY += rotationSpeed * Time.deltaTime;
                }
                transform.rotation = Quaternion.Euler(transform.rotation.x, currentRotY, transform.rotation.z);
                bowlingCamera.transform.rotation = Quaternion.Euler(bowlingCamera.transform.rotation.x, currentRotY, bowlingCamera.transform.rotation.z);
            }
        }
    }

    void ChangeCamera(CinemachineVirtualCamera newCamera, bool unpause = true)
    {
        newCamera.gameObject.SetActive(true);
        currentCamera.gameObject.SetActive(false);
        currentCamera = newCamera;

        if(unpause)
            FindObjectOfType<PauseMenuController>().Unpause();
    }

    public void ChangeCameraToScore() {
        if(lookingAt != SCORE_SCREEN) {
            lookingAt = INBETWEEN;
            toLookAt = SCORE_SCREEN;
            ChangeCamera(scoreCamera, false);
        }
    }

}
