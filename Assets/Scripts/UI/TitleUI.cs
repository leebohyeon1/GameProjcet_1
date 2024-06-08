using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleUI : MonoBehaviour
{
    //==========================================================

    void Start()
    {
       
    }


    void Update()
    {
        
    }
    //==========================================================

    #region Button
    public void StartBtn()
    {
        SceneManager.LoadScene(1);
        EventManager.Instance.PostNotification(EVENT_TYPE.SCENE_LOAD, this, 1);
    }

    public void ExitBtn()
    {
        Application.Quit();
    }

    public void OptionBtn()
    {
        UIManager.Instance.OptionUISet(true);
    }
    #endregion

}
