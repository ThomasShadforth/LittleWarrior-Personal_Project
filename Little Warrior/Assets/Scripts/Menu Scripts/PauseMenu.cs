using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public static PauseMenu instance;

    public GameObject pauseMenu;

    public GameObject[] windows;
    public GameObject[] selectedButton;

    public ButtonImage[] UpgradeMenubuttons;
    public Sprite[] upgradeButtonSprites;
    // Start is called before the first frame update

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

        foreach(ButtonImage button in UpgradeMenubuttons)
        {
            button.GetComponent<Button>().image.sprite = selectButtonImage(GetComponent<RuntimeUpgradeData>().upgradeInfo[button.upgradeIndex].upgradeName);
        }
    }
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

    Sprite selectButtonImage(string upgradeName)
    {
        foreach(Sprite image in upgradeButtonSprites)
        {
            if(image.name.Contains(upgradeName))
            {
                return image;
            }
            else
            {
                
            }
        }
        return null;
    }
}
