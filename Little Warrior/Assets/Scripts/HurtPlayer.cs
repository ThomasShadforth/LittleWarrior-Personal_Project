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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            BasePlayer player = other.GetComponent<BasePlayer>();
            if(rb.position.x < other.transform.position.x)
            {
                //ON LEFT
                isOnRight = false; 
            } else if(rb.position.x > other.transform.position.x)
            {
                //ON RIGHT
                isOnRight = true;
            }

            player.applyKnockback(isOnRight, 2f, .2f);
        }
    }
}
