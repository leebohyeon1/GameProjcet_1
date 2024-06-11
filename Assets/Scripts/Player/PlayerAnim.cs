using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    PlayerCombat playerCombat;
    //==========================================================

    void Start()
    {
        playerCombat = GetComponentInParent<PlayerCombat>();
    }
    //==========================================================

    void EndCheckAttack()
    {
        playerCombat.EndCheckAttack();
    }

    void CheckAttack()
    {
        playerCombat.CheckAttack();
    }

    void SetTimer()
    {
        playerCombat.SetTimer();
    }
}
