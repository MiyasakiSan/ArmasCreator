using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAnimationEvent : MonoBehaviour
{
    private PlayerRpgMovement playerMovement;

    public void Init(PlayerRpgMovement playerMovement)
    {
        this.playerMovement = playerMovement;
    }

    public void CanRotateEvent()
    {
        playerMovement.SetCanRotate();
    }

    public void CannotRotateEvent()
    {
        playerMovement.ResetRotate();
    }

    public void MoveForwardEvent(float speed)
    {
        playerMovement.MoveForward(speed);
    }
}
