using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneHitbox : MonoBehaviour
{
    public bool isTouched;
    
    // Start is called before the first frame update
    void Start()
    {
        isTouched = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BowlingBall"){
            isTouched = true;
            print("TRIGGER HIT");
        }
    }
}
