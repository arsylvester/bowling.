using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerOld : MonoBehaviour
{
    public CharacterController charBody;
    public Camera playerCam;
    public Camera ballSelectCam;
    public float moveSpeed = 2.0f;
    public float horizontalCameraSpeed = 2.0f; 
    public float verticalCameraSpeed = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    public float pitchMin = -45.0f;
    public float pitchMax = 45.0f;

    public float yawMin = -75.0f;
    public float yawMax = 75.0f;

    public static Boolean ballSelect = false;

    // Start is called before the first frame update
    void Start()
    {
        ballSelectCam.enabled = false;
    }

    void FixedUpdate()
    {
        if (!ballSelect)
        {
            //camera
            yaw += horizontalCameraSpeed * Input.GetAxis("Mouse X");
            pitch -= verticalCameraSpeed * Input.GetAxis("Mouse Y");

            if (pitch < pitchMin)
                pitch = pitchMin;
            else if (pitch > pitchMax)
                pitch = pitchMax;

            if (yaw < yawMin)
                yaw = yawMin;
            else if (yaw > yawMax)
                yaw = yawMax;

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

            //movement
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveForward = Input.GetAxis("Vertical");
            float verticalVelocity = -9.8f * Time.deltaTime;
            Vector3 movement = new Vector3(moveHorizontal, verticalVelocity, moveForward);
            Quaternion rotation = Quaternion.Euler(0, yaw, 0);
            charBody.Move(rotation * movement * moveSpeed * Time.deltaTime);

            //go to ball select
            if(Input.GetAxis("Switch View") == 1)
            {
                ballSelect = true;
                playerCam.enabled = false;
                ballSelectCam.enabled = true;
            }
        }
        if(ballSelect)
        {
            if(Input.GetAxis("Switch View") == -1)
            {
                ballSelect = false;
                playerCam.enabled = true;
                ballSelectCam.enabled = false;
            }
        }
    }
}
