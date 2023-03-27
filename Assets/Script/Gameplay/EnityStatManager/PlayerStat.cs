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
using ArmasCreator.UserData;
using ArmasCreator.GameData;
using ArmasCreator.Gameplay;

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

    private Coroutine staminaRegen;
    private Coroutine staminaReduceOverTime;
    private Coroutine staminaCooldown;
    private Coroutine staminaRateIncrease;

    private float staminaRegenRate = 1f;

    [SerializeField]
    private bool IsReduceStaminaRunning;

    private bool setParam = false;
    [SerializeField]
    private PlayerRpgMovement playerMovement;

    private GameModeController gameModeController;

    private bool isValnurable;

    private bool isSinglePlayer => gameModeController.IsSinglePlayerMode;

    private UserDataManager userDataManager;
    private GameDataManager gameDataManager;
    private GameplayController gameplayController;

    [Space(10)]
    [Header("Equipment")]
    [SerializeField]
    private GameObject CiladaeHelmet;
    [SerializeField]
    private GameObject CiladaeShirt;
    [SerializeField]
    private GameObject CiladaeGlove;
    [SerializeField]
    private GameObject CiladaeLegging;
    [SerializeField]
    private GameObject CiladaeShoes;
    [SerializeField]
    private GameObject CiladaeWeapon;
    [Space(10)]
    [SerializeField]
    private GameObject GotenaHelmet;
    [SerializeField]
    private GameObject GotenaShirt;
    [SerializeField]
    private GameObject GotenaGlove;
    [SerializeField]
    private GameObject GotenaLegging;
    [SerializeField]
    private GameObject GotenaShoes;
    [SerializeField]
    private GameObject GotenaWeapon;

    private float def = 0;
    public float Def => def;
    private float vit = 0;
    public float Vit => vit;
    private float atk = 0;
    public float Atk => atk;

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
        if(staminaReduceOverTime != null)
        {
            StopCoroutine(staminaReduceOverTime);
            staminaReduceOverTime = null;
        }

        IsReduceStaminaRunning = false;

        if (staminaRegen != null && staminaCooldown == null)
        {
            StopCoroutine(staminaRegen);
            staminaRegen = StartCoroutine(RegenStamina());
        }
        else
        {
            staminaRegen = StartCoroutine(RegenStamina());
        }
    }

    public void Heal(float percent)
    {
        if(currentHealth + (percent/100)*maxHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += (percent / 100) * maxHealth;
        }

        uiStat.UpdateHealthUI(currentHealth);
    }
  
    public override void receiveAttack(float damage)
    {
        if (isSinglePlayer)
        {
            if (this.GetComponent<PlayerRpgMovement>().isDodging) { Debug.Log("Dodge"); return; }

            if (this.GetComponent<PlayerRpgMovement>().isKnockBack) { return; }

            if (isValnurable) { return; }

            damage -= (def * damage) / 100;
            currentHealth -= damage;
            uiStat.UpdateHealthUI(currentHealth);

            StartCoroutine(ValnurableStage());

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

    IEnumerator ValnurableStage()
    {
        isValnurable = true;

        yield return new WaitForSeconds(0.25f);

        isValnurable = false;
    }

    public void IncreaseStaminaRegenRate(float rate)
    {
        uiStat.IncreaseStaminaRateState();

        if (staminaRateIncrease != null)
        {
            StopCoroutine(staminaRateIncrease);
            staminaRateIncrease = null;
        }

        staminaRateIncrease = StartCoroutine(IncreaseStaminaRegenRateCoroutine(rate));
    }

    IEnumerator IncreaseStaminaRegenRateCoroutine(float rate)
    {
        staminaRegenRate = rate;
        yield return new WaitForSeconds(30f);
        staminaRegenRate = 1f;
        uiStat.ResetIncreaseStaminaRateState();
    }

    public IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(2f);
        while(CurrentStamina < maxStamina)
        {
            if (isSinglePlayer)
            {
                currentStamina += (maxStamina / 100)*staminaRegenRate;
                uiStat.UpdateStaminaUI(currentStamina);
            }
            else
            {
                currentStaminaServerRpc(CurrentStamina + maxStamina / 150);
            }

            yield return new WaitForSeconds(0.05f);
        }

        currentStamina = maxStamina;
        staminaRegen = null;
    }

    public IEnumerator ReduceStaminaOverTime(float amount)
    {
        IsReduceStaminaRunning = true;
        while (CurrentStamina > 0)
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

            yield return new WaitForSeconds(0.05f);
        }

        IsReduceStaminaRunning = false;
        currentStamina = 0;

        staminaCooldown = StartCoroutine(StaminaCoolDown());
        staminaReduceOverTime = null;
    }

    public IEnumerator StaminaCoolDown()
    {
        playerMovement.canRun = false;
        staminaRegen = StartCoroutine(RegenStamina());

        yield return new WaitUntil(() => currentStamina == maxStamina);

        playerMovement.canRun = true;
        staminaCooldown = null;
    }

    public void respawnResetHealth()
    {
        uiStat.SetHealthUI(maxHealth);

        if (isSinglePlayer)
        {
            CurrentHealth = maxHealth;
            uiStat.SetHealthUI(maxHealth + vit);
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
            CurrentHealth = maxHealth + vit;
            CurrentStamina = maxStamina;

            uiStat.SetHealthUI(maxHealth + vit);
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

    private void Awake()
    {
        gameModeController = SharedContext.Instance.Get<GameModeController>();
        userDataManager = SharedContext.Instance.Get<UserDataManager>();
        gameDataManager = SharedContext.Instance.Get<GameDataManager>();
    }

    void Start()
    {
        gameplayController = SharedContext.Instance.Get<GameplayController>();

        if (gameplayController.CurrentGameplays == GameplayController.Gameplays.Normal ||
            gameplayController.CurrentGameplays == GameplayController.Gameplays.Town)
        {
            SetupStat();
            SetupEquipmentAsset();
        }

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

    private void SetupStat()
    {
        atk = 0;
        def = 0;
        vit = 0;

        var allEquipmentId = userDataManager.UserData.UserDataInventory.GetAllEquipItemIds();

        foreach(var id in allEquipmentId)
        {
            bool exist = gameDataManager.TryGetEquipItemInfoById(id, out EquipableItemModel equipItemInfo);

            if (!exist) { return; }

            atk += equipItemInfo.ATK;
            def += equipItemInfo.DEF;
            vit += equipItemInfo.VIT;
        }
    }

    #region HardCode
    private void SetupEquipmentAsset()
    {
        var allEquipmentId = userDataManager.UserData.UserDataInventory.GetAllEquipItemIds();

        foreach (var id in allEquipmentId)
        {
            var subType = gameDataManager.GetEquipInfoSubType(id, out string asset);

            if (asset != "ciladae" || asset != "gotenna") { continue; }

            switch (subType)
            {
                case (SubType.Helmet):
                    {
                        if (asset == "ciladae")
                        {
                            CiladaeHelmet.SetActive(true);
                        }
                        else
                        {
                            GotenaHelmet.SetActive(true);
                        }
                        break;
                    }
                case (SubType.Shirt):
                    {
                        if (asset == "ciladae")
                        {
                            CiladaeShirt.SetActive(true);
                        }
                        else
                        {
                            GotenaShirt.SetActive(true);
                        }
                        break;
                    }
                case (SubType.Glove):
                    {
                        if (asset == "ciladae")
                        {
                            CiladaeGlove.SetActive(true);
                        }
                        else
                        {
                            GotenaGlove.SetActive(true);
                        }
                        break;
                    }
                case (SubType.Pant):
                    {
                        if (asset == "ciladae")
                        {
                            CiladaeLegging.SetActive(true);
                        }
                        else
                        {
                            GotenaLegging.SetActive(true);
                        }
                        break;
                    }
                case (SubType.Shoes):
                    {
                        if (asset == "ciladae")
                        {
                            CiladaeShoes.SetActive(true);
                        }
                        else
                        {
                            GotenaShoes.SetActive(true);
                        }
                        break;
                    }
                case (SubType.Weapon):
                    {
                        if (asset == "ciladae")
                        {
                            CiladaeWeapon.SetActive(true);
                        }
                        else
                        {
                            GotenaWeapon.SetActive(true);
                        }
                        break;
                    }
            }
        } 
        #endregion
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
