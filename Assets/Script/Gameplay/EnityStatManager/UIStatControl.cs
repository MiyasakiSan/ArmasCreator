using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class UIStatControl : MonoBehaviour
{
    public Slider healthSlider;
    public Slider staminaSlider;
    void Start()
    {
        
    }

    public void SetHealthUI(float value)
    {
        healthSlider.maxValue = value;
        healthSlider.value = value;
    }
    public void SetStaminaUI(float value)
    {
        staminaSlider.maxValue = value;
        staminaSlider.value = value;
    }
    public void UpdateHealthUI(float value)
    {
        healthSlider.value = value;
    }
    public void UpdateStaminaUI(float value)
    {
        staminaSlider.value = value;
    }

}
