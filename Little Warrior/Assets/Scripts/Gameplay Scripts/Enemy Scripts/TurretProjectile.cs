using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{

    public float destroyOverTime;
    public int damageToDeal;

    public LayerMask groundCollision;
    [SerializeField]
    float lifeTime;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime += 1 * Time.deltaTime;
        if (lifeTime >= destroyOverTime)
        {
            Destroy(this.gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<HurtPlayer>().hurtThePlayer(BasePlayer.instance, damageToDeal);
            Destroy(this.gameObject);
        } if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("THE GROUND");
            Destroy(this.gameObject);
        }
    }

    
}
