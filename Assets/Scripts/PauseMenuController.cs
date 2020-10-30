using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour {

    [Header("Menu Pages")]
    [SerializeField] GameObject start;
    [SerializeField] GameObject controls;
    [SerializeField] GameObject credits;
    [SerializeField] GameObject quit;
    [SerializeField] GameObject title;
    [SerializeField] GameObject controlPage;
    [SerializeField] GameObject creditPage;
    private GameObject currentPage;

    [Header("Menu Objects")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject menuButtons;

    private CameraController cameraController;
    [HideInInspector] public bool paused;

    private void Start() {
        currentPage = title;
        paused = false;
        pauseMenu.SetActive(false);
        menuButtons.SetActive(false);
        cameraController = FindObjectOfType<CameraController>();
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            // Pause
            if(!paused)
                Pause();
            // Unpause
            else
                Unpause();
        }

        if(paused && Input.GetKeyDown(KeyCode.Mouse0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit)) {
                if(hit.transform.gameObject == start) {
                    print("resume");
                    Unpause();
                } else if(hit.transform.gameObject == controls) {
                    print("controls");
                    controlsPage();
                } else if(hit.transform.gameObject == credits) {
                    creditsPage();
                    print("credit");
                } else if(hit.transform.gameObject == quit) {
                    print("quit");
                    Application.Quit();
                }
            }
        }
    }

    private void Pause() {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        menuButtons.SetActive(true);
        cameraController.PauseCamera();
    }

    private void Unpause() {
        Time.timeScale = 1;
        menuButtons.SetActive(false);
        cameraController.UnpauseCamera();
        StartCoroutine(DisableMenuTimer(0.5f));
    }

    private IEnumerator DisableMenuTimer(float time) {
        yield return new WaitForSeconds(time);
        pauseMenu.SetActive(false);
    }

    private void controlsPage() {
        currentPage.SetActive(false);
        currentPage = controlPage;
        currentPage.SetActive(true);
    }

    private void creditsPage() {
        currentPage.SetActive(false);
        currentPage = creditPage;
        currentPage.SetActive(true);
    }

}