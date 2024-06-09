using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public float Scroll { get; private set; }
    public bool LockOnPressed { get; private set; }
    public bool DodgePressed { get; private set; }
    public bool AttackPressed { get; private set; }

    private Dictionary<KeyCode, bool> keyStates = new Dictionary<KeyCode, bool>();

    //==========================================================

    private void Start()
    {
        // 초기 키 상태 설정
        keyStates[KeyCode.LeftControl] = false;
        keyStates[KeyCode.Space] = false;
        keyStates[KeyCode.D] = false;
    }

    private void Update()
    {
        InputKey();
        UpdateKeyStates();
    }

    void InputKey()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");
        Scroll = Input.GetAxis("Mouse ScrollWheel");

        // 각 키의 상태 업데이트
        LockOnPressed = GetKeyDown(KeyCode.LeftControl);
        DodgePressed = GetKeyDown(KeyCode.Space);
        AttackPressed = GetKeyDown(KeyCode.D);
    }

    void UpdateKeyStates()
    {
        foreach (var key in keyStates.Keys.ToList())
        {
            if (Input.GetKeyDown(key))
            {
                keyStates[key] = true;
            }
            else if (Input.GetKeyUp(key))
            {
                keyStates[key] = false;
            }
        }
    }

    bool GetKeyDown(KeyCode key)
    {
        // 키가 방금 눌렸는지 확인
        if (Input.GetKeyDown(key))
        {
            keyStates[key] = true;
            return true;
        }

        // 키가 이미 눌린 상태인지 확인
        if (keyStates.ContainsKey(key) && keyStates[key])
        {
            return true;
        }

        return false;
    }
}
