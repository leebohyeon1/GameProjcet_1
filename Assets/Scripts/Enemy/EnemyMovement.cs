using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemyStats;

public class EnemyMovement : MonoBehaviour
{
    private EnemyStats enemyStats;
    private NavMeshAgent agent;
    private Transform player;

    private float orbitTime;
    private float randTime;
    private bool isOrbitClockwise;
    //==========================================================

    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = enemyStats.Speed;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        orbitTime = 0;
        GetRandomTime();
    }

    void Update()
    {    
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= enemyStats.attackDistance + 0.5f)
        {
            enemyStats.enemyState = EnemyState.Combat;
        }
        else //if (distanceToPlayer <= enemyStats.chaseDistance)
        {
            enemyStats.enemyState = EnemyState.Chase;
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
    //==========================================================

    void HandleCombatState()
    {
        if (agent.isActiveAndEnabled)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

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

    #region EnemyMode
    void BoundaryMode()
    {
        Vector3 direction = (transform.position - player.position).normalized;
        Vector3 Position = player.position + direction * enemyStats.attackDistance;

        transform.position = Vector3.MoveTowards(transform.position, Position, enemyStats.Speed * Time.deltaTime);
        LookAtPlayer();
    }

    void DareDevilMode()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        if (Vector3.Distance(player.position, transform.position) > enemyStats.attackDistance / 2)
        {
            agent.isStopped = false;
            agent.speed = enemyStats.Speed * 1.5f;
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
        Vector3 orbitPosition = player.position + Quaternion.Euler(0, enemyStats.rotationSpeed * orbitDirection * Time.deltaTime, 0) * direction * enemyStats.attackDistance;
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
            agent.speed = enemyStats.Speed;
            agent.SetDestination(player.position);
        }
    }

    void Attack()
    {


    }
    //==========================================================

    void LookAtPlayer()
    {
        Vector3 directionToTarget = (player.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
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
