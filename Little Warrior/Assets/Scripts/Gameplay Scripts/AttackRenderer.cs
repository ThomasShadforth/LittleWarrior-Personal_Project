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
                break;
            }
        }
    }

    public void endAnim()
    {
        animator.Play(idleState);
        player.setPlayerExtraSpeed(0);
    }
}
