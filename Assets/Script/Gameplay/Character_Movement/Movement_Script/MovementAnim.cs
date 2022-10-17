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

    [Header("Combat Manager")]
    [SerializeField]
    private int combatLayerIndex;
    // Start is called before the first frame update
    public float currentAnimatorStateInfoTime
    {
        get { return playerAnim.GetCurrentAnimatorStateInfo(1).normalizedTime; }
    }
    public bool currentAnimatorStateInfoIsName(string paramName)
    {
        return playerAnim.GetCurrentAnimatorStateInfo(1).IsName(paramName);
    }
    public bool currentAnimatorStateBaseIsName(string paramName)
    {
        return playerAnim.GetCurrentAnimatorStateInfo(0).IsName(paramName);
    }
    void Start()
    {

    }

    // Update is called once per frame
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
        playerAnim.SetTrigger("roll");
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

    #region LayerWeight Animation

    public void changeCombatLayerWeight(float weight)
    {
        StartCoroutine(SwitchStance(weight));
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
            playerAnim.SetLayerWeight(combatLayerIndex, weight);
            playerAnim.SetBool("isCombat", !playerAnim.GetBool("isCombat"));
        }
        else
        {
            playerAnim.SetBool("isCombat", !playerAnim.GetBool("isCombat"));
            yield return new WaitForSeconds(1.6f);
            playerAnim.SetLayerWeight(combatLayerIndex, weight);
        }
    } 
    #endregion

    public void MeleeSetBool(string paramName, bool var)
    {
        List<string> MeleeParams = new List<string> { "LongSwordNormal_hit1", "LongSwordNormal_hit2" };
        foreach (AnimatorControllerParameter parameter in playerAnim.parameters)
        {
            if (MeleeParams.Contains(parameter.name))
            {
                playerAnim.SetBool(parameter.name, false);
            }
        }
        playerAnim.SetBool(paramName, var);
    }

    [ServerRpc]
    public void MeleeSetBoolServerRpc(string paramName,bool var)
    {
        //TODO : Create GameDataManager for not hard CODING !! BITCH
        List<string> MeleeParams = new List<string> { "LongSwordNormal_hit1", "LongSwordNormal_hit2" };
        foreach (AnimatorControllerParameter parameter in playerAnim.parameters)
        {
            if (MeleeParams.Contains(parameter.name))
            {
                playerAnim.SetBool(parameter.name, false);
            }
        }
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
