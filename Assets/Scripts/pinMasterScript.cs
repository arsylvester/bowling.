using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pinMasterScript : MonoBehaviour {
    public GameObject[] pins;
    public bool exampleMethodCall;
    public GameObject[] knocked;

    // Start is called before the first frame update
    void Start () {
        exampleMethodCall = false;
    }

    // Update is called once per frame
    void Update () {
        if (exampleMethodCall){
            knocked = getKnocked();
            string sampleText = "Knocked: ";
            foreach (GameObject PAIN in knocked)
            {
                if (PAIN != null){
                    sampleText += (PAIN.name + ", ");
                }   
            }
            print(sampleText);
            exampleMethodCall = false;
        }
    }

    public GameObject[] getKnocked () { //I can rewrite this to just update knocked[]
        int k = 0;
        GameObject[] knockedPins = new GameObject[pins.Length];

        foreach (GameObject p in pins) {
            bool isKnocked = false;

            float rotX = p.transform.rotation.eulerAngles.x;
            float rotY = p.transform.rotation.eulerAngles.y;
            float rotZ = p.transform.rotation.eulerAngles.z;
            
            if (Mathf.Abs (rotX) < 358.5 && Mathf.Abs (rotX) > 1.5) {
                isKnocked = true;
                print ("rotX triggered: " + Mathf.Abs (rotX));
            } else if (Mathf.Abs (rotZ) < 358.5 && Mathf.Abs (rotZ) > 1.5) {
                isKnocked = true;
                print ("rotZ triggered: " + Mathf.Abs (rotZ));
            }

            if (isKnocked){
                knockedPins[k++] = p;
            }
        }

        return knockedPins;
    }

    //Testing to see if an int return is more useful
    public int getKnockedInt () {
        int k = 0;
        GameObject[] knockedPins = new GameObject[pins.Length];

        foreach (GameObject p in pins) {
            bool isKnocked = false;

            float rotX = p.transform.rotation.eulerAngles.x;
            float rotY = p.transform.rotation.eulerAngles.y;
            float rotZ = p.transform.rotation.eulerAngles.z;
            
            if (Mathf.Abs (rotX) < 358.5 && Mathf.Abs (rotX) > 1.5) {
                isKnocked = true;
                print ("rotX triggered: " + Mathf.Abs (rotX));
            } else if (Mathf.Abs (rotZ) < 358.5 && Mathf.Abs (rotZ) > 1.5) {
                isKnocked = true;
                print ("rotZ triggered: " + Mathf.Abs (rotZ));
            }

            if (isKnocked){
                knockedPins[k++] = p;
            }
        }

        return k;
    }
}