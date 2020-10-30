using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScreenTrigger : MonoBehaviour
{
    [SerializeField] RenderSelector tvScreen;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "BowlingBall")
        {
            tvScreen.cameraSwapToBall();
        }
    }
}
