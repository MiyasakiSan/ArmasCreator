using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using ArmasCreator.GameMode;
using ArmasCreator.Utilities;
using ArmasCreator.Gameplay;
using UnityEngine.UI;
using ArmasCreator.GameData;
using ArmasCreator.UserData;
using ArmasCreator.UI;

public class CombatRpgManager : NetworkBehaviour 
{
    [SerializeField]
    private PlayerRpgMovement playerMovement;

    [Header("EMT")]

    [SerializeField]
    private Image EMT_Gauge;
    [SerializeField]
    private Image thunderFill;
    [SerializeField]
    private BulletBarController bulletBarController;

    private float EMT_MaxAmount;

    public float EMT_Amount;

    private bool isEMTState;
    public bool IsEMTState => isEMTState;

    private Coroutine decreaseEMTgaugeCoroutine;

    public bool canBattle;
    public Weapon heldWeapon;
    public gameState currentGameState;
    [SerializeField]
    private KeyCode gameStateSwitchButton;
    [SerializeField]
    private MovementAnim animController;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private WeaponShealthDraw weaponController;

    [Header("Melee Combo")]

    [SerializeField]
    private float cooldownTime = 2f;

    [SerializeField]
    private float nextFireTime = 0f;

    [SerializeField]
    private int noOfClicks = 0;

    [SerializeField]
    private float lastClickedTime = 0;

    [SerializeField]
    private float maxComboDelay = 0.5f;

    public bool isSheathing;
    public bool isWithdrawing;
    public bool isUsingItem;
    public bool isShooting;
    public bool onCombo;

    private bool firstSound;
    private bool secondSound;
    private bool thirdSound;

    private bool canSwitchStance;

    private Coroutine useItemCoroutine;
    private Coroutine bulletCooldownCoroutine;
    public int BulletAmount;

    public enum gameState
    {
        neutral,combat
    }

    private GameModeController gameModeController;
    private GameplayController gameplayController;
    private GameDataManager gameDataManager;
    private UserDataManager userDataManager;
    private SoundManager soundManager;

    private bool isSinglePlayer => gameModeController.IsSinglePlayerMode;

    private void Awake()
    {
        gameModeController = SharedContext.Instance.Get<GameModeController>();
        gameDataManager = SharedContext.Instance.Get<GameDataManager>();
        userDataManager = SharedContext.Instance.Get<UserDataManager>();
        soundManager = SharedContext.Instance.Get<SoundManager>();
    }

    void Start()
    {
        if (IsLocalPlayer || isSinglePlayer)
        {
            changeGameState(gameState.neutral);
        }

        if(EMT_Gauge != null)
        {
            EMT_MaxAmount = 20; //TODO : Check is we need to make a gameData for this
            EMT_Gauge.fillAmount = 0;
            thunderFill.fillAmount = 0;
        }

        gameplayController = SharedContext.Instance.Get<GameplayController>();

        gameplayController.OnPlayerDealDamage += IncreaseEMTgauge;
        canSwitchStance = true;
        BulletAmount = 3;
        for(int bulletCount = 0;bulletCount < BulletAmount;bulletCount++)
        {
            bulletBarController.OnReload();
        }
    }
    void Update()
    {
        if (IsLocalPlayer || isSinglePlayer)
        {
            if (!canBattle) { return; }

            gameStateCheck();
            if(currentGameState != gameState.combat) { return; }

            CombatDependOnHeldWeapon(heldWeapon);

            if (Input.GetKeyDown(KeyCode.F))
            {
                UseEMT();
            }
        }
    }
    public void changeGameState(gameState value)
    {
        currentGameState = value;
    }
    private void gameStateCheck()
    {
        if (!canSwitchStance) { return; }

        if (!Input.GetKeyUp(gameStateSwitchButton)) { return ; }

        if (!(animController.currentAnimatorCombatStateInfoIsName("Idle") || animController.currentAnimatorCombatStateInfoIsName("Walk"))) { return; }

        if (currentGameState == gameState.neutral && !isSheathing && !isWithdrawing && !animController.playerAnim.GetBool("isCombat"))
        {
            changeGameState(gameState.combat);
            ChangeanimLayer(gameState.combat);
            isWithdrawing = true;
        }
        else if (currentGameState == gameState.combat && !isSheathing && !isWithdrawing && animController.playerAnim.GetBool("isCombat"))
        {
            isSheathing = true;
            changeGameState(gameState.neutral);
            ChangeanimLayer(gameState.neutral);
        }
    }
    private void ChangeanimLayer(gameState current_gameState)
    {
        Debug.Log($"Current : {currentGameState}");
        switch (current_gameState)
        {
            case gameState.neutral:

                if (isSinglePlayer)
                {
                    animController.changeCombatLayerWeight(0f);
                }
                else
                {
                    animController.changeCombatLayerWeightServerRpc(0f);
                }

                break;
            case gameState.combat:

                if (isSinglePlayer)
                {
                    animController.changeCombatLayerWeight(1f);
                }
                else
                {
                    animController.changeCombatLayerWeightServerRpc(1f);
                }

                break;
        }
    }

