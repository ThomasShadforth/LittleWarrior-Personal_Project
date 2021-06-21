using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace playerUpgradeSystem
{
    [CreateAssetMenu(fileName = "Player Upgrade Data", menuName = "ScriptableObject/PlayerUpgradeData")]

    public class PlayerUpgradeData : ScriptableObject
    {
        public static PlayerUpgradeData instance;

        public playerUpgrade[] upgrades;


    }

    [System.Serializable]
    public class playerUpgrade
    {
        public string upgradeName;
        public string description;
        public bool isUnlocked;
        public int unlockCost;
        public int upgradeLevel;

        public upgradeLevels[] upgradeTier;
    }

    [System.Serializable]
    public class upgradeLevels
    {
        public int upgradeCost;
    }
}
