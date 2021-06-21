using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour
{
    public bool facingRight;

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

    [Header("Movement")]
    public float speed;
    const float maxSpeed = 6.0f;
    const float timeToReachMaxSpeed = .3f;
    const float timeToDecel = .3f;
    const float accelRate = (maxSpeed) / timeToReachMaxSpeed;
    const float decelRate = -(maxSpeed - 3) / timeToDecel;
    const float friction = 3f;

    CombatSystem playerCombat;
    float extraSpeed;

    public static BasePlayer instance;

    void Start()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        rb = GetComponent<Rigidbody2D>();
        playerCombat = GetComponent<CombatSystem>();
        moveInput.x = 1;
        facingRight = true;
    }

    void FixedUpdate()
    {
        if (GamePause.gamePaused)
        {
            return;
        }
        Vector3 feet = new Vector3(playerFeet.position.x, playerFeet.position.y - .5f, playerFeet.position.z);
        isGrounded = Physics2D.OverlapCircle(feet, detectRadius, whatIsGround);
        
    }
    void Update()
    {
        if (GamePause.gamePaused)
        {
            return;
        }

        //Start of Horizontal movement
        

        if (Input.GetButton("Left"))
        {
            if (facingRight)
            {
                facingRight = false;
                changeLocalScale();
            }
            moveInput.x = -1;
            if (speed > 0)
            {
                speed += decelRate * Time.deltaTime;
                if (speed < 0)
                {
                    speed = -.5f;
                }
            }
            else if (speed > -maxSpeed)
            {
                speed -= accelRate * Time.deltaTime;
                if (speed <= -maxSpeed)
                {
                    speed = -maxSpeed;
                }
            }
        }
        else if (Input.GetButton("Right"))
        {
            if (!facingRight)
            {
                facingRight = true;
                changeLocalScale();
            }
            moveInput.x = 1;
            if (speed < 0)
            {
                speed -= decelRate * Time.deltaTime;
                if (speed > 0)
                {
                    speed = .5f;
                }
            }
            else if (speed < maxSpeed)
            {
                speed += accelRate * Time.deltaTime;

                if (speed >= maxSpeed)
                {
                    speed = maxSpeed;
                }
            }
            //check for left movement
        }

        //deceleration
        if (!Input.GetButton("Left") || !Input.GetButton("Right"))
        {
            speed -= (Mathf.Min(Mathf.Abs(speed), friction) * Mathf.Sign(speed) * maxSpeed * Time.deltaTime);
        }


        //Start of Vertical Movement (Jumps)
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded && jumpNum == 0)
        {
            rb.velocity = Vector2.up * jumpHeight;
            JumpTime = JumpTimeCounter;
            isJumping = true;
        }

        if(Input.GetKeyDown(KeyCode.Space) && jumpNum > 0)
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

    private void LateUpdate()
    {
        if (GamePause.gamePaused)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            return;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        
        if (knockbackTime <= 0)
        {
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
}
