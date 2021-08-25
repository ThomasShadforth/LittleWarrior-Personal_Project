using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayUI : MonoBehaviour
{
    public Image healthBar;
    public Text livesCounter;

    public static GameplayUI instance;

    bool hasChecked;
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

        updatePlayerHealthBar();
        updateLivesCounter();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasChecked)
        {
            updateLivesCounter();
            hasChecked = true;
        }
    }

    public void updatePlayerHealthBar()
    {
        healthBar.fillAmount = BasePlayer.instance.health / BasePlayer.instance.MaxHealth;
        

        if(healthBar.fillAmount <= .4f)
        {
            healthBar.color = Color.red;
        }
        else
        {
            healthBar.color = Color.green;
        }
    }

    public void updateLivesCounter()
    {
        livesCounter.text = "x " + GameManager.instance.remainingLives;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        updatePlayerHealthBar();
    }
}
