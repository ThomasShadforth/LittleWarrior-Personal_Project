using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBlocker : MonoBehaviour
{
    public Collider2D characterCollider;
    public Collider2D colliderBlocker;
    void Start()
    {
        Physics2D.IgnoreCollision(characterCollider, colliderBlocker, true); 
    }

    
}
