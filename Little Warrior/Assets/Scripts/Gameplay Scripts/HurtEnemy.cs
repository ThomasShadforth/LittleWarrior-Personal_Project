using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class HurtEnemy : MonoBehaviour
{
    public bool beingKnocked;
    EnemyAi enemy;
    float knockDur;
    Rigidbody2D rb;

    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<EnemyAi>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hurtEnemyFunc(bool willDealKnock, float damage, Vector2 knockback, Transform playerPos)
    {
        if (willDealKnock)
        {
            enemy.isKnocked = true;
            if(playerPos.position.x < rb.position.x)
            {
                rb.velocity = new Vector2(knockback.x, rb.velocity.y);  
            }
            else
            {
                rb.velocity = new Vector2(-knockback.x, rb.velocity.y);
            }

            if(playerPos.position.y > rb.position.y)
            {
                rb.velocity = new Vector2(rb.velocity.x, -knockback.y);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, knockback.y);
            }
        }

        //Add damage to enemy here
        enemy.dealDamage(damage);
        
    }

    

    void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }
}
