using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoader : MonoBehaviour
{
    public GameObject[] objectsToLoad;

    void Start()
    {
        foreach(GameObject essential in objectsToLoad)
        {
            Instantiate(essential);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
