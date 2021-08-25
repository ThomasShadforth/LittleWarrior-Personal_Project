using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class CinemachineTargetSet : MonoBehaviour
{
    CinemachineVirtualCamera cam;
    bool loadedFirst;

    public static CinemachineTargetSet instance;

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

    private void OnEnable()
    {
        
        cam = GetComponent<CinemachineVirtualCamera>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        cam.Follow = BasePlayer.instance.transform;
    }
}
