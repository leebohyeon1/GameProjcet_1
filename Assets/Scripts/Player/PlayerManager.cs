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

        //구르기 시작
        if (playerInput.DodgePressed) { playerMovement.StartDodge(playerInput.Horizontal, playerInput.Vertical); }
        //타격
        if (playerInput.AttackPressed)
        {
            playerMovement.Attack();
        }

        //락온 상대 바라보기
        if (playerStats.isLockOn && !playerStats.isDodging)
        {
           playerMovement.LookLockOnTarget();
        }

        //락온/오프
        if (playerInput.LockOnPressed && !playerStats.isLockOn && !playerStats.isDodging)
        {
            playerMovement.LockOnToClosestTarget();
        }
        else if (playerInput.LockOnPressed && playerStats.isLockOn)
        {
            playerMovement.UnlockTarget();
        }

        //락온 가능 범위 밖으로 벗어나면 락온 해제
        if (playerStats.isLockOn && playerMovement.PlayerToLockOnTargetDistance() > playerStats.lockOnRange)
        {
            playerMovement.UnlockTarget();
        }

        //마우스 휠로 락온 타겟 변경
        if (playerStats.isLockOn && playerInput.scroll != 0)
        {
            playerMovement.LockOnToNextTarget();
        }

        //구르기, 움직이기
        if (playerStats.isDodging)
        {
            playerMovement.Dodge();
        }
        else
        {
            playerMovement.Move(playerInput.Horizontal, playerInput.Vertical);
        }

        //피격
        if (Input.GetKeyDown(KeyCode.K))
        {
            playerMovement.TakeDamage(10f);
        }
    }
    //==========================================================

    void UpdateStats()
    {
        //체력 자동 회복
        playerStats.AutoRecoverHp();
        //스테미나 자동 회복
        playerStats.AutoRecoverStamina();

    }
   
}
