using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : MonoBehaviour {

    [SerializeField] private GameObject lightObj;
    [SerializeField] private MeshRenderer materialObj;
    [SerializeField] private Material onMaterial, offMaterial;
    [SerializeField] private bool offByDefault;

    private bool isFlickering;

    private static bool arraysSet;
    private static Material[] onMaterialArray = new Material[2];
    private static Material[] offMaterialArray = new Material[2];

    private void Awake() {
        if(offByDefault)
            SetLight(false);
        isFlickering = false;

        if(!arraysSet) {
            arraysSet = true;
            onMaterialArray[0] = materialObj.material;
            onMaterialArray[1] = onMaterial;
            offMaterialArray[0] = materialObj.material;
            offMaterialArray[1] = offMaterial;
        }
    }

    public void SetLight(bool active) {
        /*if(active == lightObj.activeSelf)
            return;*/

        lightObj.SetActive(active);
        if(onMaterial && offMaterial) {
            if(active) {
                materialObj.materials = onMaterialArray;
            } else {
                materialObj.materials = offMaterialArray;
            }
        }
    }

    public void ToggleLight() {
        SetLight(!lightObj.activeSelf);
    }

    public bool IsLightOn() {
        return lightObj.activeSelf;
    }

    public bool FlickerLight(float length, int amount, float delay = 0f, bool toggle = false, bool onlyIfOn = false) {
        /*if(isFlickering)
            return false;*/
        if(onlyIfOn && IsLightOn())
            return false;
        StartCoroutine(Flicker(length, amount, delay, toggle));
        return true;
    }

    private IEnumerator Flicker(float length, int amount, float delay = 0f, bool toggle = false) {
        isFlickering = true;
        yield return new WaitForSecondsRealtime(delay);

        float pauseTime = length / (amount * 2);
        for(int i = 0; i < amount; i++) {
            yield return new WaitForSecondsRealtime(pauseTime);
            ToggleLight();
            yield return new WaitForSecondsRealtime(pauseTime);
            ToggleLight();
        }

        if(toggle) {
            yield return new WaitForSecondsRealtime(pauseTime);
            ToggleLight();
        }

        isFlickering = false;
    }

}