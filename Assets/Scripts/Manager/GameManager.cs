using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerState
{
    Nomal, Die
}

public enum EnemyState
{ 
    Combat, Chase, Die
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    //==========================================================

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1);
            EventManager.Instance.PostNotification(EVENT_TYPE.SCENE_LOAD, this,1);
        }
    }
}
