using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarUI : MonoBehaviour
{
    Slider staminaBarSlider;
    //=========================================================

    void Start()
    {
        if(staminaBarSlider == null) { staminaBarSlider = GetComponent<Slider>(); }      
    }
    //=========================================================

    public void StaminaValue(float MaxVal, float CurVal)
    {
        staminaBarSlider.value = CurVal / MaxVal;
    }
}
