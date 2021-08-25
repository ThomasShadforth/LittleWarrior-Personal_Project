using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointSystem : MonoBehaviour
{
    //Used to determine if the checkpoint has been set
    public bool checkpointSet;

    public bool endOfLevel;

    public GameObject playTestMenu;

    Animator animator;

    bool loadStart;
    // Start is called before the first frame update
    void Start()
    {
        //Get animator component
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if the checkPoint is set, set the animator to play the active anim
        if (checkpointSet)
        {
            animator.SetBool("isSet", true);
        }
        else
        {
            animator.SetBool("isSet", false);
        }
    }

    //If the player enters the radius, then set the gameManager's checkpointPos (For respawns) to the position of this object
    //Set checkpointSet to true
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!endOfLevel)
            {
                GameManager.instance.GetComponent<CheckpointHandler>().checkpointPos = transform.position;
                checkpointSet = true;
            }
            else
            {
                if (!loadStart)
                {
                    loadStart = true;
                    GameManager.instance.loadLevel();
                }
                
            }
        }
    }

    
}
