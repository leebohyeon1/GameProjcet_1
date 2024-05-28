using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = transform.parent.GetComponent<PlayerMovement>();
    }

    void EndAttack()
    {
        playerMovement.EndAttack();
    }
}