    private void CombatDependOnHeldWeapon(Weapon currentWeapon)
    {
        switch (currentWeapon)
        {
            case LongSword:
                MeleeCombo();
                break;
        }
    }

    public void UseItem(string itemId)
    {
        if (useItemCoroutine != null) { return; }

        useItemCoroutine = StartCoroutine(UseItemCoroutine(itemId));
    }

    IEnumerator UseItemCoroutine(string itemId)
    {
        canSwitchStance = false;
        isUsingItem = true;
        ResetAnimBoolean();
        playerMovement.canRun = false;

        playerMovement.SetSpeedMultiplierOnUsingItem();

        if (currentGameState == gameState.combat && !isSheathing && !isWithdrawing && animController.playerAnim.GetBool("isCombat"))
        {
            isSheathing = true;
            changeGameState(gameState.neutral);
            ChangeanimLayer(gameState.neutral);

            yield return new WaitForSeconds(1f);
        }

        animController.StartUseItemAnimation(itemId);

        yield return new WaitForSeconds(3.1f);

        animController.EndUseItemAnimation();
        playerMovement.canRun = true;
        playerMovement.ResetSpeedMultiplierOnUsingItem();

        canSwitchStance = true;
        isUsingItem = false;
        useItemCoroutine = null;
        UseItemByInfo(itemId);
        gameplayController.UpdatePlayerItemUsed(1);
    }

    public void CancelUseItem()
    {
        animController.EndUseItemAnimation();

        if (useItemCoroutine != null)
        {
            StopCoroutine(useItemCoroutine);
            playerMovement.ResetSpeedMultiplierOnUsingItem();

            weaponController.cancelShealth();

            useItemCoroutine = null;
            playerMovement.canRun = true;
            canSwitchStance = true;
            isUsingItem = false;
        }
    }

    private void UseItemByInfo(string itemId)
    {
        var exist = gameDataManager.TryGetConsumeItemInfo(itemId, out ConsumeableItemModel consumeItemInfo);

        if (!exist)
        {
            Debug.LogError("=============  Can't use item  ================");
            return;
        }

        PlayerStat playerStat = GetComponent<PlayerStat>();

        if (consumeItemInfo.SubType == SubType.Health)
        {
            playerStat.Heal(consumeItemInfo.ConsumePercent);
        }
        else
        {
            playerStat.IncreaseStaminaRegenRate(1 + consumeItemInfo.ConsumePercent / 100);
        }

        userDataManager.UserData.UserDataInventory.RemoveConsumeItem(itemId, 1);
    }

    #region EMT
    private void IncreaseEMTgauge()
    {
        if (isEMTState) { return; }

        if (convertEMTAmountToEMTPercent(EMT_Amount) == EMT_MaxAmount) { return; }

        EMT_Amount++;
        EMT_Gauge.fillAmount = convertEMTAmountToEMTPercent(EMT_Amount);
    }

