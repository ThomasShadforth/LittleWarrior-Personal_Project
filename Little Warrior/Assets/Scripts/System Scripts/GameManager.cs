using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//Game Manager is responsible for handling several key processes, such as keeping a record of the current upgrade points, current level, etc.
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public BasePlayer playerChar;
    public PauseMenu menuUI;
    int upgradePoints;
    public int remainingLives;

    public int indexToReload;
    public EssentialsLoader essentialLoaderObject;

    //Experimental
    public GameObject[] essentialObjects;
    bool hasSearched;

    // Start is called before the first frame update
    void Start()
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

    // Update is called once per frame
    void Update()
    {
        if (!hasSearched)
        {
            essentialObjects = new GameObject[essentialLoaderObject.objectsToLoad.Length];
            findEssentialObjects();
            hasSearched = true;
        }
    }

    public void findEssentialObjects()
    {
        for(int i = 0; i < essentialLoaderObject.objectsToLoad.Length; i++)
        {
            essentialObjects[i] = GameObject.Find(essentialLoaderObject.objectsToLoad[i].name);
        }
    }

    //[Dealing with Upgrade Points]
    public void updateUpgradePoints(int pointChange)
    {
        upgradePoints += pointChange;

    }

    public int getUpgradePoints()
    {
        return upgradePoints;
    }

    public void reloadAfterDeath()
    {
        
        StartCoroutine(reloadCo());
    }

    public void loadGameOver()
    {
        //Create a loop/method to disable all ddols/essentials (Except for the menu fade)
        foreach(GameObject essential in essentialObjects)
        {
            if(essential.name == "MenuFader" || essential.name == gameObject.name)
            {

            }
            else
            {
                essential.SetActive(false);
            }
        }
        //Store the index of the current scene
        indexToReload = SceneManager.GetActiveScene().buildIndex;
        //Load Game Over
        StartCoroutine(gameOver());
    }

    public IEnumerator reloadCo()
    {
        
        UIFade.instance.fadeToBlack();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return new WaitForSeconds(.2f);
        UIFade.instance.fadeFromBlack();
        BasePlayer.instance.health = BasePlayer.instance.MaxHealth;
        
    }

    public IEnumerator gameOver()
    {
        yield return new WaitForSeconds(0f);
        SceneManager.LoadScene("Game Over");
        UIFade.instance.fadeFromBlack();

    }

    public void reloadFromGameOver()
    {
        StartCoroutine(gameOverReloadCo());
    }
    
    public void loadLevel()
    {
        StartCoroutine(loadLevelCo());
    }

    public IEnumerator loadLevelCo()
    {
        GamePause.gamePaused = true;
        UIFade.instance.fadeToBlack();
        GetComponent<CheckpointHandler>().firstLoad = false;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        yield return new WaitForSeconds(1f);
        UIFade.instance.fadeFromBlack();
        yield return new WaitForSeconds(.3f);
        GamePause.gamePaused = false;
    }

    public IEnumerator gameOverReloadCo()
    {
        GamePause.gamePaused = true;
        UIFade.instance.fadeToBlack();
        GetComponent<CheckpointHandler>().firstLoad = false;
        yield return new WaitForSeconds(1f);
        foreach (GameObject essential in essentialObjects)
        {
            essential.SetActive(true);
        }
        SceneManager.LoadScene(indexToReload);
        yield return new WaitForSeconds(.5f);
        UIFade.instance.fadeFromBlack();
        yield return new WaitForSeconds(.3f);
        GamePause.gamePaused = false;
    }
}
