using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    EnemyStats EnemyStats;
    EnemyMovement EnemyMovement;
    private void Start()
    {
        EnemyStats = GetComponentInParent<EnemyStats>();
        EnemyMovement = GetComponentInParent<EnemyMovement>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerCombat>().TakeDamage(EnemyStats.damage[EnemyMovement.AttackCount]);
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
