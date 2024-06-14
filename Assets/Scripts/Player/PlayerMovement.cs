using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerStats playerStats;
    private PlayerInput playerInput;
    private PlayerCombat playerCombat;
    private Animator animator;
    //==========================================================

    [Space(20f)]
    public bool ikActive = false;
    public Transform rightHandObj = null;
    public Transform leftHandObj = null;
    //==========================================================

    private float dodgeTimer;
    Vector3 dodgeDirection;
    //==========================================================

    //public LayerMask enemyLayer;
    [SerializeField] private GameObject Enemy;
   private Transform lockOnTarget;
    private float turnSmoothVelocity;
    //==========================================================

    void Start()
    {
        if (rb == null) { rb = GetComponent<Rigidbody>(); }
        if (playerStats == null) { playerStats = GetComponent<PlayerStats>(); }
        if (playerInput == null) { playerInput = GetComponent<PlayerInput>(); }
        if (playerCombat == null) { playerCombat = GetComponent<PlayerCombat>(); }
        if (animator == null) { animator = transform.GetChild(0).GetComponent<Animator>(); }

        //플레이어 회전 잠금
        rb.freezeRotation = true;
        //attackIndex = 0;
        //enemyLayer = LayerMask.GetMask("Enemy");

        Enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    void Update()
    {
        if (playerStats.playerState != PlayerState.Die) { UpdateMovement(); }

    }

    void FixedUpdate()
    {
        if (playerStats.playerState != PlayerState.Die) { FixedUpdateMovement(); }
    }


    void OnAnimatorIK()
    {
        if (animator)
        {
            if (ikActive)
            {
                // Right hand IK
                if (rightHandObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.5f);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0.5f);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
                }

                // Left hand IK
                if (leftHandObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
                }
            }
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            }
        }
    }
    //==========================================================

    void UpdateMovement() //애니메이션 및 입력 처리
    {

        //구르기 시작
        if (playerInput.DodgePressed && !playerStats.isDodging && !playerStats.isAttack)
        {
            StartDodge(playerInput.Horizontal, playerInput.Vertical);
        }

        //락온/오프
        if (playerInput.LockOnPressed && !playerStats.isLockOn && !playerStats.isDodging)
        {
            LockOnToTarget();
        }
        else if (playerInput.LockOnPressed && playerStats.isLockOn)
        {
            UnlockTarget();
        }

        //락온 가능 범위 밖으로 벗어나면 락온 해제
        //if (lockOnTarget != null&&playerStats.isLockOn && PlayerToLockOnTargetDistance() > playerStats.lockOnRange)
        //{
        //    UnlockTarget();
        //}

        //마우스 휠로 락온 타겟 변경
        //if (playerStats.isLockOn && playerInput.scroll != 0)
        //{
        //    LockOnToNextTarget();
        //}


    }

    void FixedUpdateMovement() //물리적 행동
    {
        if (playerStats.isDodging)
        {
            Dodge();
        }
        else
        {
            if (!playerStats.isAttack)
            {
                Move(playerInput.Horizontal, playerInput.Vertical);
            }
        }
        //락온 상대 바라보기
        if (playerStats.isLockOn && !playerStats.isDodging)
        {
            LookLockOnTarget();
        }
    }

    public void Move(float Horizontal, float Vertical) //앞뒤좌우 움직임
    {
        Vector3 direction = new Vector3(Horizontal, 0f, Vertical).normalized;

        if (direction.magnitude > 0f)
        {
            animator.SetBool("isMove", true);
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            MoveAnimate(Horizontal, Vertical);

            if (!playerStats.isLockOn)
            {
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, playerStats.turnSmoothTime);
                rb.rotation = Quaternion.Euler(0f, angle, 0f);
            }

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.velocity = moveDirection.normalized * playerStats.speed;

        }
        else
        {
            animator.SetBool("isMove", false);
            rb.velocity = Vector3.zero;
        }
    }

    void MoveAnimate(float horizontal, float vertical)
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
    public void StartDodge(float Horizontal, float Vertical) //구르기 시작
    {
        if (playerStats.curStamina > playerStats.dodgeStaminaCost)
        {

            playerCombat.EndCheckAttack();
            animator.SetTrigger("Roll");

            EventManager.Instance.PostNotification(EVENT_TYPE.PLAYER_ACT, this, true);
            playerStats.isDodging = true;
            dodgeTimer = playerStats.dodgeDuration;
            playerStats.curStamina -= playerStats.dodgeStaminaCost;

            dodgeDirection = new Vector3(Horizontal, 0f, Vertical).normalized;

            if (dodgeDirection.magnitude > 0f)
            {
                float targetAngle = Mathf.Atan2(dodgeDirection.x, dodgeDirection.z) * Mathf.Rad2Deg;
                rb.rotation = Quaternion.Euler(0f, targetAngle, 0f);
                dodgeDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;               
            }
            else
            {
                dodgeDirection = transform.forward;
            }
        }

    }

    public void Dodge() //구르기 중(playerStats.dodgeDuration 동안 일정한 속도로 구르기를 함);
    {
        dodgeTimer -= Time.fixedDeltaTime;
        if (dodgeTimer <= 0f)
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.PLAYER_ACT, this, false);
            playerStats.isDodging = false;
            playerStats.isAttack = false;
            rb.velocity = Vector3.zero;
            return;
        }

        rb.velocity = dodgeDirection * playerStats.dodgeSpeed;

    }
    #endregion

    #region LockOn
    public void LockOnToTarget()
    {
        if (Enemy != null)
        {
            lockOnTarget = Enemy.GetComponentInParent<EnemyStats>().Point;
            UIManager.Instance.ShowLockOnUI(Enemy.transform);
            playerStats.isLockOn = true;
        }
    }

    public void LookLockOnTarget()
    {
        if (lockOnTarget != null)
        {
            Vector3 directionToTarget = (Enemy.transform.position - transform.position).normalized;
            float targetAngle = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;
            rb.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        }
    }

    public void UnlockTarget()
    {
        UIManager.Instance.HideLockOnUI();
        playerStats.isLockOn = !playerStats.isLockOn;
    }

    public Transform GetLockOnTarget()
    {
        return lockOnTarget;
    }
    #endregion

}
