using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCatcher : MonoBehaviour {

#pragma warning disable 0649 // Disable "Field is never assigned" warning for SerializeField

    [SerializeField] private BallDispenser dispenser;
    [SerializeField] private float delay;

#pragma warning disable 0649

    private void Start() {
        if(!dispenser)
            dispenser = FindObjectOfType<BallDispenser>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<BallController>()) {
            StartCoroutine(MoveBall(other.gameObject));
        }
    }

    private IEnumerator MoveBall(GameObject ball) {
        ball.GetComponent<Rigidbody>().useGravity = false;
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        yield return new WaitForSeconds(delay);
        dispenser.MoveBall(ball);
    }

}