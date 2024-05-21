using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public LockOnUI lockOnUI;
    public HealthBarUI healthBarUI;
    public StaminaBarUI staminaBarUI;
    //==========================================================

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        if (lockOnUI == null) { lockOnUI = FindObjectOfType<LockOnUI>(); }

        if (healthBarUI == null) { healthBarUI = FindObjectOfType<HealthBarUI>(); }

        if (staminaBarUI == null) { staminaBarUI = FindObjectOfType<StaminaBarUI>(); }
    }
    //==========================================================

    public void ShowLockOnUI(Transform target)
    {
        lockOnUI.Show(target);
    }

    public void HideLockOnUI()
    {
        lockOnUI.Hide();
    }

    public void HealthBarValue(float MaxVal, float CurVal)
    {
        healthBarUI.HpValue(MaxVal, CurVal);
    }

    public void StaminaBarValue(float MaxVal, float CurVal)
    {
        staminaBarUI.StaminaValue(MaxVal, CurVal);
    }
}
