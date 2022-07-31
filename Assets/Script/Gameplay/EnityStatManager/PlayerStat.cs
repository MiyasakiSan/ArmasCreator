using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStat : AttackTarget,IDamagable<float>,IStaminaUsable<float>
{
    public float maxHealth;
    public float maxStamina;
    [SerializeField]
    NetworkVariable<float> NetworkcurrentHealth = new NetworkVariable<float>();
    [SerializeField]
    NetworkVariable<float> NetworkcurrentStamina = new NetworkVariable<float>();
    [SerializeField]
    public UIStatControl UIstat;
    public GameObject Canvas;

    private  Coroutine staminaRegen;
    private  Coroutine staminaReduceOverTime;
    [SerializeField]
    private bool IsReduceStaminaRunning;

    private bool setParam = false;
    [SerializeField]
    private PlayerRpgMovement playerMovement;

    public float currentHealth
    {
        get { return NetworkcurrentHealth.Value; }
        set { NetworkcurrentHealth.Value = value; }
    }
    public float currentStamina
    {
        get { return NetworkcurrentStamina.Value; }
        set { NetworkcurrentStamina.Value = value; }
    }
    [ServerRpc(RequireOwnership = false)]
    public void currentHealthServerRpc(float value)
    {
        currentHealth = value;
    }
    [ServerRpc(RequireOwnership = false)]
    public void currentStaminaServerRpc(float value)
    {
        currentStamina = value;
    }
    public void reduceStamina(float amount)
    {
        currentStaminaServerRpc(currentStamina - amount);
        if(staminaRegen != null)
        {
            StopCoroutine(staminaRegen);
        }
        staminaRegen = StartCoroutine(RegenStamina());
    }
    public void reduceStaminaAmountOverTime(float amount)
    {
        if (!IsReduceStaminaRunning)
        {
            staminaReduceOverTime = StartCoroutine(ReduceStaminaOverTime(amount));
        }
        if (staminaRegen != null)
        {
            StopCoroutine(staminaRegen);
            staminaRegen = null;
        }

    }
    public void stopReduceStamina()
    {
        if(currentStamina >= 0)
        {
            StopCoroutine(staminaReduceOverTime);
        }
        staminaReduceOverTime = null;
        IsReduceStaminaRunning = false;
        if (staminaRegen != null)
        {
            StopCoroutine(staminaRegen);
        }
        staminaRegen = StartCoroutine(RegenStamina());
        Debug.Log("Stop Run");
    }
  
    public override void receiveAttack(float damage)
    {
        if (!IsLocalPlayer) { return; }
        if (this.GetComponent<PlayerRpgMovement>().isDodging) { Debug.Log("Dodge"); return; }
        currentHealthServerRpc(currentHealth - damage);
        if (currentHealth <= 0) { playerMovement.playerDie(); return; }
    }
    public IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(2f);
        while(currentStamina < maxStamina)
        {
            currentStaminaServerRpc(currentStamina + maxStamina / 150);
            yield return new WaitForSeconds(0.1f);
        }
        staminaRegen = null;
    }
    public IEnumerator ReduceStaminaOverTime(float amount)
    {
        IsReduceStaminaRunning = true;
        while (currentStamina >= 0)
        {
            currentStaminaServerRpc(currentStamina - (maxStamina / 100)*amount);
            yield return new WaitForSeconds(0.1f);
        }
        IsReduceStaminaRunning = false;
    }
    public void respawnResetHealth()
    {
        UIstat.SetHealthUI(maxHealth);
        currentHealthServerRpc(maxHealth);
    }
    private void HealthChange(float previousValue, float newValue)
    {
        UIstat.UpdateHealthUI(newValue);
    }

    private void StaminaChange(float previousValue, float newValue)
    {
        UIstat.UpdateStaminaUI(newValue);
    }
    private void setupVariable()
    {
        currentHealthServerRpc(maxHealth);
        currentStaminaServerRpc(maxStamina);
        NetworkcurrentStamina.OnValueChanged += StaminaChange;
        NetworkcurrentHealth.OnValueChanged += HealthChange;
        UIstat.SetHealthUI(maxHealth);
        UIstat.SetStaminaUI(maxStamina);
    }
    private void setupClientCanvas()
    {
        Canvas = GameObject.FindGameObjectWithTag("OtherBar");
        UIstat.transform.SetParent(Canvas.transform);
        UIstat.tag = "OtherPlayerBar";
        UIstat.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.8f);
    }
    void Start()
    {
        setupVariable();
        IsReduceStaminaRunning = false;
        setParam = true;
        if (!IsLocalPlayer && Canvas == null)
        {
            setupClientCanvas();
        }
    }





    private void SceneManager_sceneUnloaded(Scene arg0)
    {
        //if (IsLocalPlayer)
        //{
        //    onStaminaUpDate += upDateStaminaUI;
        //    onHealthUpDate += upDateHealthUI;
        //    currentHealthServerRpc(maxHealth);
        //    currentStaminaServerRpc(maxStamina);
        //    IsReduceStaminaRunning = false;
        //    UIstat.SetHealthUI(maxHealth);
        //    UIstat.SetStaminaUI(maxStamina);
        //    setParam = true;
        //    SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        //}
        NetworkcurrentStamina.OnValueChanged += StaminaChange;
        NetworkcurrentHealth.OnValueChanged += HealthChange;
        UIstat.SetHealthUI(maxHealth);
        UIstat.SetStaminaUI(maxStamina);
        if (!IsLocalPlayer)
        {
            GameObject Canvas = GameObject.FindGameObjectWithTag("OtherBar");
            UIstat.transform.SetParent(Canvas.transform);
            UIstat.tag = "OtherPlayerBar";
            UIstat.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.8f);
        }
    }
   


    private void Update()
    {
        if (!IsLocalPlayer && Canvas == null)
        {
            NetworkcurrentStamina.OnValueChanged += StaminaChange;
            NetworkcurrentHealth.OnValueChanged += HealthChange;
            UIstat.SetHealthUI(maxHealth);
            UIstat.SetStaminaUI(maxStamina);
            Canvas = GameObject.FindGameObjectWithTag("OtherBar");
            UIstat.transform.SetParent(Canvas.transform);
            UIstat.tag = "OtherPlayerBar";
            UIstat.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.8f);
        }
    }

  
}
