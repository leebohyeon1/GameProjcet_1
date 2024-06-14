using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    EnemyStats EnemyStats;
    EnemyCombat EnemyCombat;
    private void Start()
    {
        EnemyStats = GetComponentInParent<EnemyStats>();
        EnemyCombat = GetComponentInParent<EnemyCombat>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerCombat>().TakeDamage(EnemyStats.damage[EnemyCombat.AttackCount]);
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
