using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using ArmasCreator.Gameplay;
using ArmasCreator.Utilities;
using TheKiwiCoder;
using DitzelGames.FastIK;

public class enemyAnimController : NetworkBehaviour
{
    public Animator anim;

    private GameplayController gameplayController;

    [SerializeField]
    private DynamicBone dynamicBone;

    [SerializeField]
    private List<FastIKLook> allLookAtIK;

    [Header("Move Set")]

    [SerializeField]
    private ProjectileSpawner spikeSpawner;

    public bool isFollwPlayer;

    void Start()
    {
        gameplayController = SharedContext.Instance.Get<GameplayController>();

        anim.SetFloat("speedMultiplier", gameplayController.CurrentQuestInfo.InitSpeed);
    }
    public bool currentAnimatorStateBaseIsName(string paramName)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(paramName);
    }

    public void SetAllLookAt(Transform transform)
    {
        foreach (var IK in allLookAtIK)
        {
            IK.Target = transform;
        }
    }

    public void SetAllLookAtWeight(float amount)
    {
        foreach (var IK in allLookAtIK)
        {
            IK.SetWeight(amount);
        }
    }

    public void ResetAllLookAt()
    {
        foreach (var IK in allLookAtIK)
        {
            IK.ResetWeight();
        }
    }

    public void SetDynamicBoneWeight(float value)
    {
        dynamicBone.SetWeight(value);
    }

    public void ResetDynamicBoneWeight()
    {
        dynamicBone.SetWeight(1);
    }

    public void SetMoving(bool value)
    {
        anim.SetBool("isMoving", value);
    }

    public void SetWalking(bool value)
    {
        anim.SetBool("isWalking", value);
    }

    [ServerRpc]
    public void AttackServerRpc()
    {
        AttackClientRpc();
    }

    [ClientRpc]
    public void AttackClientRpc()
    {
        anim.SetTrigger("Attack");
    }

    [ServerRpc]
    public void DeadServerRpc()
    {
        DeadClientRpc();
        StartCoroutine(despawn());
    }

    public void Dead()
    {
        if(anim == null)
        {
            return;
        }
        else
        {
            anim?.SetTrigger("isDead");
            gameplayController.EnterGameplayResult();
            StartCoroutine(despawn());
        }
    }

    public void RunAnimation(AnimationClip clip)
    {
        anim.Play(Animator.StringToHash(clip.name));
    }

    public void SetAnimationRootNode(bool isApply)
    {
        anim.applyRootMotion = isApply;
    }
    
    public void EnableFollowPlayer()
    {
        isFollwPlayer = true;
    }

    public void DisableFollowPlayer()
    {
        isFollwPlayer = false;
    }


    [ClientRpc]
    public void DeadClientRpc()
    {
        anim.SetTrigger("isDead");
    }

    [ServerRpc]
    public void AlertServerRpc()
    {
        AlertClientRpc();
    }

    [ClientRpc]
    public void AlertClientRpc()
    {
        anim.SetTrigger("Alert");
    }

    IEnumerator despawn()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }


    #region Boss animation event
    public void SpawnSpike()
    {
        spikeSpawner.SpawnProjectile();

        Debug.Log("Spawn Spike!");
    } 
    #endregion
}
