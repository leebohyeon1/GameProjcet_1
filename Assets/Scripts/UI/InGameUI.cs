using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public Slider hpSlider;
    public Slider staminaBarSlider;

    public Image lockOnIndicator;
    private Transform currentTarget;
    private Camera playerCamera;
    //==========================================================

    void Start()
    {
        if (hpSlider == null) { hpSlider = GameObject.Find("hpBar").GetComponent<Slider>(); }
        if (staminaBarSlider == null) { staminaBarSlider = GameObject.Find("staminaBar").GetComponent<Slider>(); }

        if (lockOnIndicator == null) { lockOnIndicator = GameObject.Find("lockOnIndicator").GetComponent<Image>(); }
        if (playerCamera == null) { playerCamera = Camera.main; }
        lockOnIndicator.enabled = false;
    }
    //==========================================================

    #region Hp
    public void HpValue(float MaxVal, float CurVal)
    {
        hpSlider.value = CurVal / MaxVal;
    }
    #endregion

    #region Stamina
    public void StaminaValue(float MaxVal, float CurVal)
    {
        staminaBarSlider.value = CurVal / MaxVal;
    }
    #endregion

    #region LockOn
    public void Show(Transform target)
    {
        lockOnIndicator.enabled = true;
        currentTarget = target;
        UpdatePosition(target);
    }

    public void Hide()
    {
        lockOnIndicator.enabled = false;
        currentTarget = null;
    }

    public void UpdatePosition(Transform target)
    {
        if (lockOnIndicator.enabled && currentTarget != null)
        {
            Vector3 screenPosition = playerCamera.WorldToScreenPoint(target.position);
            lockOnIndicator.transform.position = screenPosition;
        }
    }
    #endregion
    //==========================================================

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (playerCamera == null) { playerCamera = Camera.main; }
    }

    private void OnEnable()
    {
        // SceneManager.sceneLoaded 이벤트에 OnSceneLoaded 메서드를 구독.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // SceneManager.sceneLoaded 이벤트에서 OnSceneLoaded 메서드를 구독 해제.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
