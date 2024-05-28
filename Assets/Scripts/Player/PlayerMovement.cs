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
    private Animator animator;
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
        if(animator == null) { animator = transform.GetChild(0).GetComponent<Animator>(); }
       
        //�÷��̾� ȸ�� ���
        rb.freezeRotation = true;
        cam = Camera.main;
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    void Update()
    {
        if (playerStats.playerState != PlayerState.Die) { UpdateMovement(); }

    }

    //==========================================================
    void UpdateMovement()
    {

        //������ ����
        if (playerInput.DodgePressed) {StartDodge(playerInput.Horizontal, playerInput.Vertical); }

        //Ÿ��
        if (playerInput.AttackPressed)
        {
            Attack();
        }

        //���� ��� �ٶ󺸱�
        if (playerStats.isLockOn && !playerStats.isDodging)
        {
            LookLockOnTarget();
        }

        //����/����
        if (playerInput.LockOnPressed && !playerStats.isLockOn && !playerStats.isDodging)
        {
            LockOnToClosestTarget();
        }
        else if (playerInput.LockOnPressed && playerStats.isLockOn)
        {
            UnlockTarget();
        }

        //���� ���� ���� ������ ����� ���� ����
        if (playerStats.isLockOn && PlayerToLockOnTargetDistance() > playerStats.lockOnRange)
        {
            UnlockTarget();
        }

        //���콺 �ٷ� ���� Ÿ�� ����
        if (playerStats.isLockOn && playerInput.scroll != 0)
        {
            LockOnToNextTarget();
        }

        //������, �����̱�
        if (playerStats.isDodging)
        {
            Dodge();
        }
        
        if(!playerStats.isDodging && !playerStats.isAttacking)
        {
            Move(playerInput.Horizontal, playerInput.Vertical);
        }

        if(Input.GetKeyDown(KeyCode.V))
        {
            playerStats.TakeDamage(10);
        }
    }
    public void Move(float Horizontal, float Vertical) //�յ��¿� ������
    {
        Vector3 direction = new Vector3(Horizontal, 0f, Vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            animator.SetBool("isMove", true);
            //Vector3 cameraForward = cam.transform.forward;
            //Vector3 cameraRight = cam.transform.right;

            //cameraForward.y = 0f; // ī�޶��� ���� ������ ����
            //cameraRight.y = 0f; // ī�޶��� ���� ������ ����

            //cameraForward.Normalize();
            //cameraRight.Normalize();

            //Vector3 moveDirection = cameraForward * direction.z + cameraRight * direction.x;
           
            Animate(Horizontal, Vertical);
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, playerStats.turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.velocity = moveDirection.normalized * playerStats.speed;

        }
        else
        {
            animator.SetBool("isMove", false);
            rb.velocity = Vector3.zero;
        }
    }
    void Animate(float horizontal, float vertical)
    {
        // Create a direction vector based on input
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Convert world direction to local direction
            Vector3 localDirection = transform.InverseTransformDirection(direction);

            float forward = localDirection.z;
            float strafe = localDirection.x;

            // Determine the animation parameters based on local direction
            float animVertical = 0;
            float animHorizontal = 0;

            if (Mathf.Abs(localDirection.z) >= 0.2f)
            {
                animVertical = localDirection.z > 0 ? localDirection.z : -localDirection.z;
            }
            if (Mathf.Abs(localDirection.x) >= 0.2f)
            {
                animHorizontal = localDirection.x > 0 ? localDirection.x : -localDirection.x;
            }

            // Set animator parameters
            animator.SetFloat("x", animHorizontal);
            animator.SetFloat("z", animVertical);
        }
        else
        {
            animator.SetFloat("x", 0);
            animator.SetFloat("z", 0);
        }
    }
    #region Dodge
    public void StartDodge(float Horizontal, float Vertical) //������ ����
    {
        animator.SetTrigger("Roll");
        if (playerStats.curStamina > playerStats.dodgeStaminaCost )
        {
            playerStats.isDodging = true;
            dodgeTimer = playerStats.dodgeDuration;
            playerStats.curStamina -= playerStats.dodgeStaminaCost;

            dodgeDirection = new Vector3(Horizontal, 0f, Vertical).normalized;

            //Vector3 cameraForward = cam.transform.forward;
            //Vector3 cameraRight = cam.transform.right;

            //cameraForward.y = 0f; // ī�޶��� ���� ������ ����
            //cameraRight.y = 0f; // ī�޶��� ���� ������ ����

            //cameraForward.Normalize();
            //cameraRight.Normalize();

            //Vector3 DodgeDirection = cameraForward * dodgeDirection.z + cameraRight * dodgeDirection.x;

            if (dodgeDirection.magnitude >= 0.01f)
            {
                float targetAngle = Mathf.Atan2(dodgeDirection.x, dodgeDirection.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
                dodgeDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            }
            else
            {
                dodgeDirection = -transform.forward;
            }
        }

    }

    public void Dodge() //������ ��(playerStats.dodgeDuration ���� ������ �ӵ��� �����⸦ ��);
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
    public void LockOnToClosestTarget() //�÷��̾� ���鿡�� �¿�� 45���� �ȿ� playerStats.lockOnRange�ȿ� ���� ������ ���� �����
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
            animator.SetTrigger("Slash");
            //playerStats.isAttacking = true;
            //Collider[] hitEnemies = Physics.OverlapBox(playerStats.AttackPos.position, playerStats.attackRange, Quaternion.identity, enemyLayer);
            //foreach (Collider enemy in hitEnemies)
            //{
            //    Debug.Log("attack to: "+ enemy.name);
            //    //enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            //}
        }
    }
    public void EndAttack()
    {
        playerStats.isAttacking =false;
    }


   

    //==========================================================

}
