using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPlatform : MonoBehaviour
{
    //Measures how far the platform has travelled
    float distanceTravelled;
    //The speed and distance limit for the platform
    public float elevatorSpeed, distanceLimit;
    //gets the start position of the platform (For instances where the platform moves when the player is stood on it and the player leaves, resetting it)
    public Vector2 startPosition;
    //Boolean that determines whether the platform is obstructed by an object, preventing it from moving
    public bool isObstructedByObj;
    //stores a reference to the obstructing object, and the player (target)
    public GameObject obstruction, target;
    //Whether it is activated by the player, by a switch
    public bool ActivateByPlayer, ActivatedBySwitch;
    //whether the platform moves vertically
    public bool moveVert;
    //Whether the player is on the platform, whether the platform moves in the opposite direction
    public bool playerOnPlat, moveAway;

    //The offset for the player's movement while on the platform
    Vector3 offset;

    void Start()
    {
        //sets the start position
        startPosition = transform.position;


        //If this is set to true, inverts the speed of the platform
        if (moveAway)
        {
            elevatorSpeed = -(elevatorSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if the game is paused, the platform won't move
        if (GamePause.gamePaused)
        {
            return;
        }

        //If it isn't activated by the player,
        if (!ActivateByPlayer)
        {
            if (!isObstructedByObj)
            {
                movePlatform();
            }
            else
            {
                if(obstruction == null)
                {
                    movePlatform();
                }
                else
                {
                    return;
                }
            }
        }
        else if (ActivateByPlayer)
        {
            if (playerOnPlat)
            {
                movePlatform();
            }
            else
            {
                Vector2.MoveTowards(transform.position, startPosition, elevatorSpeed * Time.deltaTime);
            }
        }
        else if (ActivatedBySwitch)
        {

        }
        checkDistance();

        
    }

    void checkDistance()
    {
        if(distanceTravelled >= distanceLimit)
        {
            startPosition = transform.position;
            elevatorSpeed = -(elevatorSpeed);
            distanceTravelled = 0;
        }
    }

    void movePlatform()
    {
        if (moveVert)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + elevatorSpeed * Time.deltaTime, transform.position.z);

        }
        else if (!moveVert)
        {
            transform.position = new Vector3(transform.position.x + elevatorSpeed * Time.deltaTime, transform.position.y, transform.position.z);
        }
        distanceTravelled += Mathf.Abs(elevatorSpeed) * Time.deltaTime;
    }

    

    private void OnCollisionStay2D(Collision2D other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            playerOnPlat = true;
            target = other.gameObject;
            
            offset = target.transform.position - transform.position;
            
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        target = null;
    }

    private void LateUpdate()
    {
        if(target != null)
        {
            target.transform.position = transform.position + offset;
        }
    }
}
