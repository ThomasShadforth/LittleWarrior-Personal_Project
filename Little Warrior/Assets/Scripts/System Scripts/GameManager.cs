using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Game Manager is responsible for handling several key processes, such as keeping a record of the current upgrade points, current level, etc.
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    int upgradePoints;
    public int remainingLives;


    // Start is called before the first frame update
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //[Dealing with Upgrade Points]
    public void updateUpgradePoints(int pointChange)
    {
        upgradePoints += pointChange;
        Debug.Log(upgradePoints);
    }

    public int getUpgradePoints()
    {
        return upgradePoints;
    }

    //
}
