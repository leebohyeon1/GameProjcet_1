using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockOnUI : MonoBehaviour
{
    private Image lockOnIndicator;
    private Transform currentTarget;
    private Camera playerCamera;
    //==========================================================

    private void Start()
    {
        if (lockOnIndicator == null) { lockOnIndicator = transform.GetComponent<Image>(); }
        if (playerCamera == null) { playerCamera = Camera.main; }   
        lockOnIndicator.enabled = false;
    }

    //==========================================================

    public void Show(Transform target)
    {
        lockOnIndicator.enabled = true;
        currentTarget = target;
        UpdatePosition(target);
    }

    public void Hide()
    {
        lockOnIndicator.enabled = false;
        currentTarget = null;
    }

    public void UpdatePosition(Transform target)
    {
        if (lockOnIndicator.enabled && currentTarget != null)
        {
            Vector3 screenPosition = playerCamera.WorldToScreenPoint(target.position);
            lockOnIndicator.transform.position = screenPosition;
        }
    }
}
