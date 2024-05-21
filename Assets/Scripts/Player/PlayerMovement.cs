using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{   
    private Camera cam;
    private Rigidbody rb;
    private PlayerStats playerStats;
    //==========================================================

    public LayerMask enemyLayer;
    [SerializeField] private Transform lockOnTarget;
    private float turnSmoothVelocity;
    private float dodgeTimer;
    Vector3 dodgeDirection;
    //==========================================================

    void Start()
    {
        rb = GetComponent<Rigidbody>();     
        playerStats = GetComponent<PlayerStats>();

        rb.freezeRotation = true;
        cam = Camera.main;
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    //==========================================================

    public void Move(float Horizontal, float Vertical) //앞뒤좌우 움직임
    {
        float horizontal = Horizontal;
        float vertical = Vertical;
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg  + cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, playerStats.turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.velocity = moveDir.normalized * playerStats.speed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    #region Dodge
    public void StartDodge(float Horizontal, float Vertical) //구르기 시작
    {
        if (playerStats.curStamina > playerStats.dodgeStaminaCost )
        {
            playerStats.isDodging = true;
            dodgeTimer = playerStats.dodgeDuration;
            playerStats.curStamina -= playerStats.dodgeStaminaCost;

            float horizontal = Horizontal;
            float vertical = Vertical;
            dodgeDirection = new Vector3(horizontal, 0f, vertical).normalized;


            if (dodgeDirection.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(dodgeDirection.x, dodgeDirection.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
                transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
                dodgeDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            }
            else
            {
                dodgeDirection = -transform.forward;
            }
        }

    }

    public void Dodge() //구르기 중(playerStats.dodgeDuration 동안 일정한 속도로 구르기를 함);
    {
        if (!playerStats.isDodging)
        {
            return;
        }

        dodgeTimer -= Time.deltaTime;
        if (dodgeTimer <= 0f)
        {
             playerStats.isDodging = false;
            rb.velocity = Vector3.zero;
            return;
        }

        rb.velocity = dodgeDirection.normalized * playerStats.dodgeSpeed;
    }
    #endregion

    #region LockOn
    public void LockOnToClosestTarget() //플레이어 정면에서 좌우로 45각도 안에 playerStats.lockOnRange안에 적이 있으면 제일 가까운
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, playerStats.lockOnRange, enemyLayer);
        if (enemies.Length == 0) return;

        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider enemy in enemies)
        {
            Vector3 directionToEnemy = enemy.transform.position - transform.position;
            float distanceToEnemy = directionToEnemy.magnitude;
            //float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);

            if (/*angleToEnemy < playerStats.measuringAngle &&*/ distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy.transform;
            }
        }

        if (closestEnemy != null)
        {
            LockOnToTarget(closestEnemy);       
        }       
    }

    public void LockOnToNextTarget()
    {

        Collider[] enemies = Physics.OverlapSphere(transform.position, playerStats.lockOnRange, enemyLayer);
        if (enemies.Length <= 1) return;
        
        Transform nextTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider enemy in enemies)
        {
            if (enemy.transform == lockOnTarget) continue;

            Vector3 directionToEnemy = enemy.transform.position - transform.position;
            float distanceToEnemy = directionToEnemy.magnitude;
            //float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);

            if (/*angleToEnemy < playerStats.measuringAngle &&*/ distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                nextTarget = enemy.transform;
            }          
        }

        if (nextTarget != null)
        {
            LockOnToTarget(nextTarget);
        }
    }

    public void LookLockOnTarget()
    {
        Vector3 directionToTarget = (lockOnTarget.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
    }

    public float PlayerToLockOnTargetDistance()
    {
        return Vector3.Distance(transform.position, lockOnTarget.position);     
    }

    void LockOnToTarget(Transform target)
    {
        lockOnTarget = target;
        UIManager.Instance.ShowLockOnUI(target);
        playerStats.isLockOn = true;
    }

    public void UnlockTarget()
    {
        UIManager.Instance.HideLockOnUI();
        lockOnTarget = null;
        playerStats.isLockOn = !playerStats.isLockOn;
    }
    #endregion

    public void Attack()
    {
        if (playerStats.curStamina > playerStats.attackStaminaCost)
        {
            playerStats.curStamina -= playerStats.attackStaminaCost;

            //Collider[] hitEnemies = Physics.OverlapBox(playerStats.AttackPos.position, playerStats.attackRange, Quaternion.identity, enemyLayer);
            //foreach (Collider enemy in hitEnemies)
            //{
            //    Debug.Log("attack to: "+ enemy.name);
            //    //enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            //}
        }
    }

    public void TakeDamage(float damage)
    {
        playerStats.curHealth -= damage;
        
    }

    //==========================================================

}
