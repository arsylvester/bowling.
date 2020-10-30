using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pinScript : MonoBehaviour
{
    public GameObject pin;
    public Vector3 defaultPos;
    public Quaternion defaultRot;
    // Start is called before the first frame update
    void Start()
    {
        defaultPos = transform.position;
        defaultRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
