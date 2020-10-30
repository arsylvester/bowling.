using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera bowlingCamera;
    [SerializeField] CinemachineVirtualCamera scoreCamera;
    [SerializeField] CinemachineVirtualCamera ballsCamera;
    private CinemachineVirtualCamera currentCamera;

    private PauseMenuController pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        currentCamera = bowlingCamera;
        currentCamera.gameObject.SetActive(true);

        pauseMenu = FindObjectOfType<PauseMenuController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!pauseMenu || !pauseMenu.paused) {
            if(currentCamera == bowlingCamera) {
                if(Input.GetKeyDown(KeyCode.Alpha1)) {
                    ChangeCamera(scoreCamera);
                } else if(Input.GetKeyDown(KeyCode.Alpha2)) {
                    ChangeCamera(ballsCamera);
                }
            } else {
                if(Input.GetKeyDown(KeyCode.Alpha1)) {
                    ChangeCamera(bowlingCamera);
                }
            }
        }
    }

    private void ChangeCamera(CinemachineVirtualCamera newCamera)
    {
        newCamera.gameObject.SetActive(true);
        currentCamera.gameObject.SetActive(false);
        currentCamera = newCamera;
    }

    public void PauseCamera() {
        ChangeCamera(scoreCamera);
    }

    public void UnpauseCamera() {
        ChangeCamera(bowlingCamera);
    }

}
