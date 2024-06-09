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
        // �ʱ� Ű ���� ����
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

        // �� Ű�� ���� ������Ʈ
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
        // Ű�� ��� ���ȴ��� Ȯ��
        if (Input.GetKeyDown(key))
        {
            keyStates[key] = true;
            return true;
        }

        // Ű�� �̹� ���� �������� Ȯ��
        if (keyStates.ContainsKey(key) && keyStates[key])
        {
            return true;
        }

        return false;
    }
}
