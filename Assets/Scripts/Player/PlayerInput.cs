using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public float scroll { get; private set; }
    public bool LockOnPressed { get; private set; }
    public bool DodgePressed { get; private set; }
    public bool AttackPressed { get; private set; }   
    //==========================================================

    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");
        scroll = Input.GetAxis("Mouse ScrollWheel");
        LockOnPressed = Input.GetKeyDown(KeyCode.LeftControl);
        DodgePressed = Input.GetKeyDown(KeyCode.Space);
        AttackPressed = Input.GetMouseButtonDown(0);
    }
}
