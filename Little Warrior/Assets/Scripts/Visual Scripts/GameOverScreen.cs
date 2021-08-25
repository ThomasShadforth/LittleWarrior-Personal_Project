using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public GameObject text1, text2, text3;
    public GameObject buttonPanel;
    Animator animator;

    public GameObject[] objectsToDestroy;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        objectsToDestroy = GameManager.instance.essentialObjects;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void fadeSecondText()
    {
        text1.SetActive(false);
        text2.SetActive(true);
        animator.SetBool("fadeSecond", true);
        
    }

    public void fadeGameOver()
    {
        text2.SetActive(false);
        text3.SetActive(true);
        buttonPanel.SetActive(true);
        animator.SetBool("fadeThird", true);
        
    }

    public void returnToLevel()
    {
        GameManager.instance.reloadFromGameOver();
    }

    public void returnToMenu()
    {
        StartCoroutine(loadMainMenu());
    }

    public IEnumerator loadMainMenu() {
        UIFade.instance.fadeToBlack();
        foreach(GameObject obj in objectsToDestroy)
        {
            if (obj.name != "Menu Fader")
            {
                Destroy(obj);
            }
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
        UIFade.instance.fadeFromBlack();
        
    }
}
