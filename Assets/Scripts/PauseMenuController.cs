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
    [SerializeField] private GameObject scoreboard;

    private PlayerController cam;
    [HideInInspector] public bool paused;

    private void Start() {
        currentPage = title;
        paused = false;
        pauseMenu.SetActive(false);
        menuButtons.SetActive(false);
        cam = FindObjectOfType<PlayerController>();
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Debug.Log("test");
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
        //Time.timeScale = 0;
        paused = true;
        Debug.Log("pause");
        pauseMenu.SetActive(true);
        scoreboard.SetActive(false);
        menuButtons.SetActive(true);
        cam.ChangeCameraToScore();
    }

    public void Unpause() {
        //Time.timeScale = 1;
        paused = false;
        Debug.Log("unpause");
        pauseMenu.SetActive(false);
        scoreboard.SetActive(true);
        menuButtons.SetActive(false);

        currentPage = title;
        title.SetActive(true);
        controlPage.SetActive(false);
        creditPage.SetActive(false);
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