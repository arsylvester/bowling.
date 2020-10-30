using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevHitbox : MonoBehaviour
{
    public GameObject ScriptController;
    public GameObject touchedBy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BowlingBall"){
            if (touchedBy == null){
                touchedBy = other.gameObject;
            }
            ScriptController.GetComponent<scoreMaster> ().skipToFrameTen();
        }
    }
}
