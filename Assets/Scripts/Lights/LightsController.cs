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
        StartCoroutine(FlickerRandomly(true));
        StartCoroutine(FlickerRandomly(false));
    }

    private IEnumerator FlickerRandomly(bool onlyIfOn) {
        float waitTime;
        float length = Random.Range(0.3f, 0.75f);
        int amount = Random.Range(3, 6);
        float delay = Random.Range(1f, 3f);

        while(true) {
            waitTime = Random.Range(15f, 20f);
            yield return new WaitForSecondsRealtime(waitTime);
            FlickerRandomLight(length, amount, delay, false, onlyIfOn);
        }
    }

    private void FlickerOnNextFrame() {
        //float randomModifier = Random.Range(1, GameStateController.GetCurrentFrame() + 1);
        float length = 0.5f;
        int amount = 4;
        float delay = Random.Range(1f, 3f);

        if(GameStateController.GetCurrentFrame() % 2 == 1 && GameStateController.GetCurrentFrame() <= 5) { // 2, 4, 6 - flicker and turn off
            TurnOffRandom(length, amount, delay, GameStateController.GetCurrentFrame() / 2);
        } else if(GameStateController.GetCurrentFrame() == 7 || GameStateController.GetCurrentFrame() == 8) {
            int index;
            for(int i = 0; i < 10; i++) {
                index = Random.Range(0, 9);
                if(lights[index].IsLightOn()) {
                    FlickerLight(index, length, amount, delay, true);
                    break;
                }
            }
        } else // Odd - flicker
            FlickerRandomLight(length, amount, delay);
    }

    private void TurnOffRandom(float length, int amount, float delay, int column = -1) {
        int index;
        if(column == -1)
            index = Random.Range(0, 9);
        else {
            index = (Random.Range(0, 3) * 3) + column;
        }
        FlickerLight(index, length, amount, delay, true);
    }

    public void FlickerLight(int index, float length = 0.5f, int amount = 4, float delay = 0f, bool toggle = false, bool onlyIfOn = false) {
        lights[index].FlickerLight(length, amount, delay, toggle, onlyIfOn);
    }

    public void FlickerRandomLight(float length = 0.5f, int amount = 4, float delay = 0f, bool toggle = false, bool onlyIfOn = false) {
        for(int i = 0; i < 4; i++) {
            if(lights[Random.Range(0, lights.Length)].FlickerLight(length, amount, delay, toggle, onlyIfOn))
                break;
        }
    }

}