using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class NewParallaxController : MonoBehaviour
{
    private float length, startPos;
    public CinemachineTargetSet camera;
    public GameObject cam;
    public float parallaxFactor;

    public bool axisY;
    // Start is called before the first frame update
    void Start()
    {
        
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            camera = CinemachineTargetSet.instance;
        }
        else
        {
            cam = FindObjectOfType<CinemachineVirtualCamera>().gameObject;
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            float temp = (camera.transform.position.x * (1 - parallaxFactor));
            float distance = (camera.transform.position.x * parallaxFactor);

            transform.position = new Vector3(startPos + distance, axisY ? camera.transform.position.y : transform.position.y, transform.position.z);

            if (temp > startPos + length)
            {
                startPos += length;
            }
            else if (temp < startPos - length)
            {
                startPos -= length;
            }
        }
        else
        {
            float temp = (cam.transform.position.x * (1 - parallaxFactor));
            float distance = (cam.transform.position.x * parallaxFactor);

            transform.position = new Vector3(startPos + distance, axisY ? cam.transform.position.y : transform.position.y, transform.position.z);

            if (temp > startPos + length)
            {
                startPos += length;
            }
            else if (temp < startPos - length)
            {
                startPos -= length;
            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("LOADED");
        
    }
}
