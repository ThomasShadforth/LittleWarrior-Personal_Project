using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAngleCheck : MonoBehaviour
{
    [SerializeField]
    Transform angleCheck1, angleCheck2;
    [SerializeField]
    float groundAngle;

    EnemyAi enemy;
    void Start()
    {
        enemy = GetComponent<EnemyAi>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemy.isGrounded)
        {
            RaycastHit2D angle1, angle2;
            if (Physics2D.Raycast(angleCheck1.position, Vector2.down, 7.0f, enemy.whatIsGround))
            {
                angle1 = Physics2D.Raycast(angleCheck1.position, Vector2.down, 7.0f, enemy.whatIsGround);

                if (Physics2D.Raycast(angleCheck2.position, Vector2.down, 7.0f, enemy.whatIsGround))
                {
                    angle2 = Physics2D.Raycast(angleCheck2.position, Vector2.down, 7.0f, enemy.whatIsGround);

                    if (enemy.transform.localScale.x == 1)
                    {
                        groundAngle = Mathf.Atan2((angle1.point.y - angle2.point.y), (angle1.point.x - angle2.point.x)) * Mathf.Rad2Deg;
                    }
                    else
                    {
                        groundAngle = Mathf.Atan2((angle2.point.y - angle1.point.y), (angle2.point.x - angle1.point.x)) * Mathf.Rad2Deg;
                    }



                    if (groundAngle == 180f)
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
            groundAngle = 0;
        }

        enemy.enemyAngleAdjust(groundAngle);
    }
}
