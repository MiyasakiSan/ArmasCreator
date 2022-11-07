using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ArmasCreator.GameMode;
using ArmasCreator.Utilities;
using ArmasCreator.Gameplay.UI;

public class PlayerStat : AttackTarget,IDamagable<float>,IStaminaUsable<float>
{
    public float maxHealth;
    public float maxStamina;

    //TODO : change public to private
    public float currentHealth;
    public float currentStamina;

    [SerializeField]
    NetworkVariable<float> NetworkcurrentHealth = new NetworkVariable<float>();

    [SerializeField]
    NetworkVariable<float> NetworkcurrentStamina = new NetworkVariable<float>();

    private UIStatControl uiStat;
    private UIPlayerController uiPlayerController;
    private GameObject otherPlayerCanvas;

    private  Coroutine staminaRegen;
    private  Coroutine staminaReduceOverTime;
    [SerializeField]
    private bool IsReduceStaminaRunning;

    private bool setParam = false;
    [SerializeField]
    private PlayerRpgMovement playerMovement;

    private GameModeController gameModeController;

    private bool isSinglePlayer => gameModeController.IsSinglePlayerMode;

    public float CurrentHealth
    {
        get 
        {
            if (isSinglePlayer)
            {
                return currentHealth;
            }
            else
            {
                return NetworkcurrentHealth.Value;
            }
        }
        set 
        {
            if (isSinglePlayer)
            {
                currentHealth = value;
            }
            else
            {
                NetworkcurrentHealth.Value = value;
            }
        }
    }
    public float CurrentStamina
    {
        get
        {
            if (isSinglePlayer)
            {
                return currentStamina;
            }
            else
            {
                return NetworkcurrentStamina.Value;
            }
        }
        set
        {
            if (isSinglePlayer)
            {
                currentStamina = value;
            }
            else
            {
                NetworkcurrentStamina.Value = value;
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void currentHealthServerRpc(float value)
    {
        CurrentHealth = value;
    }

    [ServerRpc(RequireOwnership = false)]
    public void currentStaminaServerRpc(float value)
    {
        CurrentStamina = value;
    }

    public void reduceStamina(float amount)
    {
        if (isSinglePlayer)
        {
            currentStamina -= amount;
            uiStat.UpdateStaminaUI(currentStamina);
        }
        else
        {
            currentStaminaServerRpc(CurrentStamina - amount);
        }

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
        if(CurrentStamina >= 0 && staminaReduceOverTime != null)
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
    }
  
    public override void receiveAttack(float damage)
    {
        if (isSinglePlayer)
        {
            if (this.GetComponent<PlayerRpgMovement>().isDodging) { Debug.Log("Dodge"); return; }

            currentHealth -= damage;
            uiStat.UpdateHealthUI(currentHealth);

            if (CurrentHealth <= 0) { playerMovement.PlayerDie(); return; }
        }
        else
        {
            if (!IsLocalPlayer) { return; }

            if (this.GetComponent<PlayerRpgMovement>().isDodging) { Debug.Log("Dodge"); return; }

            currentHealthServerRpc(CurrentHealth - damage);
            if (CurrentHealth <= 0) { playerMovement.PlayerDie(); return; }
        }
    }

    public IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(2f);
        while(CurrentStamina < maxStamina)
        {
            if (isSinglePlayer)
            {
                currentStamina += maxStamina / 150;
                uiStat.UpdateStaminaUI(currentStamina);
            }
            else
            {
                currentStaminaServerRpc(CurrentStamina + maxStamina / 150);
            }

            yield return new WaitForSeconds(0.1f);
        }
        staminaRegen = null;
    }

    public IEnumerator ReduceStaminaOverTime(float amount)
    {
        IsReduceStaminaRunning = true;
        while (CurrentStamina >= 0)
        {
            if (isSinglePlayer)
            {
                currentStamina -= (maxStamina / 100) * amount;
                uiStat.UpdateStaminaUI(currentStamina);
            }
            else
            {
                currentStaminaServerRpc(CurrentStamina - (maxStamina / 100) * amount);
            }

            yield return new WaitForSeconds(0.1f);
        }
        IsReduceStaminaRunning = false;
    }

    public void respawnResetHealth()
    {
        uiStat.SetHealthUI(maxHealth);

        if (isSinglePlayer)
        {
            CurrentHealth = maxHealth;
            uiStat.SetHealthUI(maxHealth);
        }
        else
        {
            currentHealthServerRpc(maxHealth);
        }
    }

    private void HealthChange(float previousValue, float newValue)
    {
        uiStat.UpdateHealthUI(newValue);
    }

    private void StaminaChange(float previousValue, float newValue)
    {
        uiStat.UpdateStaminaUI(newValue);
    }

    public void SetupVariable()
    {
        uiPlayerController = SharedContext.Instance.Get<UIPlayerController>();

        otherPlayerCanvas = uiPlayerController.OtherPlayerCanvas;
        uiStat = uiPlayerController.PlayerStatUI;

        if (isSinglePlayer)
        {
            CurrentHealth = maxHealth;
            CurrentStamina = maxStamina;

            uiStat.SetHealthUI(maxHealth);
            uiStat.SetStaminaUI(maxStamina);
        }
        else
        {
            currentHealthServerRpc(maxHealth);
            currentStaminaServerRpc(maxStamina);
            NetworkcurrentStamina.OnValueChanged += StaminaChange;
            NetworkcurrentHealth.OnValueChanged += HealthChange;
            uiStat.SetHealthUI(maxHealth);
            uiStat.SetStaminaUI(maxStamina);
        }

        IsReduceStaminaRunning = false;
        setParam = true;
    }

    private void setupClientCanvas()
    {
        otherPlayerCanvas = GameObject.FindGameObjectWithTag("OtherBar");
        uiStat.transform.SetParent(otherPlayerCanvas.transform);
        uiStat.tag = "OtherPlayerBar";
        uiStat.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.8f);
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
        uiStat.SetHealthUI(maxHealth);
        uiStat.SetStaminaUI(maxStamina);

        if (!IsLocalPlayer)
        {
            GameObject Canvas = GameObject.FindGameObjectWithTag("OtherBar");
            uiStat.transform.SetParent(Canvas.transform);
            uiStat.tag = "OtherPlayerBar";
            uiStat.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.8f);
        }
    }

    private void Awake()
    {
        gameModeController = SharedContext.Instance.Get<GameModeController>();
    }

    void Start()
    {
        if (isSinglePlayer)
        {
            SetupVariable();
        }

        if (isSinglePlayer)
        {
            return;
        }

        if (!IsLocalPlayer && otherPlayerCanvas == null)
        {
            setupClientCanvas();
        }
    }

    private void Update()
    {
        if (isSinglePlayer)
        {
            return;
        }

        if (!IsLocalPlayer && otherPlayerCanvas == null)
        {
            NetworkcurrentStamina.OnValueChanged += StaminaChange;
            NetworkcurrentHealth.OnValueChanged += HealthChange;
            uiStat.SetHealthUI(maxHealth);
            uiStat.SetStaminaUI(maxStamina);
            otherPlayerCanvas = GameObject.FindGameObjectWithTag("OtherBar");
            uiStat.transform.SetParent(otherPlayerCanvas.transform);
            uiStat.tag = "OtherPlayerBar";
            uiStat.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.8f);
        }
    }

    private void OnDestroy()
    {

    }
}
