using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRenderer : MonoBehaviour
{
    public static readonly string idleState = "Idle";
    public static readonly string[] lightAttacks = {"Punch"};
    public static readonly string[] heavyAttacks = {"Kick", "Rising Uppercut", "Dive Kick" };
    public static readonly string[] downAttacks = {"Slide Kick", "Sweep Kick" };

    Animator animator;
    BasePlayer player;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<BasePlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setAttackArray(string attackType, string AttackName)
    {
        string[] attackArray = null ;
        if(attackType == "Light")
        {
            attackArray = lightAttacks;
        } else if(attackType == "Heavy")
        {
            attackArray = heavyAttacks;
        } else if(attackType == "Down")
        {
            attackArray = downAttacks;
        }

        playAttackAnim(AttackName, attackArray);
    }

    public void playAttackAnim(string name, string[] attacks)
    {
        
        for(int i = 0; i < attacks.Length; i++)
        {
            if(name == attacks[i])
            {
                animator.Play(attacks[i]);
                player.GetComponent<CombatSystem>().attackTime = (animator.GetCurrentAnimatorClipInfo(0).Length);

                break;
            }
        }
    }

    public void haltAirSpeed()
    {
        player.rb.velocity = new Vector2(player.rb.velocity.x, 2.5f);
        StartCoroutine(stopDescent());
    }

    public IEnumerator stopDescent()
    {
        
        player.rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        yield return new WaitForSeconds(.35f);
        player.rb.constraints = RigidbodyConstraints2D.None;
        player.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void endAnim()
    {
        animator.Play(idleState);
        CombatSystem playerCombat = player.GetComponent<CombatSystem>();
        playerCombat.currentAttack = playerCombat.Attacks[0];
        player.setPlayerExtraSpeed(0);
        player.isAttacking = false;
    }
}
