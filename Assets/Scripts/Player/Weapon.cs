using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.SHAKE_CAMERA, this);
        }
    }

}
