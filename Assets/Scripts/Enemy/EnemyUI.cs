using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUI : MonoBehaviour
{
    EnemyStats enemyStats;

    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        UIManager.Instance.EnemyHealthBarValue(enemyStats.maxHP,enemyStats.curHP);
    }
}
