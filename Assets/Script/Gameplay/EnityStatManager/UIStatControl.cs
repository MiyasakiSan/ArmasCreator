using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArmasCreator.Utilities;
using Unity.Netcode;

public class UIStatControl : MonoBehaviour
{
    [SerializeField]
    private Image staminaBarImage;
    [SerializeField]
    private Image healthBarImage;

    private Color defaultStaminaColor;

    private float maxHP;
    private float maxStamina;

    private void Awake()
    {
        
    }
    void Start()
    {
        defaultStaminaColor = staminaBarImage.color;

    }

    public void SetHealthUI(float value)
    {
        maxHP = value;
        healthBarImage.fillAmount = value / maxHP;
    }
    public void SetStaminaUI(float value)
    {
        maxStamina = value;
        staminaBarImage.fillAmount = value / maxStamina;
    }
    public void UpdateHealthUI(float value)
    {
        healthBarImage.fillAmount = value / maxHP;
    }
    public void UpdateStaminaUI(float value)
    {
        staminaBarImage.fillAmount = value / maxStamina;
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
