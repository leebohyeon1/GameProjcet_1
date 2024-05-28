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
            EventManager.Instance.PostNotification(EVENT_TYPE.HEALTH_CHANGE,this,currentHealth);
            if (currentHealth <= 0)
            {
                currentHealth = 0f;
                Die();
            }
        }
    }
    public float hpRecoveryTime = 0.05f; //ü�� ȸ�� �ֱ�
    public float hpRecoveryValue = 0.1f; //ü�� ȸ�� �ֱ⺰ ��

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
    public float staminaRecoveryValue = 0.5f; //���׹̳� ȸ�� �ֱ⺰ ��

    [Header("���׹̳� ��뷮")]
    public float attackStaminaCost = 15f; //���� �� ���׹̳� �Ҹ�
    public float dodgeStaminaCost = 10f; //������ �� ���׹̳� �Ҹ�

    [Header("�̵� �ӵ�")]
    public float speed = 6f; //�̵� �ӵ�
    public float turnSmoothTime = 0.1f; //ȸ�� �ð� (���� �������� ȸ���� ���� ��)

    [Header("������")]
    public float dodgeSpeed = 12f; //������ �ӵ�
    public float dodgeDuration = 0.2f; //������ �ð�

    //[Header("����")]
    //public Transform AttackPos; 
    //public float attackRange;

    [Header("�� ��")]
    public float measuringAngle = 45f; //�÷��̾� ���鿡�� ���� �ν� ����
    public float lockOnRange = 15f; //���� �ν� ����
    [Space(30f)]
    //==========================================================

    [Header("���� ����")]
    public PlayerState playerState;
    public bool isDodging;
    public bool isAttacking;
    public bool isBlocking;
    public bool isLockOn;
    //==========================================================

    private float staminaTimer;
    private float hpTimer;
    //==========================================================

    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.HEALTH_CHANGE, this);

        currentHealth = maxHealth;
        currentStamina = maxStamina;

        playerState = PlayerState.Nomal;

        isDodging = false;
        isAttacking = false;
        isBlocking = false;
        isLockOn = false;
    }

    void Update()
    {
        AutoRecoverStamina();
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
            case EVENT_TYPE.HEALTH_CHANGE:
                if((float)Param < 50)
                {
                    Die();
                }
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
