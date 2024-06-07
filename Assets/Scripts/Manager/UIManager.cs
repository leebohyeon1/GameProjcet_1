using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour, IListener
{ 
    public static UIManager Instance { get; private set; }

    public TitleUI titleUI;
    public InGameUI InGameUI;
    //==========================================================

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.SCENE_LOAD, this);

        if (titleUI == null) { titleUI = FindObjectOfType<TitleUI>(); }

        if (InGameUI == null) { InGameUI = FindObjectOfType<InGameUI>(); }

        TitleUISet(false);
        GameUISet(false);

        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                TitleUISet();
                break;
            case 1:
                GameUISet();
                break;
        }

    }
    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Param)
        {
            case 0:
                TitleUISet();
                GameUISet(false);
                break;
            case 1:
                TitleUISet(false);
                GameUISet(true);
                break;
        }
    }  
    //==========================================================

    #region PlayerUI
    public void ShowLockOnUI(Transform target)
    {
        InGameUI.Show(target);
    }

    public void HideLockOnUI()
    {
        InGameUI.Hide();
    }

    public void UpdateLockOnUIPosition(Transform target)
    {
        InGameUI.UpdatePosition(target);
    }

    public void HealthBarValue(float MaxVal, float CurVal)
    {
        InGameUI.HpValue(MaxVal, CurVal);
    }

    public void StaminaBarValue(float MaxVal, float CurVal)
    {
        InGameUI.StaminaValue(MaxVal, CurVal);
    }
    #endregion

    #region SetSceneUI
    void TitleUISet(bool On = true)
    {
        titleUI.gameObject.SetActive(On);
    }

    void GameUISet(bool On = true)
    {
        InGameUI.gameObject.SetActive(On);
    }

    #endregion  
}
