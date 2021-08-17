using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleCheckScript : MonoBehaviour
{
    //Stores references to the two points between which the angle is measured
    [SerializeField]
    Transform angleCheck1, angleCheck2;
    //A measurement of the ground's current angle
    [SerializeField]
    float groundAngle;
    //Reference to the player character
    BasePlayer player;

    float check1X, check2X;
    void Start()
    {
        player = GetComponent<BasePlayer>();
        check1X = angleCheck1.localPosition.x;
        check2X = angleCheck2.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FixedUpdate()
    {
        //Checks are performed while the player is on the ground
        if (player.isGrounded)
        {
            //Creates two raycast hit results, angle1 and angle2
            RaycastHit2D angle1, angle2;
            //If the first anglecheck hits the ground, stores the result
            if(Physics2D.Raycast(angleCheck1.position, Vector2.down, 7.0f, player.whatIsGround))
            {
                angle1 = Physics2D.Raycast(angleCheck1.position, Vector2.down, 7.0f, player.whatIsGround);

                //if the second achieves this, store the result
                if(Physics2D.Raycast(angleCheck2.position, Vector2.down, 7.0f, player.whatIsGround))
                {
                    angle2 = Physics2D.Raycast(angleCheck2.position, Vector2.down, 7.0f, player.whatIsGround);

                    //Calculates the initial ground angle based on where the player is facing (Subtracts angle 2 from 1 if facing right, and angle 1 from 2 if facing left)
                    if (player.facingRight)
                    {
                        groundAngle = Mathf.Atan2((angle1.point.y - angle2.point.y), (angle1.point.x - angle2.point.x)) * Mathf.Rad2Deg;
                    }
                    else
                    {
                        groundAngle = Mathf.Atan2((angle2.point.y - angle1.point.y), (angle2.point.x - angle1.point.x)) * Mathf.Rad2Deg;
                    }

                    
                    //If the ground angle is exactly 180 degrees, set the angle to 0, otherwise add 180 degrees to be accurate
                    if(groundAngle == 180f)
                    {
                        groundAngle = 0f;
                    }
                    else
                    {
                        groundAngle += 180;
                    }

                }
            }


        }
        else
        {
            groundAngle = 0f;
        }

        //The player objects angle is adjusted based on the calculated angle
        player.adjustPlayerAngle(groundAngle);
    }
}
