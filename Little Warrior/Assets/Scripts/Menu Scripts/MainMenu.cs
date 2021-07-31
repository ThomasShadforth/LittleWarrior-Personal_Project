using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Stores references to different areas of the main menu
    public GameObject MainMenuScreen, OptionsMenu, FirstSelectedMenu, FirstSelectedOptions, OptionsClosedButton, gameTitleScreen;

    //Is the menu open
    bool menuActive;

    //Animates the transitions for the options menu
    public Animator optionsAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If a key is pressed and the menu isn't active, open the main menu
        if (Input.anyKey)
        {
            if (!menuActive)
            {
                OpenMainMenu();
            }
        }
    }

    //Load the first level
    public void startGame()
    {
        StartCoroutine(gameStart());
    }

    //Executes a menu coroutine that hides the title and displays the menu options
    public void OpenMainMenu()
    {
        StartCoroutine(OpenMenu());
        
    }

    //opens/closes the options menu depending on whether it is currently open
    public void OpenCloseOptions()
    {
        if (!OptionsMenu.activeInHierarchy)
        {
            //set animator parameter to true
            optionsAnimator.SetBool("isLoading", true);
            //Set the menu to active
            OptionsMenu.SetActive(true);

            //Clear the currently selected object
            EventSystem.current.SetSelectedGameObject(null);
            //Set the selected object to the default selection for the options menu
            EventSystem.current.SetSelectedGameObject(FirstSelectedOptions);

            return;
        }
        else
        {
            optionsAnimator.SetBool("isLoading", false);

            StartCoroutine(closeOptions());

            

            
        }
    }

    //Quits the game
    public void CloseGame()
    {
        StartCoroutine(quitCo());
    }

    public IEnumerator quitCo()
    {
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }


    //Clears the selected button, and sets it to the options button when the main menu is available again
    public IEnumerator closeOptions()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForSeconds(1f);
        OptionsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(OptionsClosedButton);
    }

    //Fade to black, then load the scene before fading into it
    public IEnumerator gameStart()
    {
        UIFade.instance.fadeToBlack();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        UIFade.instance.fadeFromBlack();
    }

    
    public IEnumerator OpenMenu()
    {
        //Set isPressed to true
        gameTitleScreen.GetComponent<Animator>().SetBool("isPressed", true);
        //Wait for a second for the title to disappear
        yield return new WaitForSeconds(1f);
        //Set the title to inactive, and set the main menu to being active
        gameTitleScreen.SetActive(false);
        menuActive = true;
        MainMenuScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(FirstSelectedMenu);
    }
}
