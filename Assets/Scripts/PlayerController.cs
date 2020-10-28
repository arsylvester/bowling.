using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController charBody;
    public float moveSpeed = 2.0f;
    public float horizontalCameraSpeed = 2.0f; 
    public float verticalCameraSpeed = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    public float pitchMin = -45.0f;
    public float pitchMax = 45.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        //camera
        yaw += horizontalCameraSpeed * Input.GetAxis("Mouse X");
        pitch -= verticalCameraSpeed * Input.GetAxis("Mouse Y");

        if (pitch < pitchMin)
            pitch = pitchMin;
        else if (pitch > pitchMax)
            pitch = pitchMax;

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        //movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        Quaternion rotation = Quaternion.Euler(0, yaw, 0);
        charBody.Move(rotation * movement * moveSpeed);
        
    }
}
