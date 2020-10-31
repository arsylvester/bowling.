using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsController : MonoBehaviour {

    public Lights[] lights;

    [SerializeField] private GameObject screen, screenLight;

    private bool pauseFlickerLoop;

    private void Awake() {
        if(lights.Length == 0)
            lights = FindObjectsOfType<Lights>();
        pauseFlickerLoop = false;
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
            if(pauseFlickerLoop)
                continue;
            FlickerRandomLight(length, amount, delay, false, onlyIfOn);
        }
    }

    private void FlickerOnNextFrame() {
        //float randomModifier = Random.Range(1, GameStateController.GetCurrentFrame() + 1);
        float length = 0.5f;
        int amount = 4;
        float delay = Random.Range(1f, 3f);

        if(GameStateController.GetCurrentFrame() % 2 == 0 && GameStateController.GetCurrentFrame() <= 5) { // 2, 4, 6 - flicker and turn off
            TurnOffRandom(length, amount, delay);
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

    private void TurnOffRandom(float length, int amount, float delay) {
        int index = 0;
        for(int i = 0; i < 16; i++) {
            index = Random.Range(0, 9);
            if(index % 3 != 1 || !lights[index].IsLightOn()) // Don't turn off center lane
                continue;
        }
        FlickerLight(index, length, amount, delay, true);
    }

    public IEnumerator FlashAllLights(float pauseDelay = 2f) {
        pauseFlickerLoop = true;
        yield return FlashAllLightsOff();

        yield return new WaitForSeconds(pauseDelay);

        yield return FlashAllLightsOn();
        pauseFlickerLoop = false;
        yield return null;
    }

    public IEnumerator FlashAllLightsOff() {
        // Flicker monitor off
        StartCoroutine(FlickerMonitor(0.75f, 6, 0.5f, false));
        // Flicker all lights off
        for(int i = 0; i < lights.Length; i++) {
            lights[i].SetLight(true);
            FlickerLight(i, 0.5f, 4, 0f, true);
            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
        }

        yield return new WaitForSeconds(0.8f);
        for(int i = 0; i < lights.Length; i++) {
            lights[i].SetLight(false);
        }
        yield return null;
    }

    public IEnumerator FlashAllLightsOn() {
        // Flicker monitor on
        StartCoroutine(FlickerMonitor(0.75f, 6, 0.5f, true));
        // Flicker all lights on
        for(int i = 0; i < lights.Length; i++) {
            FlickerLight(i, 0.5f, 4, 0f, true);
            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
        }

        yield return new WaitForSeconds(0.8f);
        for(int i = 0; i < lights.Length; i++)
            lights[i].SetLight(true);
        yield return null;
    }

    public void FlashAllWrapper() { StartCoroutine(FlashAllLights()); }

    private IEnumerator FlickerMonitor(float length, int amount, float delay = 0f, bool finalState = false) {
        yield return new WaitForSecondsRealtime(delay);

        float pauseTime = length / (amount * 2);
        for(int i = 0; i < amount; i++) {
            yield return new WaitForSecondsRealtime(pauseTime);
            screen.SetActive(false);
            screenLight.SetActive(false);
            yield return new WaitForSecondsRealtime(pauseTime);
            screen.SetActive(true);
            screenLight.SetActive(true);
        }

        yield return new WaitForSecondsRealtime(pauseTime);
        screen.SetActive(finalState);
        screenLight.SetActive(finalState);
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