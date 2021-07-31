using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(GameManager.instance.remainingLives <= 0)
            {
                //Insert game over screen transition here
                //Destroy player object and other related ddol (Don't destroy on Load) objects
                //Load the scene
            }
            else
            {
                //Move the player to the previous checkpoint (There's no need to reload the scene? (Depends on what workarounds can be made for the enemies))
                BasePlayer.instance.transform.position = GameManager.instance.GetComponent<CheckpointHandler>().checkpointPos;
                GameManager.instance.remainingLives -= 1;
            }
        }
    }
}
