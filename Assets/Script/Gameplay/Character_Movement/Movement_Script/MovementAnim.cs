using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Animations;
using ArmasCreator.GameMode;
using ArmasCreator.Utilities;
public class MovementAnim : NetworkBehaviour
{

    public Animator playerAnim;

    private PlayerRpgMovement playerMovement;

    [SerializeField]
    private CombatAnimationEvent animEvent;

    [Header("Combat Manager")]
    [SerializeField]
    private int combatLayerIndex;

    [SerializeField]
    private int combatLayerOverideIndex;

    [SerializeField]
    private int useItemLayerIndex;

    private Coroutine delayOverider;
    private Coroutine switchStance;

    public void Init(PlayerRpgMovement playerMovement)
    {
        this.playerMovement = playerMovement;

        animEvent.Init(playerMovement);
    }

    public float currentAnimatorStateInfoTime
    {
        get { return playerAnim.GetCurrentAnimatorStateInfo(1).normalizedTime % 1; }
    }
    public bool currentAnimatorCombatStateInfoIsName(string paramName)
    {
        return playerAnim.GetCurrentAnimatorStateInfo(1).IsName(paramName);
    }
    public bool currentAnimatorStateBaseIsName(string paramName)
    {
        return playerAnim.GetCurrentAnimatorStateInfo(0).IsName(paramName);
    }

    #region Movement Animation

    public void AnimationState(string paramName)
    {
        List<string> movementParams = new List<string> { "walk", "run", "idle" };
        foreach (AnimatorControllerParameter parameter in playerAnim.parameters)
        {
            if (movementParams.Contains(parameter.name))
            {
                playerAnim.SetBool(parameter.name, false);
            }
        }
        playerAnim.SetBool(paramName, true);
    }

    public void ResetAnimBoolean()
    {
        playerAnim.SetBool("run", false);
        playerAnim.SetBool("walk", false);
        playerAnim.SetBool("idle", true);
    }

    [ServerRpc]
    public void AnimationStateServerRpc(string paramName)
    {
        List<string> movementParams = new List<string> { "walk", "run", "idle" };
        foreach (AnimatorControllerParameter parameter in playerAnim.parameters)
        {
            if (movementParams.Contains(parameter.name))
            {
                playerAnim.SetBool(parameter.name, false);
            }
        }
        playerAnim.SetBool(paramName, true);
    }
    #endregion

    #region Dodge Animation
    public void Dodge()
    {
        playerAnim.SetBool("roll",true);
    }

    public void StopDodge()
    {
        playerAnim.SetBool("roll", false);
    }

    [ServerRpc]
    public void DodgeServerRpc()
    {
        DodgeClientRpc();
    }
    [ClientRpc]
    public void DodgeClientRpc()
    {
        playerAnim.SetTrigger("roll");
    }
    #endregion

    public void StartUseItemAnimation(string itemId)
    {
        playerAnim.SetLayerWeight(useItemLayerIndex, 1);

        if (itemId == "bandage")
        {
            playerAnim.SetTrigger("bandaging");
        }
        else if (itemId == "adrenaline")
        {
            playerAnim.SetTrigger("injecting");
        }
        else
        {
            playerAnim.SetTrigger("useItem");
        }
    }

    public void EndUseItemAnimation()
    {
        playerAnim.SetLayerWeight(useItemLayerIndex, 0);
    }

    public void ShootAnimation()
    {
        playerAnim.SetLayerWeight(3, 1);

        playerAnim.SetBool("shoot", true);

        StartCoroutine(endShootAnim());
    }

    IEnumerator endShootAnim()
    {
        yield return new WaitForSeconds(2f);

        playerAnim.SetBool("shoot", false);
        playerAnim.SetLayerWeight(3, 0);
        playerMovement.gameObject.GetComponent<CombatRpgManager>().isShooting = false;
    }

    public void EndShootAnimation()
    {
        playerAnim.SetBool("shoot", false);
        playerAnim.SetLayerWeight(3, 0);
    }

    #region LayerWeight Animation

    public void changeCombatLayerWeight(float weight)
    {
        if(switchStance != null) 
        {
            switchStance = null;
        }

        switchStance = StartCoroutine(SwitchStance(weight));
    }

    [ServerRpc]
    public void changeCombatLayerWeightServerRpc(float weight)
    {
        StartCoroutine(SwitchStance(weight));
    }
    #endregion

    #region Switch Stance
    IEnumerator SwitchStance(float weight)
    {
        if (weight == 1)
        {
            playerMovement.canMove = false;
            playerAnim.SetBool("isCombat", true);
            playerAnim.SetLayerWeight(combatLayerIndex, weight);
            yield return new WaitForSeconds(1f);
            playerMovement.canMove = true;
        }
        else
        {
            playerMovement.canMove = false;
            playerAnim.SetBool("isCombat", false);
            yield return new WaitForSeconds(1f);
            playerAnim.SetLayerWeight(combatLayerIndex, weight);
            playerMovement.canMove = true;
        }

        switchStance = null;
    }

    [ServerRpc]
    public void CombatOverideServerRpc(float weight)
    {
        if(weight == 1)
        {
            if(delayOverider != null)
            {
                StopCoroutine(delayOverider);
                delayOverider = null;
            }

            playerAnim.SetLayerWeight(combatLayerOverideIndex, weight);
        }
        else
        {
            if (playerAnim.GetLayerWeight(combatLayerOverideIndex) == 0) { return; }

            if (delayOverider != null ) { return; }

            delayOverider = StartCoroutine(DelayOverideCombat());
        }
    }

    public void CombatOveride(float weight)
    {
        if (weight == 1)
        {
            if (delayOverider != null)
            {
                StopCoroutine(delayOverider);
                delayOverider = null;
            }

            playerAnim.SetLayerWeight(combatLayerOverideIndex, weight);
        }
        else
        {
            if(playerAnim.GetLayerWeight(combatLayerOverideIndex) == 0) { return; }

            if (delayOverider != null) { return; }

            delayOverider = StartCoroutine(DelayOverideCombat());
        }
    }

    IEnumerator DelayOverideCombat()
    {
        float weight = 1;

        while(weight > 0)
        {
            playerAnim.SetLayerWeight(combatLayerOverideIndex, weight);
            weight -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        playerAnim.SetLayerWeight(combatLayerOverideIndex, 0);
        delayOverider = null;
    }

    public bool IsOverideCompleted()
    {
        return playerAnim.GetLayerWeight(combatLayerOverideIndex) == 0;
    }

    #endregion

    public void MeleeSetBool(string paramName, bool var)
    {
        playerAnim.SetBool(paramName, var);
    }

    [ServerRpc]
    public void MeleeSetBoolServerRpc(string paramName,bool var)
    {
        playerAnim.SetBool(paramName, var);
    }

    public void dieAnimaiton()
    {
        playerAnim.SetTrigger("isDead");
    }

    [ServerRpc]
    public void dieAnimaitonServerRpc()
    {
        dieAnimaitonClientRpc();
    }

    [ClientRpc]
    public void dieAnimaitonClientRpc()
    {
        
        playerAnim.SetTrigger("isDead");
    }
}
