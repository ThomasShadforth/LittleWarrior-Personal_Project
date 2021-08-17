using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCheckpoints : CheckpointSystem
{
    public int checkpointIndex;
    public bool hasConditions;
    public bool isFinalCheckpoint;

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isFinalCheckpoint)
            {
                Debug.Log("DETECTED");
                checkpointSet = true;
                TutorialLevel.instance.openTutorialPopUp(checkpointIndex, transform.position, hasConditions);
            }
            else
            {
                if (!checkpointSet)
                {
                    checkpointSet = true;
                    StartCoroutine(TutorialLevel.instance.finalPopUp());
                }
            }
        }
    }
}
