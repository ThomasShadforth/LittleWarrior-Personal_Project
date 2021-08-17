using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialLevel : MonoBehaviour
{
    public static TutorialLevel instance;

    //Stores the UI pop-ups that appear during the tutorial
    public GameObject popupUIPanel;
    public GameObject[] TutPopUps;
    public GameObject enemyToSpawn;

    [SerializeField]
    GameObject[] objectsToDestroy;

    [SerializeField]
    CheckpointSystem finalTutCheckpoint;
    GameObject tutorialEnemy;
    private int popUpIndex;
    bool doesPopHaveConditions;
    bool isPopUpActive, enemySpawned;

    [Header("Pop Up Conditions")]
    bool leftPressed, rightPressed, jumpPressed, lightAttPressed, heavyAttPressed, enemyDestroyed, finalCheckpointReached;

    void Start()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        openTutorialPopUp(0, transform.position, false);
    }

    // Update is called once per frame
    void Update()
    {
        checkPopupStatus();
    }

    void checkPopupStatus()
    {
        if (isPopUpActive)
        {
            
            if (!doesPopHaveConditions)
            {
                if (Input.anyKey)
                {

                    TutPopUps[popUpIndex].SetActive(false);
                    isPopUpActive = false;

                    if (popUpIndex == 0)
                    {

                        openTutorialPopUp(1, transform.position, true);
                        return;
                    }
                    else
                    {
                        
                        popupUIPanel.SetActive(false);
                    }
                }
            }
            else
            {
                bool conditionFulfilled = checkPopUpCondition();
                if (conditionFulfilled)
                {
                    if (popUpIndex != 7)
                    {
                        popupUIPanel.SetActive(false);
                    }
                    TutPopUps[popUpIndex].SetActive(false);
                    isPopUpActive = false;

                    if(popUpIndex == 4)
                    {
                        openTutorialPopUp(5, transform.position, false);
                    }
                }
            }
        }
    }

    public void openTutorialPopUp(int index, Vector3 spawnPosition, bool hasCondition)
    {
        
        popUpIndex = index;
        isPopUpActive = true;
        Debug.Log(popUpIndex);
        doesPopHaveConditions = hasCondition;
        popupUIPanel.SetActive(true);
        TutPopUps[popUpIndex].SetActive(true);
        if(popUpIndex == 4 && !enemySpawned)
        {
            tutorialEnemy = Instantiate(enemyToSpawn, new Vector3(spawnPosition.x + 1, spawnPosition.y, spawnPosition.z), Quaternion.identity);
            enemySpawned = true;
        }
    }

    public IEnumerator endOfTutorial()
    {
        UIFade.instance.fadeToBlack();
        yield return new WaitForSeconds(.3f);
        foreach(GameObject tutorialObject in objectsToDestroy)
        {
            Destroy(tutorialObject);
        }
        yield return new WaitForSeconds(1f);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        UIFade.instance.fadeFromBlack();
    }

    

    bool checkPopUpCondition()
    {
        if(popUpIndex == 1)
        {
            if (Input.GetButtonDown("Left"))
            {
                leftPressed = true;
            }

            if (Input.GetButtonDown("Right"))
            {
                rightPressed = true;
            }

            if(leftPressed && rightPressed)
            {
                return true;
            }
        } else if (popUpIndex == 2)
        {
            if (Input.GetButtonDown("Jump"))
            {
                jumpPressed = true;
            }

            if (jumpPressed)
            {
                return true;
            }
        } else if (popUpIndex == 3)
        {
            if (Input.GetButtonDown("LightAtt"))
            {
                lightAttPressed = true;
            }

            if (Input.GetButtonDown("HeavyAtt"))
            {
                
                heavyAttPressed = true;
            }

            if(lightAttPressed && heavyAttPressed)
            {
                return true;
            }
        } else if (popUpIndex == 4)
        {
            if(tutorialEnemy == null)
            {
                enemyDestroyed = true;
            }

            if (enemyDestroyed)
            {
                return true;
            }
        } else if (popUpIndex == 6)
        {
            if (finalTutCheckpoint.checkpointSet)
            {
                finalCheckpointReached = true;
            }

            if (finalCheckpointReached)
            {
                popUpIndex = 7;
                
                return true;
            }
        }

        return false;
    }

    public IEnumerator finalPopUp()
    {
        
        TutPopUps[7].SetActive(true);
        yield return new WaitForSeconds(3f);
        StartCoroutine(endOfTutorial());
    }
}
