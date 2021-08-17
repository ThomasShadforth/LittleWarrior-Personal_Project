using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    float distanceTravelled;
    Vector3 originalPos;
    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x + 2 * Time.deltaTime, transform.position.y, transform.position.z);
        distanceTravelled += 2 * Time.deltaTime;

        if(distanceTravelled >= 50)
        {
            transform.position = originalPos;
            distanceTravelled = 0;
        }
    }
}
