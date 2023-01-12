using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArmasCreator.Utilities;
using Unity.Netcode;

public class UIStatControl : MonoBehaviour
{
    public Slider healthSlider;
    public Slider staminaSlider;

    [SerializeField]
    private Image staminaBarImage;

    private Color defaultStaminaColor;

    private void Awake()
    {
        
    }
    void Start()
    {
        defaultStaminaColor = staminaBarImage.color;
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

    public void IncreaseStaminaRateState()
    {
        staminaBarImage.color = new Color(0, 129, 171);
    }

    public void ResetIncreaseStaminaRateState()
    {
        staminaBarImage.color = defaultStaminaColor;
    }

}
