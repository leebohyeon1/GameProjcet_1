using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour, IListener
{
    public Transform rightHandIKTarget;
    public Transform leftHandIKTarget;
    public GameObject weaponPrefab;

    private GameObject equippedWeapon;
    private IKControl ikControl;

    void Start()
    {
        ikControl = GetComponent<IKControl>();
        EquipWeapon(weaponPrefab);  

        EventManager.Instance.AddListener(EVENT_TYPE.CHECK_ATTACK,this);
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        equippedWeapon.GetComponent<BoxCollider>().enabled = (bool)Param;
        equippedWeapon.GetComponent<Weapon>().trailRenderer.enabled = (bool)Param;
    }

    public void EquipWeapon(GameObject weapon)
    {
        if (equippedWeapon != null)
        {
            Destroy(equippedWeapon);
        }

        equippedWeapon = Instantiate(weapon, rightHandIKTarget.position, rightHandIKTarget.rotation, rightHandIKTarget);
        equippedWeapon.transform.localPosition = Vector3.zero;
        equippedWeapon.transform.localRotation = Quaternion.identity;

        ikControl.rightHandObj = rightHandIKTarget;
        //ikControl.leftHandObj = leftHandIKTarget;
        ikControl.ikActive = true;
    }



}
