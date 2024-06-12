using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static EnemyStats;

public class EnemyMovement : MonoBehaviour,IListener
{
    private EnemyStats enemyStats;
    private NavMeshAgent agent;
    private Transform player;
    private Rigidbody rb;
    private Animator animator;

    private float orbitTime;
    private float randTime;
    private bool isOrbitClockwise;
    private float turnSmoothVelocity;

    int hasgAttackCount = Animator.StringToHash("AttackCount");
    public int AttackCount
    {
        get => animator.GetInteger(hasgAttackCount);
        set => animator.SetInteger(hasgAttackCount, value);
    }

    public Transform wideAttackPos;
    public Transform longAttackPos;
    public TrailRenderer trail;
    //==========================================================

    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.ENEMY_STATE, this);

        rb = GetComponent<Rigidbody>();
        enemyStats = GetComponent<EnemyStats>();
        agent = GetComponent<NavMeshAgent>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        agent.speed = enemyStats.speed;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        orbitTime = 0;     
        GetRandomTime();
        animator.SetBool("isMove", true);
      
    }

    void Update()
    {
        if (enemyStats.enemyState == EnemyState.Die)
        {
           
            return;
        }

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= enemyStats.attackDistance && enemyStats.enemyState != EnemyState.Combat)
        {
            enemyStats.enemyState = EnemyState.Combat;
            EventManager.Instance.PostNotification(EVENT_TYPE.ENEMY_STATE,this, EnemyState.Combat);
        }
        else if (distanceToPlayer > enemyStats.attackDistance && enemyStats.enemyState != EnemyState.Chase )
        {
            enemyStats.enemyState = EnemyState.Chase;
            EventManager.Instance.PostNotification(EVENT_TYPE.ENEMY_STATE, this, EnemyState.Chase);
        }

        switch (enemyStats.enemyState)
        {
            case EnemyState.Combat:
                HandleCombatState();
                break;
            case EnemyState.Chase:
                ChasePlayer();
                break;
        } 

    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch(Param)
        {
            case EnemyState.Combat:
                //animator.SetBool("isMove", false);
                StartCoroutine("AttackRoof");
                break;
            case EnemyState.Chase:
                animator.SetBool("isMove",true);
                StopCoroutine("AttackRoof");
                break;
            case EnemyState.Die:
                animator.SetTrigger("Die");
                StopCoroutine("AttackRoof");
                break;
        }
    }
    //==========================================================

    void HandleCombatState()
    {
        if (agent.isActiveAndEnabled)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        if (!enemyStats.isAttack)
        {
            switch (enemyStats.enemyPersonality)
            {
                case EnemyPersonality.Cautious:
                    BoundaryMode();
                    break;
                case EnemyPersonality.Aggressive:
                    DareDevilMode();
                    break;
                case EnemyPersonality.Cold:
                    ColdMode();
                    break;
            }
        }
     
    }

    #region EnemyMode
    void BoundaryMode()
    {
        Vector3 direction = (transform.position - player.position).normalized;
        Vector3 Position = player.position + direction * enemyStats.attackDistance;

        rb.position = Vector3.MoveTowards(transform.position, Position, enemyStats.speed * Time.deltaTime);
        LookAtPlayer();
    }

    void DareDevilMode()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        if (Vector3.Distance(player.position, transform.position) > enemyStats.attackDistance / 2)
        {
            agent.isStopped = false;
            agent.speed = enemyStats.speed * 1.5f;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
            agent.ResetPath();
            LookAtPlayer();
        }
    }

    void ColdMode()
    {    
        orbitTime += Time.deltaTime;
        RotateChange();
        Vector3 direction = (transform.position - player.position).normalized;
        float orbitDirection = isOrbitClockwise ? 1 : -1;
        Vector3 orbitPosition = player.position + Quaternion.Euler(0, enemyStats.rotationSpeed * orbitDirection * Time.deltaTime, 0) * direction * enemyStats.attackDistance/2;
        transform.position = Vector3.MoveTowards(transform.position, orbitPosition, enemyStats.boundarySpeed * Time.deltaTime);
     
        //transform.RotateAround(player.position, orbitDirection, enemyStats.rotationSpeed * Time.deltaTime);
        LookAtPlayer();
    }
    #endregion

    void ChasePlayer()
    {
        if (agent.isActiveAndEnabled)
        {
            agent.isStopped = false;
            agent.speed = enemyStats.speed;
            agent.SetDestination(player.position);
        }
    }

    //void Attack()
    //{
    //    attackTimer += Time.deltaTime;
    //    if (attackTimer > enemyStats.attackTime)
    //    {
    //        attackTimer = 0;
    //        animator.SetTrigger("Attack");
    //    }

    //}

    IEnumerator AttackRoof()
    {
        while (enemyStats.enemyState != EnemyState.Die)
        {
            // Wait for a random interval between 2 and 5 seconds
            float waitTime = Random.Range(0.5f, 1.7f);        
            yield return new WaitForSeconds(waitTime);
            AttackCount = Random.Range(0, 3);
            // Select and execute a random pattern
            //animator.SetInteger("AttackCount", Attackindex);
            animator.SetTrigger("Attack");
          
           
        }
    
    }

    public void Attack()
    {
        wideAttackPos.GetComponent<BoxCollider>().enabled = true;
        enemyStats.isAttack = true;
        trail.enabled = true;
    }

    public void EndAttack()
    {
        wideAttackPos.GetComponent<BoxCollider>().enabled = false;
        trail.enabled = false;
        enemyStats.isAttack = false;
    }

    public void LongAttack()
    {
        longAttackPos.GetComponent<BoxCollider>().enabled = true;
        enemyStats.isAttack = true;
        trail.enabled = true;
    }
    public void EndLongAttack()
    {
        longAttackPos.GetComponent<BoxCollider>().enabled = false;
        trail.enabled = false;
        enemyStats.isAttack = false;
    }
    //==========================================================

    void LookAtPlayer()
    {
        Vector3 directionToTarget = (player.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.23f);
        rb.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    void RotateChange()
    {
        if (orbitTime >= randTime)
        {
            isOrbitClockwise = !isOrbitClockwise;
            orbitTime = 0;
            GetRandomTime();
        }
    }

    void GetRandomTime()
    {
        randTime = Random.Range(1.2f, 2f);
    }

}
