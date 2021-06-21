using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainAngle : MonoBehaviour
{
    public float ZRotation;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, ZRotation);
    }
}
