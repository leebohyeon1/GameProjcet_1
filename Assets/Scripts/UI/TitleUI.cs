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
    public void StartGame()
    {
        SceneManager.LoadScene(1);
        EventManager.Instance.PostNotification(EVENT_TYPE.SCENE_LOAD, this, 1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion

}
