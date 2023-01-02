using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour
{
    public bool facingRight;
    bool isCrouched;

    [Header("JumpValues")]
    public LayerMask whatIsGround;
    public float jumpHeight;
    public float detectRadius;
    public bool isGrounded;
    public bool isJumping;
    public float JumpTimeCounter;
    public float ExtraJumps;
    float jumpNum;

    public float knockbackTime;
    public Vector2 knockbackForce;
    public bool isAttacking;

    [SerializeField]
    Transform playerFeet;

    Vector2 moveInput;

    float JumpTime;

    public Rigidbody2D rb;
    Animator animator;

    [Header("Movement")]
    public float speed;
    const float maxSpeed = 3.0f;
    const float timeToReachMaxSpeed = .3f;
    const float timeToDecel = .3f;
    const float accelRate = (maxSpeed) / timeToReachMaxSpeed;
    const float decelRate = -(maxSpeed - (maxSpeed / 2)) / timeToDecel;
    const float friction = 2.2f;
    public bool isOnPlatform;

    [Header("Health Values")]
    public float MaxHealth;
    int healthUpgradeLevel = 0;
    public float health;

    [Header("Base Stats")]
    [SerializeField]
    public int baseAtk;
    [SerializeField]
    public int playerDef;

    [Header("Player Audio")]
    public AudioClip[] playerSFX;
    AudioSource _as;

    CombatSystem playerCombat;
    float extraSpeed;
    public int upgradePoints;
    public static BasePlayer instance;

    void Start()
    {
        //Uses singleton structure, only one instance of the player can exist at a time
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        //Get the rigidbody, animator and combat components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _as = GetComponent<AudioSource>();
        playerCombat = GetComponent<CombatSystem>();
        //Used to determine the x scale of the player object
        moveInput.x = 1;
        facingRight = true;

        health = MaxHealth;
    }

    void FixedUpdate()
    {
        
        if (GamePause.gamePaused)
        {
            return;
        }
        //Determines whether or not the player is grounded at the moment (Works in conjunction with jumping and angle checks)
        Vector3 feet = new Vector3(playerFeet.position.x, playerFeet.position.y, playerFeet.position.z);
        isGrounded = Physics2D.OverlapCircle(feet, detectRadius, whatIsGround);
        
    }
    void Update()
    {
        //Can only move when the game is not paused
        if (GamePause.gamePaused)
        {
            return;
        }

        //Start of Horizontal movement
        checkAnimState();

        //Checks for input (Same applies to the right, but in the opposite direction)
        if (Input.GetButton("Left"))
        {
            //Starts run animation
            animator.SetBool("isRunning", true);
            //Flips the x scale to face left if the player is currently facing right
            if (facingRight)
            {
                facingRight = false;
                changeLocalScale();
                
            }
            //Set move input
            moveInput.x = -1;
            //If the player's speed was greater than 0, begin deceleration
            if (speed > 0)
            {
                //Decelerate towards the left
                speed += decelRate * 1.9f * Time.deltaTime;
                //Set speed to minimum if less than 0
                if (speed < 0)
                {
                    speed = -.5f;
                }
            }
            //While the speed has not reached max speed for movement to the left
            else if (speed > -maxSpeed)
            {
                //Accelerate
                speed -= accelRate * Time.deltaTime;
                //Set the animation speed based on how fast the player is in relation to the max speed
                animator.SetFloat("AnimSpeed", Mathf.Abs(speed) / maxSpeed);
                //Set the speed to it's maximum if it exceeds it
                if (speed <= -maxSpeed)
                {
                    speed = -maxSpeed;
                }
            }
        }
        else if (Input.GetButton("Right"))
        {
            animator.SetBool("isRunning", true);
            if (!facingRight)
            {
                facingRight = true;
                changeLocalScale();
                
            }
            moveInput.x = 1;
            if (speed < 0)
            {
                speed -= decelRate * 1.9f * Time.deltaTime;
                if (speed > 0)
                {
                    speed = .5f;
                }
            }
            else if (speed < maxSpeed)
            {
                speed += accelRate * Time.deltaTime;
                animator.SetFloat("AnimSpeed", speed / maxSpeed);
                if (speed >= maxSpeed)
                {
                    speed = maxSpeed;
                }
            }
            //check for left movement
        }

        if (Input.GetButton("Down"))
        {
            isCrouched = true;
        }
        else
        {
            isCrouched = false;
        }

        //deceleration
        if (!Input.GetButton("Left") && !Input.GetButton("Right"))
        {
            animator.SetBool("isRunning", false);
            speed -= (Mathf.Min(Mathf.Abs(speed * 2.2f), friction * 2.2f) * Mathf.Sign(speed) * maxSpeed * Time.deltaTime);
            
        }


        //Start of Vertical Movement (Jumps)
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded && !isCrouched && jumpNum == 0)
        {
            rb.velocity = Vector2.up * jumpHeight;
            JumpTime = JumpTimeCounter;
            isJumping = true;
        }

        if(Input.GetKeyDown(KeyCode.Space) && !isCrouched && jumpNum > 0)
        {
            jumpNum--;
            rb.velocity = Vector2.up * jumpHeight;
            JumpTime = JumpTimeCounter;
            isJumping = true;
        }

        //Write higher jump later
        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (JumpTime > 0)
            {
                rb.velocity = Vector2.up * jumpHeight;
                JumpTime -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }

        if (isGrounded)
        {
            jumpNum = ExtraJumps;
        }
        //End of Vertical Movement (Jumps)

        //Insert method that checks for animation state changes
    }

    public void setPlayerExtraSpeed(float speedChange)
    {
        if (moveInput.x == 1)
        {
            extraSpeed = speedChange;
        }
        else if (moveInput.x == -1)
        {
            extraSpeed = -speedChange;
        }
    }

    public void applyKnockback(bool isRight, float knockback, float knockbackTimer)
    {
        knockbackTime = knockbackTimer;
        knockbackForce.y = knockback / 2;
        if (isRight)
        {
            knockbackForce.x = -knockback;
        }
        else
        {
            knockbackForce.x = knockback;
        }
    }

    public void adjustPlayerAngle(float newZAngle)
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, newZAngle);
    }

    //Applies velocity to player in late update (if not being knocked back or pausing the game)
    private void LateUpdate()
    {
        if (GamePause.gamePaused)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            return;
        }
        else
        {
            if (!isAttacking)
            {
                rb.constraints = RigidbodyConstraints2D.None;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
        
        if (knockbackTime <= 0)
        {
            animator.SetBool("isBeingHurt", false);
            if (!isAttacking || playerCombat.currentAttack.canMoveWhileAttack)
            {
                rb.velocity = new Vector2(speed + extraSpeed, rb.velocity.y);
            }
            else if (isAttacking && !playerCombat.currentAttack.canMoveWhileAttack)
            {
                rb.velocity = new Vector2(extraSpeed, rb.velocity.y);
            }
        }
        else
        {
            animator.SetBool("isBeingHurt", true);
            rb.velocity = new Vector2(knockbackForce.x, knockbackForce.y);
            knockbackTime -= Time.deltaTime;
        }
    }

    void changeLocalScale()
    {
        Vector3 scalar = transform.localScale;
        scalar.x = -scalar.x;
        transform.localScale = scalar;
    }

    //Take damage from the enemy (If hit)
    public void dealDamage(float damageVal)
    {
        //Update health value and health bar
        health -= damageVal;
        GameplayUI.instance.updatePlayerHealthBar();

        //Decrease lives if health is less or equal to 0
        if(health <= 0)
        {
            GameManager.instance.remainingLives--;

            //Insert Death Anim Here
            //Game over or reload based on remaining number of lives
            if(GameManager.instance.remainingLives <= 0)
            {
                GameManager.instance.loadGameOver();
            }
            else
            {
                GameManager.instance.reloadAfterDeath();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(playerFeet != null)
        {
            Gizmos.DrawWireSphere(playerFeet.position, detectRadius);
        }
    }

    void checkAnimState()
    {
        if(!isGrounded && rb.velocity.y > 0)
        {
            animator.SetBool("isJumping", true);
        }

        if(!isGrounded && rb.velocity.y < 0)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }

        if (isGrounded)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }
    }

    //Used to upgrade a player's stats (If it was upgraded from the menu)
    public void upgradeStat(string statName, int statIncrease)
    {
        switch (statName)
        {
            case "Health Upgrade":
                MaxHealth += statIncrease;
                healthUpgradeLevel++;
                break;
            case "Attack Upgrade":
                baseAtk += statIncrease;
                break;
            case "Defense Upgrade":
                playerDef += statIncrease;
                break;
            default:
                
                break;
        }
    }

    public void playSFX(int soundID)
    {
        _as.clip = playerSFX[soundID];
        _as.Play();
    }
}
