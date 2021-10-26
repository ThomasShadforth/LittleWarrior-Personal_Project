using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    //How much durability/health the object has
    public float objectDurability;
    //If the object explodes after being destroyed
    bool willExplode;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //Method that is called when the player hits the object with an attack (If it's detected during the active frames)
    public void DamageObject(float damage)
    {
        objectDurability -= damage;
        checkForDestroy();
    }


    //Run every time the object takes damage, determines whether or not it needs to be destroyed
    public void checkForDestroy()
    {
        if(objectDurability <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
