using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlood : MonoBehaviour {

    [SerializeField] private List<GameObject> blood = new List<GameObject>();
    [SerializeField] private int frameNumber;
    [SerializeField] private bool onScore, onThrow, strongMan;

    private void Start() {
        if(!strongMan) {
            GameStateController._instance.m_NewFrame.AddListener(() => {
                StartCoroutine(SpawnBloodFunc());
            });
        } else {
            // summon him here
        }
    }

    private IEnumerator SpawnBloodFunc() {
        while(true) {
            if(GameStateController.GetCurrentFrame() >= frameNumber) { // In frame
                if((onScore && PlayerController.lookingAt == 2) || // Looking at score screen
                    (onThrow && PlayerController.lookingAt == 0)) // Looking at throw
                    foreach(GameObject obj in blood)
                        obj.SetActive(true);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void SpawnDudeFunction() {
        blood[0].SetActive(true);
    }

}