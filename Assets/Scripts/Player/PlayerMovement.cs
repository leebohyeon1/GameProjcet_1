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
    private PlayerInput playerInput;
    //==========================================================

    public LayerMask enemyLayer;
    [SerializeField] private Transform lockOnTarget;
    private float turnSmoothVelocity;
    private float dodgeTimer;
    Vector3 dodgeDirection;
    //==========================================================

    void Start()
    {
        
        if(rb == null) { rb = GetComponent<Rigidbody>(); }
        if(playerStats == null) { playerStats = GetComponent<PlayerStats>(); }
        if(playerInput == null) { playerInput = GetComponent<PlayerInput>(); }
       
        //플레이어 회전 잠금
        rb.freezeRotation = true;
        cam = Camera.main;
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    void Update()
    {
        UpdateMovement();
    }

    //==========================================================
    void UpdateMovement()
    {

        //구르기 시작
        if (playerInput.DodgePressed) {StartDodge(playerInput.Horizontal, playerInput.Vertical); }

        //타격
        if (playerInput.AttackPressed)
        {
            Attack();
        }

        //락온 상대 바라보기
        if (playerStats.isLockOn && !playerStats.isDodging)
        {
            LookLockOnTarget();
        }

        //락온/오프
        if (playerInput.LockOnPressed && !playerStats.isLockOn && !playerStats.isDodging)
        {
            LockOnToClosestTarget();
        }
        else if (playerInput.LockOnPressed && playerStats.isLockOn)
        {
            UnlockTarget();
        }

        //락온 가능 범위 밖으로 벗어나면 락온 해제
        if (playerStats.isLockOn && PlayerToLockOnTargetDistance() > playerStats.lockOnRange)
        {
            UnlockTarget();
        }

        //마우스 휠로 락온 타겟 변경
        if (playerStats.isLockOn && playerInput.scroll != 0)
        {
            LockOnToNextTarget();
        }

        //구르기, 움직이기
        if (playerStats.isDodging)
        {
            Dodge();
        }
        else
        {
            Move(playerInput.Horizontal, playerInput.Vertical);
        }
    }
    public void Move(float Horizontal, float Vertical) //앞뒤좌우 움직임
    {
        Vector3 direction = new Vector3(Horizontal, 0f, Vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            Vector3 cameraForward = cam.transform.forward;
            Vector3 cameraRight = cam.transform.right;

            cameraForward.y = 0f; // 카메라의 수직 성분을 제거
            cameraRight.y = 0f; // 카메라의 수직 성분을 제거

            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = cameraForward * direction.z + cameraRight * direction.x;

            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, playerStats.turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.velocity = moveDirection.normalized * playerStats.speed;
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

            dodgeDirection = new Vector3(Horizontal, 0f, Vertical).normalized;

            Vector3 cameraForward = cam.transform.forward;
            Vector3 cameraRight = cam.transform.right;

            cameraForward.y = 0f; // 카메라의 수직 성분을 제거
            cameraRight.y = 0f; // 카메라의 수직 성분을 제거

            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 DodgeDirection = cameraForward * dodgeDirection.z + cameraRight * dodgeDirection.x;

            if (dodgeDirection.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(DodgeDirection.x, DodgeDirection.z) * Mathf.Rad2Deg;
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

    public Transform GetLockOnTarget()
    {
        return lockOnTarget;
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

   

    //==========================================================

}
