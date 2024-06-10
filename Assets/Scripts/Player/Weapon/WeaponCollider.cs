using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour ,IListener
{
    public TrailRenderer trailRenderer;
    PlayerStats playerstats;
    //==========================================================

    private void Start()
    {
        playerstats = GetComponentInParent<PlayerStats>();

        EventManager.Instance.AddListener(EVENT_TYPE.CHECK_ATTACK, this);
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

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        GetComponent<BoxCollider>().enabled = (bool)Param;
        trailRenderer.enabled = (bool)Param;
    }
}
