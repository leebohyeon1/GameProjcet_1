using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerStats playerStats;
    private PlayerMovement playerMovement;
    private PlayerUI playerUI;
    //==========================================================

    void Start()
    {
        if (playerInput == null) { playerInput = GetComponent<PlayerInput>(); }
       
        if (playerStats == null) { playerStats = GetComponent<PlayerStats>(); }
        
        if (playerMovement == null) { playerMovement = GetComponent<PlayerMovement>(); }
       
        if (playerUI == null) { playerUI = GetComponent<PlayerUI>(); }
    }

    void Update()
    {
        if (playerStats.playerState == PlayerState.Die)
        {
            return;
        }

        UpdateUI();
        UpdateMovement();
        UpdateStats();
    }
    //==========================================================

    void UpdateUI()
    {
        playerUI.HpBarValue(playerStats.maxHealth,  playerStats.curHealth);

        playerUI.StaminaBarValue(playerStats.maxStamina, playerStats.curStamina);
    }
    //==========================================================

    void UpdateMovement()
    {

        //������ ����
        if (playerInput.DodgePressed) { playerMovement.StartDodge(playerInput.Horizontal, playerInput.Vertical); }
        //Ÿ��
        if (playerInput.AttackPressed)
        {
            playerMovement.Attack();
        }

        //���� ��� �ٶ󺸱�
        if (playerStats.isLockOn && !playerStats.isDodging)
        {
           playerMovement.LookLockOnTarget();
        }

        //����/����
        if (playerInput.LockOnPressed && !playerStats.isLockOn && !playerStats.isDodging)
        {
            playerMovement.LockOnToClosestTarget();
        }
        else if (playerInput.LockOnPressed && playerStats.isLockOn)
        {
            playerMovement.UnlockTarget();
        }

        //���� ���� ���� ������ ����� ���� ����
        if (playerStats.isLockOn && playerMovement.PlayerToLockOnTargetDistance() > playerStats.lockOnRange)
        {
            playerMovement.UnlockTarget();
        }

        //���콺 �ٷ� ���� Ÿ�� ����
        if (playerStats.isLockOn && playerInput.scroll != 0)
        {
            playerMovement.LockOnToNextTarget();
        }

        //������, �����̱�
        if (playerStats.isDodging)
        {
            playerMovement.Dodge();
        }
        else
        {
            playerMovement.Move(playerInput.Horizontal, playerInput.Vertical);
        }

        //�ǰ�
        if (Input.GetKeyDown(KeyCode.K))
        {
            playerMovement.TakeDamage(10f);
        }
    }
    //==========================================================

    void UpdateStats()
    {
        //ü�� �ڵ� ȸ��
        playerStats.AutoRecoverHp();
        //���׹̳� �ڵ� ȸ��
        playerStats.AutoRecoverStamina();

    }
   
}
