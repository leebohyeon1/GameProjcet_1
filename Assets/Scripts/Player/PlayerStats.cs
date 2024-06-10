using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IListener
{
    private PlayerMovement playerMovemt;
    //==========================================================

    [Header("체력")]
    public float maxHealth = 100f; //최대 체력
    [SerializeField]
    private float currentHealth; //현재 체력
    public float curHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
            if (currentHealth <= 0)
            {
                currentHealth = 0f;
                Die();
            }
        }
    }
    public float hpRecoveryTime = 0.07f; //체력 회복 주기
    public float hpRecoveryValue = 0.07f; //체력 회복 주기별 값

    [Header("스테미나")]
    public float maxStamina = 100; //최대 스태미나
    [SerializeField]
    private float currentStamina; //현재 스테미나
    public float curStamina
    {
        get
        {
            return currentStamina;
        }
        set
        {
            currentStamina = value;
            if(currentStamina <= 0f)
            {
                currentStamina = 0f;           
            }
        }
    }
    public float staminaRecoveryTime = 0.05f; //스테미나 회복 주기
    public float staminaRecoveryValue = 0.2f; //스테미나 회복 주기별 값
    public float StaminaRecoveryBeginsAfterAction = 0.7f;// 행동 후 스테미나 시작까지 시간

    [Header("스테미나 사용량")]
    public float attackStaminaCost = 15f; //공격 시 스테미나 소모량
    public float dodgeStaminaCost = 10f; //구르기 시 스테미나 소모량

    [Header("이동 속도")]
    public float speed = 12f; //이동 속도
    public float turnSmoothTime = 0.01f; //회전 시간 (값이 낮을수록 회전을 빨리 함)

    [Header("구르기")]
    public float dodgeSpeed = 17f; //구르기 속도
    public float dodgeDuration = 0.5f; //구르기 시간

    [Header("공격")]
    public float Damage;
    private GameObject equippedWeapon;

    [Header("락 온")]
    public float measuringAngle = 45f; //플레이어 정면에서 락온 인식 각도
    public float lockOnRange = 15f; //락온 인식 범위
    [Space(30f)]
    //==========================================================

    [Header("현재 상태")]
    public PlayerState playerState;
    public bool isDodging;
    public bool isAttackScan;
    public bool isAttack;
    public bool isBlocking;
    public bool isLockOn;
    public bool isStaminaRecovery;
    //========================================================== 
    [Space(20f)]
    public Transform rightHandIKTarget;
    public Transform leftHandIKTarget;
    public GameObject weaponPrefab;

    //==========================================================
    private float staminaTimer;
    private float hpTimer;
    //==========================================================

    void Start()
    {      
        currentHealth = maxHealth;
        currentStamina = maxStamina;

        playerState = PlayerState.Nomal;

        isStaminaRecovery = true;
        isDodging = false;
        isAttackScan = false;
        isAttack = false;
        isBlocking = false;
        isLockOn = false;

        playerMovemt = GetComponent<PlayerMovement>();
        EquipWeapon(weaponPrefab);
        EventManager.Instance.AddListener(EVENT_TYPE.PLAYER_ACT, this);

    }

    void Update()
    {
        if(isStaminaRecovery)
        {
            AutoRecoverStamina();
        } 
        AutoRecoverHp();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lockOnRange);

        Gizmos.color = Color.red;

    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.PLAYER_ACT:
                isStaminaRecovery = !(bool)Param;
                break;
        }
    }
    //==========================================================

    void Die()
    {
        playerState = PlayerState.Die;
    }

    public void TakeDamage(float damage)
    {
        if(isDodging)
        {
            return;
        }
        curHealth -= damage;

    }

    public void AutoRecoverStamina()
    {
        staminaTimer += Time.deltaTime;

        if (staminaTimer > staminaRecoveryTime)
        {
            if (curStamina < maxStamina)
            {
                curStamina += staminaRecoveryValue;
                if (curStamina > maxStamina)
                {
                    curStamina = maxStamina;
                }
            }
            staminaTimer = 0f;
        }

    }

    public void AutoRecoverHp()
    {
        hpTimer += Time.deltaTime;

        if (hpTimer > hpRecoveryTime)
        {
            if (curHealth < maxHealth)
            {
                curHealth += hpRecoveryValue;
                if (curHealth > maxHealth)
                {
                    curHealth = maxHealth;
                }
            }
            hpTimer = 0f;
        }

    }

    public void EquipWeapon(GameObject weapon)
    {
        if (equippedWeapon != null)
        {
            Destroy(equippedWeapon);
        }

        equippedWeapon = Instantiate(weapon, rightHandIKTarget.position, rightHandIKTarget.rotation, rightHandIKTarget);
        equippedWeapon.transform.localPosition = Vector3.zero;
        equippedWeapon.transform.localRotation = Quaternion.identity;

        playerMovemt.rightHandObj = rightHandIKTarget;
        playerMovemt.leftHandObj = leftHandIKTarget;
        playerMovemt.ikActive = true;
    }
}
