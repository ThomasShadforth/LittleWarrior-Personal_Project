using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeUpgradeData : MonoBehaviour
{
    public playerUpgradeSystem.PlayerUpgradeData originalData;
    public playerUpgradeSystem.playerUpgrade[] upgradeInfo;
    void Start()
    {
        upgradeInfo = originalData.upgrades;

        for(int i = 0; i < upgradeInfo.Length; i++)
        {
            upgradeInfo[i].upgradeLevel = 0;
            upgradeInfo[i].unlockCost = upgradeInfo[i].upgradeTier[upgradeInfo[i].upgradeLevel].upgradeCost;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
