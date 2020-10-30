using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject start;
    [SerializeField] GameObject controls;
    [SerializeField] GameObject credits;
    [SerializeField] GameObject quit;
    [SerializeField] GameObject title;
    [SerializeField] GameObject controlPage;
    [SerializeField] GameObject creditPage;
    private GameObject currentPage;

    private void Start()
    {
        currentPage = title;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.transform.gameObject == start)
                {
                    print("start");
                    SceneManager.LoadScene("_Main");
                }
                else if (hit.transform.gameObject == controls)
                {
                    print("controls");
                    controlsPage();
                }
                else if (hit.transform.gameObject == credits)
                {
                    creditsPage();
                    print("credit");
                }
                else if (hit.transform.gameObject == quit)
                {
                    print("quit");
                    Application.Quit();
                }
            }
        }
    }

    private void controlsPage()
    {
        currentPage.SetActive(false);
        currentPage = controlPage;
        currentPage.SetActive(true);
    }

    private void creditsPage()
    {
        currentPage.SetActive(false);
        currentPage = creditPage;
        currentPage.SetActive(true);
    }
}
