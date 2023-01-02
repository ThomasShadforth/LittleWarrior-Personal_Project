using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class is used to store the runtime upgrade data
public class RuntimeUpgradeData : MonoBehaviour
{
    //Stores the original data
    public playerUpgradeSystem.PlayerUpgradeData originalData;
    //Used to store the upgrade info in a separate array
    public playerUpgradeSystem.playerUpgrade[] upgradeInfo;

    private void Awake()
    {
        
        upgradeInfo = originalData.upgrades;

        //Cycles through and sets to default values
        for (int i = 0; i < upgradeInfo.Length; i++)
        {
            //Sets level to default, and cost to default
            upgradeInfo[i].upgradeLevel = 0;
            upgradeInfo[i].unlockCost = upgradeInfo[i].upgradeTier[upgradeInfo[i].upgradeLevel].upgradeCost;
            if (upgradeInfo[i].isAttack)
            {
                //If the attack in question is meant to be used from the start of the game, then unlock, otherwise, leave it locked
                if (upgradeInfo[i].isStarterAttack)
                {
                    upgradeInfo[i].isUnlocked = true;
                }
                else
                {
                    upgradeInfo[i].isUnlocked = false;
                }
            }
            else
            {
                upgradeInfo[i].isUnlocked = false;
            }
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Updates the upgrade cost
    public void updateUpgradeCost(int upgradeIndex)
    {
        upgradeInfo[upgradeIndex].unlockCost = upgradeInfo[upgradeIndex].upgradeTier[upgradeInfo[upgradeIndex].upgradeLevel].upgradeCost;
    }
}
