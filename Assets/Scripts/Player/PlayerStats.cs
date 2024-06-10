using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IListener
{
    private PlayerMovement playerMovemt;
    //==========================================================

    [Header("ü��")]
    public float maxHealth = 100f; //�ִ� ü��
    [SerializeField]
    private float currentHealth; //���� ü��
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
    public float hpRecoveryTime = 0.07f; //ü�� ȸ�� �ֱ�
    public float hpRecoveryValue = 0.07f; //ü�� ȸ�� �ֱ⺰ ��

    [Header("���׹̳�")]
    public float maxStamina = 100; //�ִ� ���¹̳�
    [SerializeField]
    private float currentStamina; //���� ���׹̳�
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
    public float staminaRecoveryTime = 0.05f; //���׹̳� ȸ�� �ֱ�
    public float staminaRecoveryValue = 0.2f; //���׹̳� ȸ�� �ֱ⺰ ��
    public float StaminaRecoveryBeginsAfterAction = 0.7f;// �ൿ �� ���׹̳� ���۱��� �ð�

    [Header("���׹̳� ��뷮")]
    public float attackStaminaCost = 15f; //���� �� ���׹̳� �Ҹ�
    public float dodgeStaminaCost = 10f; //������ �� ���׹̳� �Ҹ�

    [Header("�̵� �ӵ�")]
    public float speed = 12f; //�̵� �ӵ�
    public float turnSmoothTime = 0.01f; //ȸ�� �ð� (���� �������� ȸ���� ���� ��)

    [Header("������")]
    public float dodgeSpeed = 17f; //������ �ӵ�
    public float dodgeDuration = 0.5f; //������ �ð�

    [Header("����")]
    public float Damage;
    private GameObject equippedWeapon;

    [Header("�� ��")]
    public float measuringAngle = 45f; //�÷��̾� ���鿡�� ���� �ν� ����
    public float lockOnRange = 15f; //���� �ν� ����
    [Space(30f)]
    //==========================================================

    [Header("���� ����")]
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
