using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class HurtEnemy : MonoBehaviour
{
    public bool beingKnocked;
    float knockDur;
    Rigidbody2D rb;

    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hurtEnemyFunc(bool willDealKnock, float damage, float screenShakeIntensity, Vector2 knockback, Transform playerPos)
    {
        if (willDealKnock)
        {
            if(playerPos.position.x < rb.position.x)
            {
                
            }
            else
            {

            }

            if(playerPos.position.y > rb.position.y)
            {

            }
            else
            {

            }
        }

        //Add damage to enemy here

        
        
        Invoke("DestroyEnemy", .7f);
        
    }

    

    void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }
}
