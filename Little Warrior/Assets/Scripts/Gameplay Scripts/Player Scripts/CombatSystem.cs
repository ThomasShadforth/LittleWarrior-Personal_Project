using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CombatSystem : MonoBehaviour
{
    //The player's animator component
    Animator animator;

    //The attacks the player has/can use (depending on if it has been unlocked or not)
    public CombatAttacks[] Attacks;
    //The attack the player is currently using
    public CombatAttacks currentAttack;


    public float attackTime;
    //Speed that is applied to the player character (For moving the player forward on attack)
    float bonusSpeed;
    //The player's rigidbody
    Rigidbody2D rb;
    //Detection radius for attacks
    public float attackRadius;
    //The player script (Accesses values to set when attacking to prevent movement in certain cases)
    BasePlayer playerChar;
    //The point where attacks are detected using the raidus
    public Transform attackPoint;
    //Whether the player is currently attacking or not (Prevents attacks from being spammed repeatedly
    bool isAttacking;
    //Layermasks used for checking for enemies or destructible objects
    public LayerMask enemyLayer, objectsLayer;
    //Reference to the attack renderer script (Responsible for playing animations)
    [SerializeField]
    AttackRenderer attRender;

    //The type of attack (Light, heavy, etc.)
    string attackType;

    //Reference to the main camera (For the purpose of applying camera shake)
    CinemachineVirtualCamera cineCam;


    void Start()
    {
        //Setting the initial values (The attack at pos 0 is just the player's idle state)
        currentAttack = Attacks[0];
        rb = GetComponent<Rigidbody2D>();
        playerChar = GetComponent<BasePlayer>();
        animator = GetComponent<Animator>();
        cineCam = FindObjectOfType<CinemachineVirtualCamera>();
        
        //Sets up the links between attacks that have been un
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

        /*if(attackTime > 0)
        {
            attackTime -= Time.deltaTime;
            //Debug.Log(attackTime);
        }
        
        if(attackTime <= 0)
        {
            currentAttack = Attacks[0];
            
            playerChar.setPlayerExtraSpeed(0f);
        }*/

        
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

    //Used to set up links between all attacks in the array (ensures attacks can be chained into one another properly where possible)
    public void setUpAttackLinks()
    {
        //Goes from the end of the attacks array to the start
        for(int i = Attacks.Length - 1; i >= 0; i--)
        {
            
            //Get the name of the current attack entry
            string nameOfAttack = Attacks[i].AttackName;

            for(int j = Attacks.Length - 1; j >= 0; j--)
            {
                //If the current attack's name is the same as the next attack for the second loop, then set the current attack's previous attack name to that of the current name in the secondary loop
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
        //This applies across all attack methods
        //Cycles through each of the attacks in the array
        foreach (CombatAttacks attack in Attacks)
        {
            //If the attack's name is the same as the next attack that leads off of the current attack, then check if it is unlocked
            if(currentAttack.nextLightAttack == attack.AttackName)
            {
                //If the attack is unlocked, then use the attack
                if (attack.isUnlocked)
                {
                    isAttacking = true;
                    currentAttack = attack;
                    playerChar.isAttacking = true;
                    //Pass the attack type and the name of the attack to the attack renderer
                    attRender.setAttackArray("Light", attack.AttackName);
                    //attackTime = animator.GetCurrentAnimatorStateInfo(0).length;
                    //attackTime -= 2;

                    attackType = "Light Attack";

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
                    playerChar.isAttacking = true;
                    //checkMovement();
                    //attackEnemy();

                    attRender.setAttackArray("Heavy", attack.AttackName);
                    attackType = "Heavy Attack";
                    //attackTime = animator.GetCurrentAnimatorStateInfo(0).length;
                    
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
                    playerChar.isAttacking = true;

                    attRender.setAttackArray("Down", attack.AttackName);

                    //attackTime = animator.GetCurrentAnimatorStateInfo(0).length;

                    //attackTime -= 2;

                    attackType = "Light Attack";

                    if (currentAttack.endOfAttackString)
                    {
                        //currentAttack = Attacks[0];
                    }

                    break;
                }
            }
        }
    }

    //Used to check whether or not movement is applied during an attack (Horizontal or Vertical)
    public void checkMovement()
    {
        //If willMoveHor, adds horizontal speed to the player
        if (currentAttack.willMoveHor)
        {
            bonusSpeed = currentAttack.movementChange.x;
            playerChar.setPlayerExtraSpeed(bonusSpeed);
        }

        //If willMoveVert, applies a jump force to the player that pushes them upwards
        if (currentAttack.willMoveVert)
        {
            rb.velocity = Vector2.up * currentAttack.movementChange.y;
        }
    }

    
    //Enemy/Destructible detection
    public void attackEnemy()
    {
        //Checks for any enemies present within the attack radius (Which is not too large)
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);

        //get the hurt enemy component, and damage the enemy (As well as play a sound effect)
        foreach(Collider2D detectedEnemy in hitEnemies)
        {
            HurtEnemy damagedEnemy = detectedEnemy.GetComponent<HurtEnemy>();
            damagedEnemy.playAttackSFX(attackType);
            damagedEnemy.hurtEnemyFunc(currentAttack.knockback.willKnockback, currentAttack.damage, currentAttack.knockback.knockbackForce, transform, currentAttack.knockback.knockbackDur);
            
            //Apply camera shake
            shakeCamera(.06f);

        }

        //Detects destructible objects
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, objectsLayer);

        //Damage any detected destructibles
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

    //Briefly apply camera shake to the cinemachine camera
    void shakeCamera(float shake)
    {
        StartCoroutine(shakeScreen(shake));
    }

    public IEnumerator shakeScreen(float noise)
    {
        //Set the amplitude gain in order to cause camera shake
        CinemachineBasicMultiChannelPerlin cinemachineNoise = cineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineNoise.m_AmplitudeGain = noise;
        yield return new WaitForSeconds(.6f);
        //Return ampGain to 0 to stop shaking
        cinemachineNoise.m_AmplitudeGain = 0f;
        
    }

}
