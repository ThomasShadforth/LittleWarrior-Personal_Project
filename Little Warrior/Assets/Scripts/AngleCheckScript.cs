using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleCheckScript : MonoBehaviour
{
    [SerializeField]
    Transform angleCheck1, angleCheck2;
    [SerializeField]
    float groundAngle;
    BasePlayer player;
    void Start()
    {
        player = GetComponent<BasePlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FixedUpdate()
    {
        if (player.isGrounded)
        {
            RaycastHit2D angle1, angle2;
            if(Physics2D.Raycast(angleCheck1.position, Vector2.down, 7.0f, player.whatIsGround))
            {
                angle1 = Physics2D.Raycast(angleCheck1.position, Vector2.down, 7.0f, player.whatIsGround);

                if(Physics2D.Raycast(angleCheck2.position, Vector2.down, 7.0f, player.whatIsGround))
                {
                    angle2 = Physics2D.Raycast(angleCheck2.position, Vector2.down, 7.0f, player.whatIsGround);

                    groundAngle = Mathf.Atan2((angle1.point.y - angle2.point.y), (angle1.point.x - angle2.point.x)) * Mathf.Rad2Deg;

                    

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

        player.adjustPlayerAngle(groundAngle);
    }
}
