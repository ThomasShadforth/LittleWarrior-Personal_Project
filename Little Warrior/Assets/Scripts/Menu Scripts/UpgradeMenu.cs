using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    public Text upgradeName, upgradeDesc, upgradeCostText, upgradeButtonText;
    public Button upgradeButton;
    public RuntimeUpgradeData upgradeData;
    // Start is called before the first frame update
    void Start()
    {
        upgradeData = GetComponent<RuntimeUpgradeData>(); 
    }

    // Update is called once per frame
    void Update()
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
        
        //if the player has enough points for an upgrade, the text will be displayed normally
        //If not, then the button will be greyed out

    }
}
