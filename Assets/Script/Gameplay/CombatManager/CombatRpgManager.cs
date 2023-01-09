using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using ArmasCreator.GameMode;
using ArmasCreator.Utilities;
using ArmasCreator.Gameplay;
using UnityEngine.UI;

public class CombatRpgManager : NetworkBehaviour 
{
    [SerializeField]
    private PlayerRpgMovement playerMovement;

    [Header("EMT")]

    [SerializeField]
    private Slider EMT_Gauge;

    private float EMT_Amount;

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

    public enum gameState
    {
        neutral,combat
    }

    private GameModeController gameModeController;
    private GameplayController gameplayController;

    private bool isSinglePlayer => gameModeController.IsSinglePlayerMode;

    private void Awake()
    {
        gameModeController = SharedContext.Instance.Get<GameModeController>();
    }

    void Start()
    {
        if (IsLocalPlayer || isSinglePlayer)
        {
            changeGameState(gameState.neutral);
        }

        if(EMT_Gauge != null)
        {
            EMT_Gauge.maxValue = 20; //TODO : Check is we need to make a gameData for this
            EMT_Gauge.value = 0;
        }

        gameplayController = SharedContext.Instance.Get<GameplayController>();

        gameplayController.OnPlayerDealDamage += IncreaseEMTgauge;
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

    private void IncreaseEMTgauge()
    {
        if (isEMTState) { return; }

        if (EMT_Amount == EMT_Gauge.maxValue) { return; }

        EMT_Amount++;
        EMT_Gauge.value = EMT_Amount;
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
        else if(!isEMTState && EMT_Amount == 20)
        {
            isEMTState = true;

            decreaseEMTgaugeCoroutine = StartCoroutine(DecreaseEMTgauge(2));
        }
    }

    private IEnumerator DecreaseEMTgauge(float decreaseRate)
    {
        while (EMT_Amount > 0)
        {
            EMT_Amount -= 0.01f;
            EMT_Gauge.value = EMT_Amount;

            yield return new WaitForSeconds(0.01f / decreaseRate);
        }

        EMT_Amount = 0;
        EMT_Gauge.value = 0;
        isEMTState = false;
    }

    #region Melee Combat
    public void MeleeCombo()
    {
        Melee_CurrentAnimOutOfTime();
        Melee_IsComboOutOfTime();
        if (Input.GetMouseButtonDown(0))
        {
            MeleeCombo_OnClick();
        }
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
        //playerMovement.ResetSpeedMultiplier();
        playerMovement.ResetRotate();
        playerMovement.canWalk = true;
        playerMovement.canRun = true;

        if (isSinglePlayer)
        {
            noOfClicks = 0;

            //if (animController.IsOverideCompleted()) { return; }

            //animController.CombatOveride(0);
        }
        else
        {
            noOfClicks = 0;

            if (animController.IsOverideCompleted()) { return; }

            animController.CombatOverideServerRpc(0);
        }
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
        //SetCombatOveride();

        //playerMovement.SetSpeedMultiplierOnCombat();
        //playerMovement.SetRotateMultiplierOnCombat();
    }

    private void SetCombatOveride()
    {
        if (isSinglePlayer)
        {
            animController.CombatOveride(1);
        }
        else
        {
            animController.CombatOverideServerRpc(1);
        }
    }

    public void setComboBoolDependOn(int num)
    {
        if (num == 1)
        {
            if (isSinglePlayer)
            {
                animController.MeleeSetBool($"{heldWeapon.comboParam}Normal_hit1", true);
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
