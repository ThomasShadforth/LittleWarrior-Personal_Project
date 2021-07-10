using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EnemyAttackSystem : MonoBehaviour
{
    public LayerMask playerLayer;
    public float attackRadius = .1f;
    public Transform attackPoint;
    bool canAttackAgain;
    EnemyAi baseEnemy;
    HurtPlayer hurtPlayer;
    Animator animator;

    CinemachineVirtualCamera cineCam;

    AnimatorStateInfo currentStateInfo;
    AnimatorClipInfo[] currentClipInfo;
    float animTime;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        baseEnemy = GetComponent<EnemyAi>();
        hurtPlayer = GetComponent<HurtPlayer>();
        cineCam = FindObjectOfType<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
        currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        animTime = currentClipInfo[0].clip.length * currentStateInfo.normalizedTime;
        
    }

    private void FixedUpdate()
    {
        if (!baseEnemy.TargetInAttackRange() && baseEnemy.isAttacking)
        {
            
            animator.SetBool("attackAgain", false);
            canAttackAgain = false;
            if (animTime > currentClipInfo[0].clip.length)
            {
                baseEnemy.isAttacking = false;
                animator.SetBool("isAttacking", false);
            }
        }
    }

    public void attack(float endVal)
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, playerLayer);

        foreach(Collider2D player in hitPlayer)
        {

            hurtPlayer.hurtThePlayer(player.GetComponent<BasePlayer>());

            shakeCamera(.1f);
            if (endVal == 0)
            {
                if (!canAttackAgain && baseEnemy.TargetInAttackRange())
                {
                    canAttackAgain = true;
                    animator.SetBool("attackAgain", true);
                }
                
                
            }
            else
            {
                animator.SetBool("isAttacking", false);
                animator.SetBool("attackAgain", false);
                baseEnemy.isAttacking = false;
                canAttackAgain = false;
            }

            
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
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
