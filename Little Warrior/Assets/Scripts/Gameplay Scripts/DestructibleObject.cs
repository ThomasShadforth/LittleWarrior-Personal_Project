using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public float objectDurability;
    bool willExplode;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageObject(float damage)
    {
        objectDurability -= damage;
        checkForDestroy();
    }

    public void checkForDestroy()
    {
        if(objectDurability <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
