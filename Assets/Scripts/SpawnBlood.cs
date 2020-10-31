using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlood : MonoBehaviour {

    [SerializeField] private List<GameObject> blood = new List<GameObject>();
    [SerializeField] private int frameNumber;

    private void Start() {
        GameStateController._instance.m_NewFrame.AddListener(() => {
            StartCoroutine(SpawnBloodFunc());
        });
    }

    private IEnumerator SpawnBloodFunc() {
        while(true) {
            if(GameStateController.GetCurrentFrame() >= frameNumber && PlayerController.lookingAt == 2) { // Looking at score screen
                foreach(GameObject obj in blood)
                    obj.SetActive(true);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

}