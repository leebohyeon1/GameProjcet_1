using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderHandler : MonoBehaviour
{
    EnemyStats EnemyStats;
    private void Start()
    {
        EnemyStats = GetComponentInParent<EnemyStats>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStats>().TakeDamage(EnemyStats.damage);
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
