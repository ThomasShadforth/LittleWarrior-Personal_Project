using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    //Used to determine if the checkpoint has been set
    bool checkpointSet;
    Animator animator;
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.GetComponent<CheckpointHandler>().checkpointPos = transform.position;
            checkpointSet = true;
        }
    }
}
