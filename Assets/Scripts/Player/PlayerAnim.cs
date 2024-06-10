using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    PlayerMovement playerMovement;
    //==========================================================

    void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }
    //==========================================================

    void EndCheckAttack()
    {
        playerMovement.EndCheckAttack();
    }

    void CheckAttack()
    {
        playerMovement.CheckAttack();
    }
}
