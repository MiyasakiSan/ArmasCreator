using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using ArmasCreator.GameMode;
using ArmasCreator.Utilities;

public class CombatRpgManager : NetworkBehaviour 
{
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
    private float maxComboDelay = 0.75f;

    public enum gameState
    {
        neutral,combat
    }

    private GameModeController gameModeController;

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
    }
    void Update()
    {
        if (IsLocalPlayer || isSinglePlayer)
        {
            if (!canBattle) { return; }

            gameStateCheck();
            if(currentGameState != gameState.combat) { return; }

            CombatDependOnHeldWeapon(heldWeapon);
        }
    }
    public void changeGameState(gameState value)
    {
        currentGameState = value;
    }
    private void gameStateCheck()
    {
        if (!Input.GetKeyDown(gameStateSwitchButton)) { return ; }
        if(!animController.currentAnimatorStateInfoIsName("Idle")) { return; }
        if(currentGameState == gameState.neutral)
        {
            changeGameState(gameState.combat);
            ChangeanimLayer(currentGameState);
        }
        else
        {
            changeGameState(gameState.neutral);
            ChangeanimLayer(currentGameState);
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
        if (animController.currentAnimatorStateInfoTime <= 0.9f) { return; }

        if (animController.currentAnimatorStateInfoIsName($"{heldWeapon.comboParam}NormalAttack1"))
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

        if (animController.currentAnimatorStateInfoIsName($"{heldWeapon.comboParam}NormalAttack2"))
        {
            if (isSinglePlayer)
            {
                animController.MeleeSetBool($"{heldWeapon.comboParam}Normal_hit2", false);
            }
            else
            {
                animController.MeleeSetBoolServerRpc($"{heldWeapon.comboParam}Normal_hit2", false);
            }

            noOfClicks = 0;
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
    }
    public void setComboBoolDependOn(int num)
    {
        if (num == 1)
        {
            if (isSinglePlayer)
            {
                animController.MeleeSetBool($"{heldWeapon.comboParam}Normal_hit{num}", true);
            }
            else
            {
                animController.MeleeSetBoolServerRpc($"{heldWeapon.comboParam}Normal_hit{num}", true);
            }
        }

        if (animController.currentAnimatorStateInfoTime <= 0.7f) { return; }

        if (num == 2 )
        {
            if (isSinglePlayer)
            {
                animController.MeleeSetBool($"{heldWeapon.comboParam}Normal_hit{num}", true);
            }
            else
            {
                animController.MeleeSetBoolServerRpc($"{heldWeapon.comboParam}Normal_hit{num}", true);
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
}
