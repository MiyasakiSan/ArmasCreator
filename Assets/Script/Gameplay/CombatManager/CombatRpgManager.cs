using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

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
    public float cooldownTime = 2f;
    private float nextFireTime = 0f;
    public static int noOfClicks = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 1;
    public enum gameState
    {
        neutral,combat
    }
    void Start()
    {
        if (IsLocalPlayer)
        {
            changeGameState(gameState.neutral);
        }
    }
    void Update()
    {
        if (IsLocalPlayer)
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
                animController.changeCombatLayerWeightServerRpc(0f);
                break;
            case gameState.combat:
                animController.changeCombatLayerWeightServerRpc(1f);
                break;
        }
    }
    private void CombatDependOnHeldWeapon(Weapon currentWeapon)
    {
        switch (currentWeapon)
        {
            case LongSword:
                LongSwordCombo();
                break;
        }
    }
    #region LongSword Combat
    public void LongSwordCombo()
    {
        LongSword_CurrentAnimOutOfTime();
        LongSword_IsComboOutOfTime();
        if (Input.GetMouseButtonDown(0))
        {
            LongSwordCombo_OnClick();
        }
    }
    public void LongSword_CurrentAnimOutOfTime()
    {
        if (animController.currentAnimatorStateInfoTime <= 0.9f) { return; }
        if (animController.currentAnimatorStateInfoIsName("SwordAttack1"))
        {
            animController.LongSwordSetBoolServerRpc("LongSword_hit1", false);
        }
        if (animController.currentAnimatorStateInfoIsName("SwordAttack2"))
        {
            animController.LongSwordSetBoolServerRpc("LongSword_hit2", false);
            noOfClicks = 0;
        }
    }
    public void LongSword_IsComboOutOfTime()
    {
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }
        if (Time.time <= nextFireTime) { return; }
    }
    public void LongSwordCombo_OnClick()
    {
        lastClickedTime = Time.time;
        noOfClicks++;
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 2);
        setComboBoolDependOn(noOfClicks);
    }
    public void setComboBoolDependOn(int num)
    {
        if (num == 1 )
        {
            animController.LongSwordSetBoolServerRpc("LongSword_hit1", true);
        }
        if (animController.currentAnimatorStateInfoTime <= 0.7f) { return; }
        if (num == 2 && animController.currentAnimatorStateInfoIsName("SwordAttack1"))
        {
            animController.LongSwordSetBoolServerRpc("LongSword_hit2", true);
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
