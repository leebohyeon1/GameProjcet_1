using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
