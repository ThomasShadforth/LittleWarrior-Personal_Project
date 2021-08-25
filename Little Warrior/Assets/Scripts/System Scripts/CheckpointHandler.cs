using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointHandler : MonoBehaviour
{
    public Vector2 checkpointPos;

    public bool firstLoad;
    bool firstPos;
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

            StartCoroutine(setStartCo());
            
        }
        else
        {
            if (SceneManager.GetActiveScene().name != "Tutorial Level")
            {
                BasePlayer.instance.transform.position = checkpointPos;
            }
        }
    }

    void Start()
    {
        
        
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

    

    public IEnumerator setStartCo()
    {
        checkForFirstSpawn();
        yield return new WaitForSeconds(0.1f);
        if (SceneManager.GetActiveScene().name != "Tutorial Level")
        {
            BasePlayer.instance.transform.position = checkpointPos;
        }
    }
    
}
