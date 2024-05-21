using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{

    public void HpBarValue(float MaxVal, float CurVal)
    {
        UIManager.Instance.HealthBarValue(MaxVal, CurVal);
    }

    public void StaminaBarValue(float MaxVal, float CurVal)
    {
        UIManager.Instance.StaminaBarValue(MaxVal, CurVal);
    }
}
