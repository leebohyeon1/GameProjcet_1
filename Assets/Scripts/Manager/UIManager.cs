using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour, IListener
{ 
    public static UIManager Instance { get; private set; }

    public TitleUI titleUI;
    public InGameUI inGameUI;
    public OptionUI optionUI;
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
        if (inGameUI == null) { inGameUI = FindObjectOfType<InGameUI>(); }
        if(optionUI == null) { optionUI = FindObjectOfType<OptionUI>(); } 

        TitleUISet(false);
        GameUISet(false);
        OptionUISet(false);

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OptionUISet();
        }
    }
    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        //switch (Param)
        //{
        //    case 0:
        //        TitleUISet();
        //        GameUISet(false);
        //        OptionUISet(false);
        //        break;
        //    case 1:
        //        TitleUISet(false);
        //        GameUISet();
        //        OptionUISet(false);
        //        break;
        //}
    }  
    //==========================================================

    #region PlayerUI
    public void ShowLockOnUI(Transform target)
    {
        inGameUI.Show(target);
    }

    public void HideLockOnUI()
    {
        inGameUI.Hide();
    }

    public void UpdateLockOnUIPosition(Transform target)
    {
        inGameUI.UpdatePosition(target);
    }

    public void HealthBarValue(float MaxVal, float CurVal)
    {
        inGameUI.HpValue(MaxVal, CurVal);
    }

    public void StaminaBarValue(float MaxVal, float CurVal)
    {
        inGameUI.StaminaValue(MaxVal, CurVal);
    }
    #endregion

    #region SetSceneUI
    public void TitleUISet(bool On = true)
    {
        titleUI.gameObject.SetActive(On);
    }

    public void GameUISet(bool On = true)
    {
        inGameUI.gameObject.SetActive(On);
    }

    public void OptionUISet(bool On = true)
    {
        optionUI.gameObject.SetActive(On);
    }

    #endregion

    #region BossUI
    public void EnemyHealthBarValue(float MaxVal, float CurVal)
    {
        inGameUI.EnemyHpValue(MaxVal,CurVal);
    }
    #endregion

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                TitleUISet();
                GameUISet(false);
                OptionUISet(false);
                break;
            case 1:
                TitleUISet(false);
                GameUISet();
                OptionUISet(false);
                break;
        }
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
