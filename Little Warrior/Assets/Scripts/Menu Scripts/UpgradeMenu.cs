using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    public Text upgradeName, upgradeDesc, upgradeCostText, upgradeButtonText, upgradePointText;
    public Button upgradeButton;
    public RuntimeUpgradeData upgradeData;
    bool isUnlocked;
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
        //if the player has enough points for an upgrade, the text will be displayed normally
        //If not, then the button will be greyed out
        if(GameManager.instance.getUpgradePoints() >= upgradeData.upgradeInfo[upgradeIndex].unlockCost)
        {
            upgradeButton.interactable = true;
            upgradeButton.image.color = Color.white;
        }
        else
        {
            upgradeButton.interactable = false;
            upgradeButton.image.color = Color.red;
        }
    }

    public void upgradeTrigger()
    {
        //gets the upgrade information from the upgrade data, such as the selected upgrade and whether it has already been unlocked
        playerUpgradeSystem.playerUpgrade upgraded = upgradeData.upgradeInfo[selectedIndex];
        isUnlocked = upgraded.isUnlocked;
        
        //Check for whether or not the upgrade has already been unlocked
        if (isUnlocked)
        {

            //If it is unlocked
            if (upgraded.isAttack)
            {
                CombatSystem playerCombat = BasePlayer.instance.GetComponent<CombatSystem>();

                foreach(CombatAttacks attack in playerCombat.Attacks)
                {
                    if(attack.AttackName == upgraded.upgradeName)
                    {
                        if (GameManager.instance.getUpgradePoints() > upgraded.unlockCost)
                        {
                            attack.damage += upgraded.upgradeTier[upgraded.upgradeLevel].upgradeIncrease;
                        }
                    }
                }
            }
            else
            {
                BasePlayer.instance.upgradeStat(upgraded.upgradeName, upgraded.upgradeTier[upgraded.upgradeLevel].upgradeIncrease);
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
                        Debug.Log("UPGRADE FOUND");
                        if (GameManager.instance.getUpgradePoints() > upgraded.unlockCost)
                        {
                            attack.isUnlocked = true;
                            upgraded.isUnlocked = true;
                            playerCombat.removeEndOfString(attack.previousAttackName);
                            attack.endOfAttackString = true;
                            Debug.Log(upgraded.unlockCost);
                            GameManager.instance.updateUpgradePoints(-upgraded.unlockCost);
                            updateUpgradePoints();
                            return;
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        
                    }
                }
            }
            else
            {
                upgraded.isUnlocked = true;
                BasePlayer.instance.upgradeStat(upgraded.upgradeName, upgraded.upgradeTier[upgraded.upgradeLevel].upgradeIncrease);
                upgradeData.updateUpgradeCost(selectedIndex);
            }
                
        } 
        
    }

    public void updateUpgradePoints()
    {
        upgradePointText.text = "Upgrade Points: " + GameManager.instance.getUpgradePoints();
    }
}