    private void UseEMT()
    {
        Debug.Log("Use EMT");

        if (isEMTState && decreaseEMTgaugeCoroutine != null)
        {
            StopCoroutine(decreaseEMTgaugeCoroutine);

            decreaseEMTgaugeCoroutine = null;
            isEMTState = false;
        }
        else if (!isEMTState && EMT_Amount >= EMT_MaxAmount)
        {
            isEMTState = true;

            thunderFill.fillAmount = 1;

            decreaseEMTgaugeCoroutine = StartCoroutine(DecreaseEMTgauge(2));
        }
    }

    private IEnumerator DecreaseEMTgauge(float decreaseRate)
    {
        while (EMT_Amount > 0)
        {
            EMT_Amount -= 0.1f;
            EMT_Gauge.fillAmount = convertEMTAmountToEMTPercent(EMT_Amount);
            thunderFill.fillAmount = convertEMTAmountToEMTPercent(EMT_Amount);

            yield return new WaitForSeconds(0.1f / decreaseRate);
        }

        EMT_Amount = 0;
        EMT_Gauge.fillAmount = 0;
        thunderFill.fillAmount = 0;
        isEMTState = false;
    } 

    private float convertEMTAmountToEMTPercent(float EMT_Amount)
    {
        return EMT_Amount / EMT_MaxAmount;
    }
    #endregion

    #region Melee Combat
    public void MeleeCombo()
    {
        Melee_CurrentAnimOutOfTime();
        Melee_IsComboOutOfTime();
        if (Input.GetMouseButtonDown(0))
        {
            MeleeCombo_OnClick();
        }
        else if (Input.GetMouseButtonDown(1) && BulletAmount > 0 && !isShooting)
        {
            Shoot();
            BulletAmount--;
            bulletBarController.OnShoot();
            isShooting = true;
        }
    }

    public void Shoot()
    {
        animController.ShootAnimation();

        var spike = Instantiate(projectilePrefab, weaponController.currentWeaponInHand.transform.position, this.transform.rotation);

        if (bulletCooldownCoroutine != null)
        {
            StopCoroutine(bulletCooldownCoroutine);
            bulletCooldownCoroutine = null;
        }

        bulletCooldownCoroutine = StartCoroutine(bulletCooldown());
    }

    IEnumerator bulletCooldown()
    {
        yield return new WaitForSeconds(2.5f);

        while(BulletAmount < 3)
        {
            yield return new WaitForSeconds(1f);
            BulletAmount++;
            bulletBarController.OnReload();
            soundManager.PlayOneShot(soundManager.fModEvent.PlayerGunReloadSFX, gameObject);
        }

        BulletAmount = 3;
        bulletCooldownCoroutine = null;
    }

    public void EndShoot()
    {
        //animController.EndShootAnimation();
    }

    public void Melee_CurrentAnimOutOfTime()
    {
        if (animController.currentAnimatorCombatStateInfoIsName("Idle"))
        {
            ResetAnimBoolean();
        }

        if (animController.currentAnimatorStateInfoTime <= 0.9f) { return; }

        else if (animController.currentAnimatorCombatStateInfoIsName($"{heldWeapon.comboParam}NormalAttack1"))
        {
            if (isSinglePlayer)
            {
                animController.MeleeSetBool($"{heldWeapon.comboParam}Normal_hit1", false);
                firstSound = false;
            }
            else
            {
                animController.MeleeSetBoolServerRpc($"{heldWeapon.comboParam}Normal_hit1", false);
            }
        }
        else if(animController.currentAnimatorCombatStateInfoIsName($"{heldWeapon.comboParam}NormalAttack2"))
        {
            if (isSinglePlayer)
            {
                animController.MeleeSetBool($"{heldWeapon.comboParam}Normal_hit2", false);
            }
            else
            {
                animController.MeleeSetBoolServerRpc($"{heldWeapon.comboParam}Normal_hit2", false);
            }
        }
        else if(animController.currentAnimatorCombatStateInfoIsName($"{heldWeapon.comboParam}NormalAttack3"))
        {
            if (isSinglePlayer)
            {
                animController.MeleeSetBool($"{heldWeapon.comboParam}Normal_hit3", false);
            }
            else
            {
                animController.MeleeSetBoolServerRpc($"{heldWeapon.comboParam}Normal_hit3", false);
            }

            noOfClicks = 0;
        }
    }

