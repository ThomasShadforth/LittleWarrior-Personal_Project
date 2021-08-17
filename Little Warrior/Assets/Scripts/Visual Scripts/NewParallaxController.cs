using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewParallaxController : MonoBehaviour
{
    private float length, startPos;
    public GameObject camera;
    public float parallaxFactor;

    public bool axisY;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = (camera.transform.position.x * (1 - parallaxFactor));
        float distance = (camera.transform.position.x * parallaxFactor);

        transform.position = new Vector3(startPos + distance, axisY? camera.transform.position.y : transform.position.y, transform.position.z);

        if(temp > startPos + length)
        {
            startPos += length;
        } else if(temp < startPos - length)
        {
            startPos -= length;
        }
    }
}
