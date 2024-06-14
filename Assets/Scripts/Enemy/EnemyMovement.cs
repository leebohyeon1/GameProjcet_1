using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static EnemyStats;

public class EnemyMovement : MonoBehaviour
{
    private EnemyStats enemyStats;
    private NavMeshAgent agent;
    [SerializeField] private Transform player;
    private Rigidbody rb;
    private Animator animator;

    private float turnSmoothVelocity;
    //==========================================================

    void Start()
    {

        rb = GetComponent<Rigidbody>();
        enemyStats = GetComponent<EnemyStats>();
        agent = GetComponent<NavMeshAgent>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        agent.speed = enemyStats.speed;

        animator.SetBool("isMove", true);
        enemyStats.enemyState = EnemyState.Idle;
    }

    void Update()
    {
        if (enemyStats.enemyState == EnemyState.Die)
        {          
            return;
        }

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);


        HandleIdleState(distanceToPlayer);
  

            //case EnemyState.Chase:
            //    HandleChaseState();
            //    break;

            //case EnemyState.CD_Combat:
            //    HandleCloseCombatState();
            //    break;
            //case EnemyState.LD_COMBAT:
            //    HandleLongCombatState();
            //   break;
        if (!enemyStats.isAttack)
        {
            MoveTowards(player.position);
        }
    }
    //==========================================================
    private void HandleIdleState(float distanceToPlayer)
    {
        if (distanceToPlayer <= enemyStats.closeDistanceAttackRange)
        {
            enemyStats.enemyState = EnemyState.CD_Combat;
        }
        else if (distanceToPlayer <= enemyStats.longDistanceAttackRange)
        {
            enemyStats.enemyState = EnemyState.LD_COMBAT;
        }
        else
        {
            enemyStats.enemyState = EnemyState.Idle;
        }
    }

    private void HandleChaseState()
    {
        MoveTowards(player.position);
        enemyStats.enemyState = EnemyState.Idle;
    }

    private void HandleCloseCombatState()
    {
        if (!enemyStats.isAttack)
        {
            MoveTowards(player.position);
        }
    }

    private void HandleLongCombatState()
    {
        if (!enemyStats.isAttack)
        {
            MoveTowards(player.position);
            //enemyStats.enemyState = EnemyState.Idle;
        }
    }
    //==========================================================

    private void MoveTowards(Vector3 targetPosition)
    {
        animator.SetBool("isMove", true);
        Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 target = targetPosition - direction * (enemyStats.closeDistanceAttackRange/3);
        Vector3 newPosition = Vector3.MoveTowards(transform.position, target, enemyStats.speed * Time.deltaTime);
        rb.MovePosition(newPosition);
        LookAtPlayer();
    }

    public void Combat()
    {
        enemyStats.enemyState = EnemyState.CD_Combat;
        agent.isStopped = true;
    }

    public void Chase()
    {
        enemyStats.enemyState = EnemyState.Chase;
        agent.isStopped = false;
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

}
