using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStats;

public enum PlayerState
{
    Nomal,
    Die
}

public class PlayerStats : MonoBehaviour
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
        currentHealth = maxHealth;
        currentStamina = maxStamina;

        playerState = PlayerState.Nomal;

        isDodging = false;
        isAttacking = false;
        isBlocking = false;
        isLockOn = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lockOnRange);

        Gizmos.color = Color.red;

    }
    //==========================================================

    void Die()
    {
        playerState = PlayerState.Die;
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
