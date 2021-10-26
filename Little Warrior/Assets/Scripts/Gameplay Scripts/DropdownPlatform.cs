using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownPlatform : MonoBehaviour
{
    //Boolean that determines whether or not the player is currently on the platform
    bool isOnPlatform;
    //The amount of time it takes for the platform to reset when the player jumps through it/drops down
    [SerializeField]
    float resetTime;

    //the collider for the platform. Reference to it is stored in order to enable/disable it on the fly
    Collider2D platCollider;
    void Start()
    {
        platCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //When the player presses these inputs on the platform, lets them drop down
        if(Input.GetButton("Down") && Input.GetKeyDown(KeyCode.Space) && isOnPlatform)
        {
            //Disables the collider, then invokes a method to reactivate the collider
            platCollider.enabled = false;
            if (!isOnPlatform)
            {
                Invoke("ResetCollider", resetTime);
            }
        }
    }

    //While the player remains on the platform, isOnPlatform is true
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            isOnPlatform = true;
        }
    }


    //When the player jumps off the platform, set to false
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isOnPlatform = false;
        }
    }

    //When the player jumps and it detects the trigger zone attached to the player, disables the platform collider to allow the player to jump through,
    //before invoking the method to re-enable it
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            BasePlayer player = other.GetComponent<BasePlayer>();
            if(!player.isGrounded && player.rb.velocity.y > 0)
            {
                platCollider.enabled = false;
                Invoke("ResetCollider", resetTime);
            }
        }
    }

    public void ResetCollider()
    {
        platCollider.enabled = true;
    }
}
