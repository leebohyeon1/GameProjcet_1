using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimEvent : MonoBehaviour
{
    //EnemyMovement EnemyMovement;
    EnemyCombat EnemyCombat;

    private void Start()
    {
        EnemyCombat = GetComponentInParent<EnemyCombat>();
    }

    void StartAttack()
    {
        EnemyCombat.Attack();
    }

    void StartAttackCheck()
    {
        EnemyCombat.AttackCheck();
    }

    void EndAttackCheck()
    {
        EnemyCombat.EndAttackCheck();
    }

    void LongAttackCheck()
    {
        EnemyCombat.LongAttackCheck();
    }

    void EndLongAttackCheck()
    {
        EnemyCombat.EndLongAttackCheck();
    }

    void EndAttack()
    {
        EnemyCombat.EndAttack();
    }

    void StartJumpAttack()
    {
        EnemyCombat.StartJumpAttack();
    }

    void EndJumpAttack()
    {
        EnemyCombat.EndJumpAttack();
    }
}
