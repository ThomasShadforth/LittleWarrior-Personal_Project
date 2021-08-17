using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    public Image healthBar;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updatePlayerHealthBar();
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
}
