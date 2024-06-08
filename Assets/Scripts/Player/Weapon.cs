using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public TrailRenderer trailRenderer;
    PlayerStats playerstats;

    private void Start()
    {
        playerstats = GetComponentInParent<PlayerStats>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.SHAKE_CAMERA, this);
            other.GetComponentInParent<EnemyStats>().TakeDamage(playerstats.Damage);
            GetComponent<BoxCollider>().enabled = false;
        }
        
    }
}
