using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    Slider hpSlider;
    //==========================================================

    void Start()
    {
        if(hpSlider == null) {  hpSlider = GetComponent<Slider>(); }
    }
    //==========================================================

    public void HpValue(float MaxVal, float CurVal)
    {
        hpSlider.value = CurVal / MaxVal;
    }
}
