using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventCamDestroy : MonoBehaviour
{

    public static PreventCamDestroy instance;

    
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
