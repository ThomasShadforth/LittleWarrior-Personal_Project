using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.SceneManagement;

public class EnemyAi : MonoBehaviour
{
    [Header("General AI Properties")]
    public Transform target;
    public float activateDistance = 50f;
    public float speed = 5f;
    public float nextWaypoint = 3f;
    public float jumpNodeHeightReq = .5f;
    public float jumpMod = .3f;
    public float jumpCheckOffset = .1f;
    public float attackDistance = .2f;


    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;
    Animator animator;

    [Header("Custom Behaviour")]
    public bool jumpEnabled;
    public bool FollowEnabled;
    public bool directionLook;
    public bool isAttacking;

    [Header("Enemy Stats")]
    public float MaxHealth;
    public int awardedUpgradePoints;
    public float health;

    public bool isGrounded;
    public bool isKnocked;
    public float knockTime;
    [SerializeField]
    Transform enemyFeet;

    Vector2 knockbackForce;

    public float overlapCheck = .4f;
    public LayerMask whatIsGround;

    [SerializeField]
    EnemyUI attachedUI;

    bool targetAndUISet;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        InvokeRepeating("UpdatePath", 0f, .25f);

        health = MaxHealth;

        
    }

    

    void UpdatePath()
    {
        if (seeker.IsDone() && TargetInDistance() && FollowEnabled)
        {
            seeker.StartPath(rb.position, target.position, onPathComplete);
        }
    }

    void onPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GamePause.gamePaused)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            return;
        }

        if(target.GetComponent<BasePlayer>().health <= 0)
        {
            return;
        }

        else
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        

        if (isAttacking && !isKnocked)
        {
            return;  
        }

        

        if (FollowEnabled && TargetInDistance())
        {
            if (!isKnocked)
            {
                pathFollow();
            }
            else
            {

                
            }
            
        }

        

        if (TargetInAttackRange())
        {
            isAttacking = true;
            animator.SetBool("isAttacking", true);
        }
    }

    private void Update()
    {
        if (!targetAndUISet)
        {
            target = FindObjectOfType<BasePlayer>().transform;
            attachedUI = GetComponentInChildren<EnemyUI>();
            targetAndUISet = true;
        }
    }
    void LateUpdate()
    {
        if (isKnocked)
        {
            if (knockTime > 0)
            {

                animator.SetBool("isHurt", true);
                rb.velocity = new Vector2(knockbackForce.x, knockbackForce.y);
                knockTime -= Time.deltaTime;
                return;
            }
            else
            {
                animator.SetBool("isHurt", false);
                isKnocked = false;
                rb.velocity = Vector3.zero;
            }
        }
    }

    void pathFollow()
    {
        if (path == null)
        {
            return;
        }

        

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        isGrounded = Physics2D.OverlapCircle(enemyFeet.position, overlapCheck, whatIsGround);

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        if (jumpEnabled && isGrounded)
        {
            if(direction.y > jumpNodeHeightReq)
            {
                rb.AddForce(Vector2.up * speed * jumpMod);
            }
        }

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypoint)
        {
            currentWaypoint++;
        }

        if (directionLook)
        {
            if(rb.velocity.x > 0.05f)
            {
                Vector3 scalar = transform.localScale;
                if(scalar.x < 0)
                {
                    scalar.x = -scalar.x;
                }
                transform.localScale = scalar;
            } else if(rb.velocity.x < -0.05f)
            {
                Vector3 scalar = transform.localScale;
                if(scalar.x > 0)
                {
                    scalar.x = -scalar.x;
                }
                transform.localScale = scalar;
            }
        }
        if(rb.velocity.x != 0)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    bool TargetInDistance()
    {
        if(Vector2.Distance(rb.position, target.position) <= activateDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TargetInAttackRange()
    {
        if(Vector2.Distance(rb.position, target.position) <= attackDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void enemyAngleAdjust(float zRotation)
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, zRotation);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, activateDistance);
    }


    public void setKnock(Vector2 knockForce, float knockDur)
    {
        
        knockbackForce = knockForce;
        knockTime = knockDur;
        
    }

    public void dealDamage(float damage)
    {
        health -= damage;
        attachedUI.updateHealth();
        if(health <= 0)
        {
            //Play Death Anim, then destroy
            Invoke("DestroyEnemy", .7f);
        }
    }

    private void DestroyEnemy()
    {
        GameManager.instance.updateUpgradePoints(awardedUpgradePoints);
        Destroy(this.gameObject);
    }
}
