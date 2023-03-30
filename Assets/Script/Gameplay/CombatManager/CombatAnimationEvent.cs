using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAnimationEvent : MonoBehaviour
{
    private PlayerRpgMovement playerMovement;

    [SerializeField]
    private CombatRpgManager combatManager;

    public void Init(PlayerRpgMovement playerMovement)
    {
        this.playerMovement = playerMovement;
    }

    public void CanRotateEvent()
    {
        playerMovement.ResetRotate();
    }

    public void CannotRotateEvent()
    {
        playerMovement.SetCanRotate();
    }

    public void MoveForwardEvent(float speed)
    {
        playerMovement.MoveForward(speed);
    }

    public void StopMoveForwardEvent()
    {
        playerMovement.StopMoveForward();
    }

    public void StopMoveForwardVelo()
    {
        playerMovement.StopMoveForwardNotResetVelo();
    }

    public void StopShoot()
    {
        combatManager.EndShoot();
    }

    public void WithdrawFinish()
    {
        combatManager.isWithdrawing = false;
    }

    public void SheathFinish()
    {
        combatManager.isSheathing = false;
    }
}
