using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointHandler : MonoBehaviour
{
    public Vector2 checkpointPos;

    bool firstLoad;
    // Start is called before the first frame update
    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += onSceneLoaded;
    }

    void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!firstLoad)
        {
            
            checkForFirstSpawn();
        }
        else
        {
            BasePlayer.instance.transform.position = checkpointPos;
        }
    }

    void Start()
    {

        Invoke("setStartPos", .2f);
    }

    // Update is called once per frame
    void Update()
    {
        

        
    }

    public void checkForFirstSpawn()
    {
        checkpointPos = GameObject.Find("First_Spawn").transform.position;
        firstLoad = true;
    }

    void setStartPos()
    {
        BasePlayer.instance.transform.position = checkpointPos;
    }

    
}
