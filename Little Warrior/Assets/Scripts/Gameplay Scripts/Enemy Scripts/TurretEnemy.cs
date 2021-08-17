using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : MonoBehaviour
{
    public Transform target;
    public float detectRadius;
    public GameObject turretProj;
    public float projectileSpeed;

    [SerializeField]
    Transform turretFirePoint;

    bool playerInRange;
    public float rateOfFire;
    [SerializeField]
    float currentTimer;
    float delayTime;

    Animator animator;

    Vector2 fireDirection;
    
    [SerializeField]
    float fireAngle;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentTimer = rateOfFire;
    }

    // Update is called once per frame
    void Update()
    {
        if (GamePause.gamePaused)
        {
            return;
        }

        if (playerInRange)
        {
            if (delayTime <= 0)
            {
                animator.SetBool("TurretCharging", true);
                if (currentTimer > 0)
                {
                    currentTimer -= Time.deltaTime;
                }
            }
            else
            {
                animator.SetBool("TurretCharging", false);
                delayTime -= Time.deltaTime;
            }
        }

        if (Vector2.Distance(target.transform.position, transform.position) <= detectRadius)
        {
            if (!playerInRange)
            {
                animator.SetBool("PlayerInRange", true);
            }
            playerInRange = true;
            
            //Do stuff! (Change angle based on player position (Need to look into this))

            fireDirection = target.transform.position - transform.position;
            fireDirection.y = fireDirection.y - .1f;
            fireAngle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
            turretFirePoint.transform.rotation = Quaternion.Euler(turretFirePoint.rotation.x, turretFirePoint.rotation.y, fireAngle - 90);

            if (currentTimer <= 0)
            {
                StartCoroutine(fireProjectile());
                currentTimer = rateOfFire;
                delayTime = 2.0f;
            }
        }
        else
        {
            playerInRange = false;
            animator.SetBool("PlayerInRange", false);
        }

        setXScale();
    }

    void setXScale()
    {
        if(target.transform.position.x < transform.position.x)
        {
            Vector3 scalar = transform.localScale;
            scalar.x = -2;
            transform.localScale = scalar;
        } else if (target.transform.position.x > transform.position.x)
        {
            Vector3 scalar = transform.localScale;
            scalar.x = 2;
            transform.localScale = scalar;
        }
    }


    public IEnumerator fireProjectile()
    {
        yield return new WaitForSeconds(.32f);
        GameObject firedObject = Instantiate(turretProj, turretFirePoint.position, turretFirePoint.rotation);
        firedObject.GetComponent<Rigidbody2D>().velocity = turretFirePoint.up * projectileSpeed;
        
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, detectRadius);
        Gizmos.DrawLine(turretFirePoint.transform.position, target.position);
    }
}
