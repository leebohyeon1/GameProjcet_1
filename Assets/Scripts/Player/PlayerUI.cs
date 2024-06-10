using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private PlayerStats playerStats;
    private PlayerMovement playerMovement;
    //==========================================================

    void Start()
    {
        if (playerStats == null) { playerStats = GetComponent<PlayerStats>(); }

        if (playerMovement == null) { playerMovement = GetComponent<PlayerMovement>(); }
    }

    void Update()
    {
       
        UpdateUI();
    }
    //==========================================================

    void UpdateUI()
    {
        UIManager.Instance.HealthBarValue(playerStats.maxHealth, playerStats.curHealth);

        UIManager.Instance.StaminaBarValue(playerStats.maxStamina, playerStats.curStamina);
       
        UIManager.Instance.UpdateLockOnUIPosition(playerMovement.GetLockOnTarget());
    }
}
