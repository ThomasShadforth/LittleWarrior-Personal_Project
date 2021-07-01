using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    public Text upgradeName, upgradeDesc, upgradeCostText, upgradeButtonText;
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
        if(BasePlayer.instance.upgradePoints >= upgradeData.upgradeInfo[upgradeIndex].unlockCost)
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
        playerUpgradeSystem.playerUpgrade upgraded = upgradeData.upgradeInfo[selectedIndex];
        isUnlocked = upgraded.isUnlocked;
        
        if (isUnlocked)
        {
            if (upgraded.isAttack)
            {

            }
            else
            {

            }
        }
        else
        {
            if (upgraded.isAttack)
            {
                CombatSystem playerCombat = BasePlayer.instance.GetComponent<CombatSystem>();
                foreach(CombatAttacks attack in playerCombat.Attacks)
                {
                    if(attack.AttackName == upgraded.upgradeName)
                    {
                        Debug.Log("UPGRADE FOUND");
                        attack.isUnlocked = true;
                        playerCombat.removeEndOfString(attack.previousAttackName);
                        attack.endOfAttackString = true;
                        return;
                    }
                    else
                    {
                        Debug.Log("ATTACK NOT FOUND");
                    }
                }
            }
            else
            {

            }
                //Trigger the unlock function here
                //(Note, will need a check to determine if the unlocked upgrade was a stat upgrade or a combat move, perhaps add a boolean to the upgrade information that helps to provide this)
        } 
        
    }
}
