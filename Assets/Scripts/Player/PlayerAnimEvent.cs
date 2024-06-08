using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvent : MonoBehaviour
{
    PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }
    //void StartAttack()
    //{
    //    //playerMovement.StartAttack();
    //}
    void EndCheckAttack()
    {
        playerMovement.EndCheckAttack();
    }

    void CheckAttack()
    {
        playerMovement.CheckAttack();
    }

    //void EndAttack()
    //{
    //    playerMovement.EndAttack();
    //}
}
