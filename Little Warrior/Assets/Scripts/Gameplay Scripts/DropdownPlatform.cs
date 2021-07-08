using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownPlatform : MonoBehaviour
{
    bool isOnPlatform;
    [SerializeField]
    float resetTime;

    Collider2D platCollider;
    void Start()
    {
        platCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Down") && Input.GetKeyDown(KeyCode.Space) && isOnPlatform)
        {
            platCollider.enabled = false;
            if (!isOnPlatform)
            {
                Invoke("ResetCollider", resetTime);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("ON PLAT");
            isOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isOnPlatform = false;
        }
    }

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
