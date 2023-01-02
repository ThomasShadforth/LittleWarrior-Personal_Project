using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace playerUpgradeSystem
{
    [CreateAssetMenu(fileName = "Player Upgrade Data", menuName = "ScriptableObject/PlayerUpgradeData")]

    public class PlayerUpgradeData : ScriptableObject
    {
        //Gets an instance of this SO
        public static PlayerUpgradeData instance;

        //Array of all available upgrades
        public playerUpgrade[] upgrades;


    }

    //Stores the name, description, the type of upgrade, cost, etc.
    [System.Serializable]
    public class playerUpgrade
    {
        public string upgradeName;
        public string description;
        public bool isUnlocked;
        public bool isAttack;
        public bool isStarterAttack;
        public int unlockCost;
        public int upgradeLevel;

        //Stores a list of all the upgrade tiers available
        public upgradeLevels[] upgradeTier;
    }

    //Stores the cost of each tier, and what increases as a result of the upgrade (Health, etc.)
    [System.Serializable]
    public class upgradeLevels
    {
        public int upgradeCost;
        public int upgradeIncrease;
    }
}
