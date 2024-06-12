using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimEvent : MonoBehaviour
{
    EnemyMovement EnemyMovement;

    private void Start()
    {
        EnemyMovement = GetComponentInParent<EnemyMovement>();
    }

    void StartAttack()
    {
        EnemyMovement.Attack();
    }

    void EndAttack()
    {
        EnemyMovement.EndAttack();
    }

    void LongAttack()
    {
        EnemyMovement.LongAttack();
    }

    void EndLongAttack()
    {
        EnemyMovement.EndLongAttack();
    }
}
