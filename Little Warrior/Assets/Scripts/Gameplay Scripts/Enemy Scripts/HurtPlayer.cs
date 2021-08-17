using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    Rigidbody2D rb;
    bool isOnRight;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void hurtThePlayer(BasePlayer player, float damage)
    {
        
        if (rb.position.x < player.transform.position.x)
        {
            //ON LEFT
            isOnRight = false;
        }
        else if (rb.position.x > player.transform.position.x)
        {
            //ON RIGHT
            isOnRight = true;
        }
        player.dealDamage(damage);
        player.applyKnockback(isOnRight, 3f, .2f);
    }
}
