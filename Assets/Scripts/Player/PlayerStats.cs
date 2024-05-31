using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IListener
{
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
    public Transform Weapon;


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

    private float staminaTimer;
    private float hpTimer;
    //==========================================================

    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.PLAYER_ACT, this);
        currentHealth = maxHealth;
        currentStamina = maxStamina;

        playerState = PlayerState.Nomal;

        isStaminaRecovery = true;
        isDodging = false;
        isAttackScan = false;
        isAttack = false;
        isBlocking = false;
        isLockOn = false;
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
        isStaminaRecovery = !(bool)Param;
    }
    //==========================================================

    void Die()
    {
        playerState = PlayerState.Die;
    }

    public void TakeDamage(float damage)
    {
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
}
