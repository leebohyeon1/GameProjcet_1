using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerStats playerStats;
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private Animator animator;
    //==========================================================   
    private static readonly int hasgAttackCount = Animator.StringToHash("AttackCount");

    public int AttackCount
    {
        get => animator.GetInteger(hasgAttackCount);
        set => animator.SetInteger(hasgAttackCount, value);
    }

    private float attackTimer;
    public float[] attackTime;
    public float[] waitingTime;

    private bool isAttackScan;
    //==========================================================

    void Start()
    {
        if (rb == null) { rb = GetComponent<Rigidbody>(); }
        if (playerStats == null) { playerStats = GetComponent<PlayerStats>(); }
        if (playerInput == null) { playerInput = GetComponent<PlayerInput>(); }
        if (playerMovement == null) { playerMovement = GetComponent<PlayerMovement>(); }
        if (animator == null) { animator = transform.GetChild(0).GetComponent<Animator>(); }

        AttackCount = 0;
        isAttackScan = false;
    }

    void Update()
    {
        if (playerStats.playerState == PlayerState.Die){ return; }
        //타격
        InitialAttackInput();

        UpdateAttackTimer();

    }
    //==========================================================

    #region Attack

    private void InitialAttackInput()
    {
        if (playerInput.AttackPressed && !playerStats.isAttack)
        {
            Attack();
        }
    }

    private void UpdateAttackTimer()
    {
        if (!playerStats.isAttack) return;

        if (playerInput.AttackPressed && !isAttackScan && AttackCount < attackTime.Length)
        {
            Attack();
        }

        attackTimer += Time.deltaTime;

        if (attackTimer >= waitingTime[AttackCount])
        {
            isAttackScan = false;

            if (attackTimer >= attackTime[AttackCount])
            {
                EndAttack();
            }
        }
    }
    public void Attack()
    {
        if (playerStats.curStamina > playerStats.attackStaminaCost)
        {
           
            rb.velocity = Vector3.zero;                  
            animator.SetTrigger("Attack");
            playerStats.isAttack = true;
            isAttackScan = true;
            animator.SetBool("isMove", false);
        }
    }
    
    public void CheckAttack()
    {
        playerStats.curStamina -= playerStats.attackStaminaCost;
        Debug.Log("현재 카운트: " + AttackCount);
        AttackCount++;     
        if(AttackCount >= attackTime.Length)
        {
            AttackCount = attackTime.Length -1;
        }
        EventManager.Instance.PostNotification(EVENT_TYPE.CHECK_ATTACK, this, true);
        EventManager.Instance.PostNotification(EVENT_TYPE.PLAYER_ACT, this, true);
    }

    public void EndCheckAttack()
    {
        //playerStats.isAttack = false;
        EventManager.Instance.PostNotification(EVENT_TYPE.CHECK_ATTACK, this, false);
        EventManager.Instance.PostNotification(EVENT_TYPE.PLAYER_ACT, this, false);
        //animator.SetBool("isMove", true);
    }

    private void EndAttack()
    {
        playerStats.isAttack = false;
        AttackCount = 0;
        attackTimer = 0;
        animator.CrossFade("Idle", 0f);
    }

    public void SetTimer()
    {
        attackTimer = 0;
    }
    #endregion

    public void TakeDamage(float damage)
    {
        if (playerStats.isDodging)
        {
            return;
        }
        playerStats.curHealth -= damage;

        if (playerStats.curHealth <= 0)
        {
            playerStats.playerState = PlayerState.Die;
            rb.velocity = Vector3.zero;
            animator.SetTrigger("Death");
            GetComponent<CapsuleCollider>().enabled = false;
        }

    }
}
