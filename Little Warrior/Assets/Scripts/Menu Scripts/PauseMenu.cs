using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;

    public GameObject[] windows;
    public GameObject[] selectedButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (!pauseMenu.activeInHierarchy)
            {
                pauseMenu.SetActive(true);
                loadPanel(0);
                GamePause.gamePaused = true;
                
            }
            else
            {
                resumeGame();
            }
        }
    }

    public void loadPanel(int windowIndex)
    {
        for(int i = 0; i < windows.Length; i++)
        {
            
            if(i == windowIndex)
            {
                windows[i].SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(selectedButton[i]);
                if (windows[i].name == "UpgradeMenu")
                {
                    GetComponent<UpgradeMenu>().updateUpgradePoints();
                    GetComponent<UpgradeMenu>().selectUpgrade(0);
                }
            }
            else
            {
                windows[i].SetActive(false);
            }
        }
    }

    public void resumeGame()
    {
        pauseMenu.SetActive(false);

        GamePause.gamePaused = false;
    }
}