    public void ResetAnimBoolean()
    {
        playerMovement.ResetRotate();
        playerMovement.canWalk = true;
        playerMovement.canRun = true;

        if (isSinglePlayer)
        {
            noOfClicks = 0;
        }
        else
        {
            noOfClicks = 0;

            if (animController.IsOverideCompleted()) { return; }

            animController.CombatOverideServerRpc(0);
        }
    }

    public void ResetCombatBool()
    {
        isSheathing = false;
        isWithdrawing = false;
        isUsingItem = false;
    }

    public void Melee_IsComboOutOfTime()
    {
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }
        if (Time.time <= nextFireTime) { return; }
    }
    public void MeleeCombo_OnClick()
    {
        lastClickedTime = Time.time;
        noOfClicks++;
        noOfClicks = Mathf.Clamp(noOfClicks, 0, heldWeapon.comboCount);
        setComboBoolDependOn(noOfClicks);
        playerMovement.canWalk = false;
        playerMovement.canRun= false;

        if (noOfClicks > 0)
        {
            playerMovement.StopSFX();
            onCombo = true;
        }
        else
        {
            onCombo = false;
        }
    }

    public void setComboBoolDependOn(int num)
    {
        if (num == 1)
        {
            if (isSinglePlayer)
            {
                animController.MeleeSetBool($"{heldWeapon.comboParam}Normal_hit1", true);

                if (!firstSound)
                {
                    soundManager.PlayOneShot(soundManager.fModEvent.PlayerFirstComboSFX, this.gameObject);
                    firstSound = true;
                    thirdSound = false;
                    secondSound = false;
                }
            }
            else
            {
                animController.MeleeSetBoolServerRpc($"{heldWeapon.comboParam}Normal_hit1", true);
            }
        }

        if (animController.currentAnimatorStateInfoTime <= 0.3f) { return; }

        if (num > 0  && animController.currentAnimatorCombatStateInfoIsName($"{heldWeapon.comboParam}NormalAttack1"))
        {
            if (isSinglePlayer)
            {
                animController.MeleeSetBool($"{heldWeapon.comboParam}Normal_hit2", true);

                if (!secondSound) 
                {
                    soundManager.PlayOneShot(soundManager.fModEvent.PlayerSecondComboSFX, this.gameObject);
                    secondSound = true;
                    firstSound = false;
                }
            }
            else
            {
                animController.MeleeSetBoolServerRpc($"{heldWeapon.comboParam}Normal_hit2", true);
            }
        }

        if (animController.currentAnimatorStateInfoTime <= 0.3f) { return; }

        if (num > 1 && animController.currentAnimatorCombatStateInfoIsName($"{heldWeapon.comboParam}NormalAttack2"))
        {
            if (isSinglePlayer)
            {
                animController.MeleeSetBool($"{heldWeapon.comboParam}Normal_hit3", true);

                if (!thirdSound) 
                {
                    soundManager.PlayOneShot(soundManager.fModEvent.PlayerThirdComboSFX, this.gameObject);
                    thirdSound = true;
                    secondSound = false;
                }
            }
            else
            {
                animController.MeleeSetBoolServerRpc($"{heldWeapon.comboParam}Normal_hit3", true);
            }
        }
    } 
    public void dieState()
    {
        changeGameState(CombatRpgManager.gameState.neutral);
        canBattle = false;
    }
    public void respawnState()
    {
        canBattle = true;
    }
    #endregion

    private void OnDestroy()
    {
        gameplayController.OnPlayerDealDamage -= IncreaseEMTgauge;
    }
}
