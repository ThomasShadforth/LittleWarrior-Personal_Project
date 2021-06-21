using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuScreen, OptionsMenu, FirstSelectedMenu, FirstSelectedOptions, OptionsClosedButton;

    bool menuActive;

    public Animator optionsAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            if (!menuActive)
            {
                OpenMainMenu();
            }
        }
    }

    public void startGame()
    {
        StartCoroutine(gameStart());
    }

    public void OpenMainMenu()
    {
        menuActive = true;
        MainMenuScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(FirstSelectedMenu);
    }

    public void OpenCloseOptions()
    {
        if (!OptionsMenu.activeInHierarchy)
        {
            optionsAnimator.SetBool("isLoading", true);
            OptionsMenu.SetActive(true);

            EventSystem.current.SetSelectedGameObject(null);

            EventSystem.current.SetSelectedGameObject(FirstSelectedOptions);

            return;
        }
        else
        {
            optionsAnimator.SetBool("isLoading", false);

            StartCoroutine(closeOptions());

            

            
        }
    }

    public void CloseGame()
    {
        StartCoroutine(quitCo());
    }

    public IEnumerator quitCo()
    {
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }

    public IEnumerator closeOptions()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForSeconds(1f);
        OptionsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(OptionsClosedButton);
    }

    public IEnumerator gameStart()
    {
        UIFade.instance.fadeToBlack();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        UIFade.instance.fadeFromBlack();
    }

}
