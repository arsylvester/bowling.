using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : MonoBehaviour {

    [SerializeField] private GameObject lightObj;
    [SerializeField] private MeshRenderer materialObj;
    [SerializeField] private Material onMaterial, offMaterial;
    [SerializeField] private bool offByDefault;

    private bool isFlickering;

    private void Awake() {
        if(offByDefault)
            SetLight(false);
        isFlickering = false;
    }

    public void SetLight(bool active) {
        if(active == lightObj.activeSelf)
            return;

        lightObj.SetActive(active);
        if(onMaterial && offMaterial) {
            if(active)
                materialObj.material = onMaterial;
            else
                materialObj.material = offMaterial;
        }
    }

    public void ToggleLight() {
        SetLight(!lightObj.activeSelf);
    }

    public bool IsLightOn() {
        return lightObj.activeSelf;
    }

    public bool FlickerLight(float length, int amount, float delay = 0f) {
        if(isFlickering)
            return false;
        StartCoroutine(Flicker(length, amount, delay));
        return true;
    }

    private IEnumerator Flicker(float length, int amount, float delay = 0f) {
        isFlickering = true;
        yield return new WaitForSecondsRealtime(delay);

        float pauseTime = length / (amount * 2);
        for(int i = 0; i < amount; i++) {
            yield return new WaitForSecondsRealtime(pauseTime);
            ToggleLight();
            yield return new WaitForSecondsRealtime(pauseTime);
            ToggleLight();
        }
        isFlickering = false;
    }

}