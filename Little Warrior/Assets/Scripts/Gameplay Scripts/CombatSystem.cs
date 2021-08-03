using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CombatSystem : MonoBehaviour
{
    Animator animator;

    public CombatAttacks[] Attacks;
    public CombatAttacks currentAttack;

    float attackTime;
    float bonusSpeed;
    Rigidbody2D rb;
    public float attackRadius;
    BasePlayer playerChar;
    public Transform attackPoint;
    bool isAttacking;
    public LayerMask enemyLayer, objectsLayer;
    [SerializeField]
    AttackRenderer attRender;


    CinemachineVirtualCamera cineCam;


    void Start()
    {
        currentAttack = Attacks[0];
        rb = GetComponent<Rigidbody2D>();
        playerChar = GetComponent<BasePlayer>();
        animator = GetComponent<Animator>();
        cineCam = FindObjectOfType<CinemachineVirtualCamera>();
        
        setUpAttackLinks();
    }

    // Update is called once per frame
    void Update()
    {
        if (GamePause.gamePaused)
        {
            return;
        }
        checkForAttackInput();

        if(attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
        
        if(attackTime <= 0)
        {
            currentAttack = Attacks[0];
            
            playerChar.setPlayerExtraSpeed(0f);
        }

        
    }

    public void removeEndOfString(string attackName)
    {
        for(int i = 0; i < Attacks.Length; i++)
        {
            if(Attacks[i].AttackName == attackName)
            {
                Attacks[i].endOfAttackString = false;
                break;
            }
        }
    }

    public void setUpAttackLinks()
    {
        for(int i = Attacks.Length - 1; i >= 0; i--)
        {
            
            string nameOfAttack = Attacks[i].AttackName;

            for(int j = Attacks.Length - 1; j >= 0; j--)
            {
                if(nameOfAttack == Attacks[j].nextLightAttack || nameOfAttack == Attacks[j].nextHeavyAttack || nameOfAttack == Attacks[j].nextDownAttack)
                {
                    Attacks[i].previousAttackName = Attacks[j].AttackName;
                }
            }
        }
    }

    //checked throughout the update method, detects when the player uses any of the attack buttons
    //Each branch will execute the corresponding attack check for the button (Light, Heavy, downAttack, etc if there are any more)
    public void checkForAttackInput()
    {
        if (Input.GetButtonDown("LightAtt") && !Input.GetButton("Down"))
        {
            checkLightAttack();
        } else if (Input.GetButtonDown("HeavyAtt"))
        {
            checkHeavyAttack();
        } else if(Input.GetButtonDown("LightAtt") && Input.GetButton("Down"))
        {
            checkDownAttack();
        }
    }

    public void checkLightAttack()
    {
        foreach (CombatAttacks attack in Attacks)
        {
            if(currentAttack.nextLightAttack == attack.AttackName)
            {
                if (attack.isUnlocked)
                {
                    isAttacking = true;
                    currentAttack = attack;

                    attRender.setAttackArray("Light", attack.AttackName);
                    attackTime = animator.GetCurrentAnimatorStateInfo(0).length;

                   
                    if (currentAttack.endOfAttackString)
                    {
                        
                    }
                    break;
                }
            }
        }
    }

    public void checkHeavyAttack()
    {
        foreach(CombatAttacks attack in Attacks)
        {
            if(currentAttack.nextHeavyAttack == attack.AttackName)
            {
                if (attack.isUnlocked)
                {
                    currentAttack = attack;

                    //checkMovement();
                    //attackEnemy();

                    attRender.setAttackArray("Heavy", attack.AttackName);

                    attackTime = animator.GetCurrentAnimatorStateInfo(0).length;
                    if (currentAttack.endOfAttackString)
                    {
                        //currentAttack = Attacks[0];
                    }
                    break;
                }
            }
        }
    }

    public void checkDownAttack()
    {
        
        foreach (CombatAttacks attack in Attacks)
        {
            if (currentAttack.nextDownAttack == attack.AttackName)
            {
                if (attack.isUnlocked)
                {
                    currentAttack = attack;
                    

                    attRender.setAttackArray("Down", attack.AttackName);
                    attackTime = animator.GetCurrentAnimatorStateInfo(0).length;
                    

                    if (currentAttack.endOfAttackString)
                    {
                        //currentAttack = Attacks[0];
                    }

                    break;
                }
            }
        }
    }

    public void checkMovement()
    {
        if (currentAttack.willMoveHor)
        {
            bonusSpeed = currentAttack.movementChange.x;
            playerChar.setPlayerExtraSpeed(bonusSpeed);
        }

        if (currentAttack.willMoveVert)
        {
            rb.velocity = Vector2.up * currentAttack.movementChange.y;
        }
    }

    public void attackEnemy()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);

        foreach(Collider2D detectedEnemy in hitEnemies)
        {
            HurtEnemy damagedEnemy = detectedEnemy.GetComponent<HurtEnemy>();
            damagedEnemy.hurtEnemyFunc(currentAttack.knockback.willKnockback, currentAttack.damage, currentAttack.knockback.knockbackForce, transform, currentAttack.knockback.knockbackDur);
            shakeCamera(.1f);

        }

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, objectsLayer);

        foreach(Collider2D detectedObject in hitObjects)
        {
            
            DestructibleObject foundObject = detectedObject.GetComponent<DestructibleObject>();
            foundObject.DamageObject(currentAttack.damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }


    void shakeCamera(float shake)
    {
        StartCoroutine(shakeScreen(shake));
    }

    public IEnumerator shakeScreen(float noise)
    {
        CinemachineBasicMultiChannelPerlin cinemachineNoise = cineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineNoise.m_AmplitudeGain = noise;
        yield return new WaitForSeconds(.6f);
        cinemachineNoise.m_AmplitudeGain = 0f;
    }

}
