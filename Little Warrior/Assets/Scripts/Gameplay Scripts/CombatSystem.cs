using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    public CombatAttacks[] Attacks;
    public CombatAttacks currentAttack;

    float attackTime;
    float bonusSpeed;
    Rigidbody2D rb;

    BasePlayer playerChar;

    void Start()
    {
        currentAttack = Attacks[0];
        rb = GetComponent<Rigidbody2D>();
        playerChar = GetComponent<BasePlayer>();
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

    //checked throughout the update method, detects when the player uses any of the attack buttons
    //Each branch will execute the corresponding attack check for the button (Light, Heavy, downAttack, etc if there are any more)
    public void checkForAttackInput()
    {
        if (Input.GetButtonDown("LightAtt"))
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
                    currentAttack = attack;
                    attackTime = currentAttack.attackDur;

                    checkMovement();

                    if (currentAttack.endOfAttackString)
                    {
                        currentAttack = Attacks[0];
                        return;
                    }
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
                    attackTime = currentAttack.attackDur;

                    checkMovement();

                    if (currentAttack.endOfAttackString)
                    {
                        currentAttack = Attacks[0];
                    }
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
                    attackTime = currentAttack.attackDur;

                    checkMovement();

                    if (currentAttack.endOfAttackString)
                    {
                        currentAttack = Attacks[0];
                    }
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

}
