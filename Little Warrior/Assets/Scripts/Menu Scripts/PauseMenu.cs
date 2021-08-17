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

    public ButtonImage[] upgradeButtons;
    public Sprite[] upgradeButtonSprites;

    public Text characterHealth, characterAttack, characterDefense;
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


    }

    void Start()
    {
        foreach(ButtonImage button in upgradeButtons)
        {
            button.GetComponent<Button>().image.sprite = setButtonSprite(GetComponent<UpgradeMenu>().upgradeData.upgradeInfo[button.UpgradeIndex].upgradeName);
        }
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
                updateCharacterDetails();
                GamePause.gamePaused = true;
                
            }
            else
            {
                SoundManager.instance.playSFX("Button");
                resumeGame();
            }
        }
    }

    public void loadPanel(int windowIndex)
    {
        SoundManager.instance.playSFX("Button");
        for (int i = 0; i < windows.Length; i++)
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
                } else if(windows[i].name == "CharacterMenu")
                {
                    updateCharacterDetails();
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

    public Sprite setButtonSprite(string upgradeName)
    {
        /*foreach(Sprite buttonSprite in upgradeButtonSprites)
        {
            
            if (buttonSprite.name.Contains(upgradeName))
            {
                
                return buttonSprite;
            }
        }*/

        for(int i = 0; i < upgradeButtonSprites.Length; i++)
        {
            Sprite buttonSprite = upgradeButtonSprites[i];
            if (buttonSprite.name.Contains(upgradeName))
            {
                return buttonSprite;
            }
        }

        return null;
    }

    public void updateCharacterDetails()
    {
        characterHealth.text = BasePlayer.instance.health + " / " + BasePlayer.instance.MaxHealth;
        characterAttack.text = "" + BasePlayer.instance.baseAtk;
        characterDefense.text = "" + BasePlayer.instance.playerDef;
    }
}
