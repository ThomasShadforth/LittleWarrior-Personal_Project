using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPlatform : MonoBehaviour
{
    float distanceTravelled;
    public float elevatorSpeed, distanceLimit;
    public Vector2 startPosition;
    public bool isObstructedByObj;
    public GameObject obstruction, target;
    public bool ActivateByPlayer, ActivatedBySwitch, moveVert, playerOnPlat, moveAway;

    Vector3 offset;

    void Start()
    {
        startPosition = transform.position;

        if (moveAway)
        {
            elevatorSpeed = -(elevatorSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GamePause.gamePaused)
        {
            return;
        }

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
