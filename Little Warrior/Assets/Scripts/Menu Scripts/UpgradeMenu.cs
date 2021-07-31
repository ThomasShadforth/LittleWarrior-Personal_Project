using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    //References to menu text items
    public Text upgradeName, upgradeDesc, upgradeCostText, upgradeButtonText, upgradePointText;
    //The upgradeButton, used to alter the colour and set interactible
    public Button upgradeButton;
    //The runtime version of the upgrade data
    public RuntimeUpgradeData upgradeData;
    //If the upgrade is unlocked or not
    bool isUnlocked;
    //The currently selected upgrade's index
    int selectedIndex;
    // Start is called before the first frame update
    void Start()
    {
        upgradeData = GetComponent<RuntimeUpgradeData>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setDefaultSelection()
    {

    }

    private void FixedUpdate()
    {
        //Checks the current upgrade points stored in the game manager
        if (GameManager.instance.getUpgradePoints() >= upgradeData.upgradeInfo[selectedIndex].unlockCost)
        {
            //If the player has enough, make the button interactable, set the colour to white and the text accordingly
            upgradeButton.interactable = true;
            upgradeButton.image.color = Color.white;
            if (upgradeData.upgradeInfo[selectedIndex].isUnlocked)
            {
                upgradeButtonText.text = "Upgrade";
            }
            else
            {
                upgradeButtonText.text = "Unlock";
            }
        }
        //otherwise, set the button to red, make it impossible to interact with the button, etc.
        else
        {
            upgradeButton.interactable = false;
            upgradeButton.image.color = Color.red;
            upgradeButtonText.text = "Cannot Upgrade";
        }
    }

    public void selectUpgrade(int upgradeIndex)
    {
        //Insert the following within the block:
        //The Upgrade/Move name
        //Description
        //Cost
        //Whether or not the upgrade can be bought (This will be reflected in the button colour/text)
        Debug.Log("Upgrade in slot " + upgradeIndex);
        upgradeName.text = "" + upgradeData.upgradeInfo[upgradeIndex].upgradeName;
        upgradeDesc.text = "" + upgradeData.upgradeInfo[upgradeIndex].description;
        upgradeCostText.text = "" + upgradeData.upgradeInfo[upgradeIndex].unlockCost.ToString();
        selectedIndex = upgradeIndex;
        
        
    }

    public void upgradeTrigger()
    {
        //gets the upgrade information from the upgrade data, such as the selected upgrade and whether it has already been unlocked
        playerUpgradeSystem.playerUpgrade upgraded = upgradeData.upgradeInfo[selectedIndex];
        isUnlocked = upgraded.isUnlocked;
        
        //Check for whether or not the upgrade has already been unlocked
        if (isUnlocked)
        {
            

            //If it is unlocked, check if it's an attack
            if (upgraded.isAttack)
            {
                //Gets the respective combat system info from the player's component
                CombatSystem playerCombat = BasePlayer.instance.GetComponent<CombatSystem>();

                //Cycles through each attack to compare to the selected upgrade's name
                foreach(CombatAttacks attack in playerCombat.Attacks)
                {
                    if(attack.AttackName == upgraded.upgradeName)
                    {

                        if (GameManager.instance.getUpgradePoints() > upgraded.unlockCost)
                        {
                            //Increase the damage, upgrade level, and update the cost and remaining upgrade points on the screen
                            GameManager.instance.updateUpgradePoints(-upgraded.unlockCost);
                            attack.damage += upgraded.upgradeTier[upgraded.upgradeLevel].upgradeIncrease;
                            upgraded.upgradeLevel++;
                            upgradeData.updateUpgradeCost(selectedIndex);
                            updateUpgradePoints();
                            return;
                        }
                    }
                }
                
            }
            else
            {
                //Pass the upgrade name and the stat increase (Based on the upgrade level) to a method in the player character that increases stats
                GameManager.instance.updateUpgradePoints(-upgraded.unlockCost);
                upgraded.upgradeLevel++;
                BasePlayer.instance.upgradeStat(upgraded.upgradeName, upgraded.upgradeTier[upgraded.upgradeLevel].upgradeIncrease);
                upgradeData.updateUpgradeCost(selectedIndex);
                updateUpgradePoints();
                return;
            }
        }
        else
        {
            //If the upgrade isn't unlocked
            if (upgraded.isAttack)
            {
                
                //If the upgrade is an attack, get the combat system and store it in a variable
                CombatSystem playerCombat = BasePlayer.instance.GetComponent<CombatSystem>();
                //Necessary in order to cycle through the player's attacks to find the upgrade
                foreach(CombatAttacks attack in playerCombat.Attacks)
                {
                    if(attack.AttackName == upgraded.upgradeName)
                    {
                        
                        if (GameManager.instance.getUpgradePoints() > upgraded.unlockCost)
                        {
                            //Same as above, except unlocking
                            //Remove the previous attack's end of string bool
                            attack.isUnlocked = true;
                            upgraded.isUnlocked = true;
                            playerCombat.removeEndOfString(attack.previousAttackName);
                            attack.endOfAttackString = true;
                            upgraded.upgradeLevel++;
                            GameManager.instance.updateUpgradePoints(-upgraded.unlockCost);
                            updateUpgradePoints();
                            return;
                        }
                        
                    }
                    
                }
            }
            else
            {
                GameManager.instance.updateUpgradePoints(-upgraded.unlockCost);
                upgraded.isUnlocked = true;
                upgraded.upgradeLevel++;
                BasePlayer.instance.upgradeStat(upgraded.upgradeName, upgraded.upgradeTier[upgraded.upgradeLevel].upgradeIncrease);
                upgradeData.updateUpgradeCost(selectedIndex);
                updateUpgradePoints();
                return;
            }
                
        } 
        
    }

    //Refreshes the upgrade points the player has on hand
    public void updateUpgradePoints()
    {
        upgradePointText.text = "Upgrade Points: " + GameManager.instance.getUpgradePoints();
    }
}
