using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCombat : MonoBehaviour
{
    private EnemyStats enemyStats;
    private Transform player;
    private Rigidbody rb;
    private Animator animator;
    //==========================================================

    int hasgAttackCount = Animator.StringToHash("AttackCount");
    public int AttackCount
    {
        get => animator.GetInteger(hasgAttackCount);
        set => animator.SetInteger(hasgAttackCount, value);
    }
    int hasAttackRange = Animator.StringToHash("AttackRange");
    public int AttackRange
    {
        get => animator.GetInteger(hasAttackRange);
        set => animator.SetInteger(hasAttackRange, value);
    }
    private float attackTimer;
    private bool isJump = false;
    Vector3 jumpTarget;
    //==========================================================

    [SerializeField] private Transform wideAttackPos;
    [SerializeField] private Transform longAttackPos;
    [SerializeField] private TrailRenderer trail;
    //==========================================================

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        enemyStats = GetComponent<EnemyStats>();

        animator = transform.GetChild(0).GetComponent<Animator>();

        attackTimer = enemyStats.attackCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyStats.enemyState == EnemyState.CD_Combat || enemyStats.enemyState == EnemyState.LD_COMBAT)
        {
            HandleCombat();
        }

        if (isJump)
        {
           
            rb.position = Vector3.MoveTowards(transform.position, jumpTarget, enemyStats.speed * 2.5f * Time.deltaTime);

        }
    }
    //==========================================================

    private void HandleCombat()
    {
        if (enemyStats.isAttack) return;

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            if (enemyStats.enemyState == EnemyState.CD_Combat)
            {
                PerformCloseAttack();
            }
            else if (enemyStats.enemyState == EnemyState.LD_COMBAT)
            {
               // PerformLongAttack();
            }
            attackTimer = Random.Range(0.5f,enemyStats.attackCooldown);
        }
    }

    #region Attack

    void PerformCloseAttack() //근거리 공격패턴
    {
        AttackRange = 0;
        AttackCount = Random.Range(0, 3);  
        switch (AttackCount)
        {
            case 0:

                break;
            case 1:

                break;
            case 2:
               
                break;
        }
        animator.SetTrigger("Attack");
       
    }

    void PerformLongAttack()
    {
        AttackRange = 1;
        AttackCount = Random.Range(3, 6);

        animator.SetTrigger("Attack");
    }
    //==========================================================

    #endregion
    public void StartJumpAttack()
    {
        isJump = true;
        Vector3 direction = (player.position - transform.position).normalized;
        jumpTarget = player.position - direction * (enemyStats.closeDistanceAttackRange/3);
    }
    public void EndJumpAttack()
    {
        isJump = false;
        rb.velocity = Vector3.zero;

    }
    #region AttackAnim
    public void Attack()
    {
        enemyStats.isAttack = true;
    }

    public void AttackCheck()
    {
        wideAttackPos.GetComponent<BoxCollider>().enabled = true;        
        trail.enabled = true;
    }

    public void EndAttackCheck()
    {
        wideAttackPos.GetComponent<BoxCollider>().enabled = false;
        trail.enabled = false;
    }

    public void LongAttackCheck()
    {
        longAttackPos.GetComponent<BoxCollider>().enabled = true;
        //enemyStats.isAttack = true;
        trail.enabled = true;
    }

    public void EndLongAttackCheck()
    {
        longAttackPos.GetComponent<BoxCollider>().enabled = false;
        trail.enabled = false;
    }

    public void EndAttack()
    {
        enemyStats.isAttack = false;
        enemyStats.enemyState = EnemyState.Idle;
    }
    #endregion

    public void TakeDamage(float damage)
    {
        if (enemyStats.curHP > 0)
        {
            enemyStats.curHP -= damage;
            if (enemyStats.curHP <= 0)
            {
                enemyStats.curHP = 0;
                enemyStats.enemyState = EnemyState.Die;
                FindObjectOfType<PlayerMovement>().UnlockTarget();
                animator.SetTrigger("Die");
            }
        }
    }
}

