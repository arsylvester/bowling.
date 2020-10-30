using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsController : MonoBehaviour {

    public Lights[] lights;

    private void Awake() {
        if(lights.Length == 0)
            lights = FindObjectsOfType<Lights>();
    }

    private void Start() {
        GameStateController._instance.m_NewFrame.AddListener(FlickerOnNextFrame);
        StartCoroutine(FlickerRandomly());
    }

    private IEnumerator FlickerRandomly() {
        float waitTime = 20f;
        float length = 0.5f;
        int amount = 4;
        float delay = Random.Range(1f, 3f);

        while(true) {
            yield return new WaitForSecondsRealtime(waitTime);
            FlickerRandomLight(length, amount, delay);
        }
    }

    private void FlickerOnNextFrame() {
        //float randomModifier = Random.Range(1, GameStateController.GetCurrentFrame() + 1);
        float length = 0.5f;
        int amount = 4;
        float delay = Random.Range(1f, 3f);
        FlickerRandomLight(length, amount, delay);
    }

    public void FlickerLight(int index, float length = 0.5f, int amount = 4, float delay = 0f) {
        for(int i = 0; i < 4; i++) {
            if(lights[index].FlickerLight(length, amount, delay))
                break;
        }
    }

    public void FlickerRandomLight(float length = 0.5f, int amount = 4, float delay = 0f) {
        FlickerLight(Random.Range(0, lights.Length), length, amount, delay);
    }

}