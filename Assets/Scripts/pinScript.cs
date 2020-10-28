using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pinScript : MonoBehaviour
{
    public GameObject pin;
    bool isKnocked;
    // Start is called before the first frame update
    void Start()
    {
        isKnocked = false;
    }

    // Update is called once per frame
    void Update()
    {
        float rotX = pin.transform.rotation.eulerAngles.x;
        float rotY = pin.transform.rotation.eulerAngles.y;
        float rotZ = pin.transform.rotation.eulerAngles.z;
        // if (Mathf.Abs(rotX) < (360 - 1.5) || Mathf.Abs(rotZ) > 1.5) {
        //     isKnocked = true;
        //     print (Mathf.Abs(rotX) + ", " + Mathf.Abs(rotZ));
        // }
        if (Mathf.Abs(rotX) < 358.5 && Mathf.Abs(rotX) > 1.5){
            isKnocked = true;
            print ("rotX triggered: " + Mathf.Abs(rotX));
        }
        else if (Mathf.Abs(rotZ) < 358.5 && Mathf.Abs(rotZ) > 1.5) {
            isKnocked = true;
            print ("rotZ triggered: " + Mathf.Abs(rotZ));
        }
    }
}
