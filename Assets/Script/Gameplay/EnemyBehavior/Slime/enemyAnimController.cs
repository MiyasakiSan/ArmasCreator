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

    public bool isTutorial;

    [SerializeField]
    private DynamicBone dynamicBone;

    [SerializeField]
    private List<FastIKLook> allLookAtIK;

    [Header("Move Set")]

    [SerializeField]
    private ProjectileSpawner spikeSpawner;

    [SerializeField]
    private GameObject DeadCamCart;

    public bool isFollwPlayer;
    private MonsterSFX monsterSFX;

    void Start()
    {
        gameplayController = SharedContext.Instance.Get<GameplayController>();
        monsterSFX = GetComponent<MonsterSFX>();

        if (gameplayController != null)
        {
            anim.SetFloat("speedMultiplier", gameplayController.CurrentQuestInfo.InitSpeed);

            if (gameplayController.EnemyType == ArmasCreator.GameData.SubType.Shrimp)
            {
                monsterSFX.PlayMachineSFX();
            }
            else
            {
                monsterSFX.PlayTigerIdleSFX();
            }
        }
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

    public void Attack()
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
            anim?.SetBool("isDead",true);

            if (isTutorial)
            {
                Destroy(this.gameObject, 3.5f);
            }

            if (gameplayController == null) { return; }

            gameplayController.EnterGameplayResult();
            DeadCamCart.SetActive(true);

            if (gameplayController.EnemyType == ArmasCreator.GameData.SubType.Shrimp)
            {
                monsterSFX.StopMachineSFX();
            }
            else
            {
                monsterSFX.StopTigerIdleSFX();
                monsterSFX.PlayTigerDeadSFX();
            }

            GetComponent<MonsterUseVFX>().IsEnterRageMode(false);

            var player = GameObject.FindGameObjectWithTag("Player");

            player.GetComponent<CombatRpgManager>().canBattle = false;
            player.GetComponent<PlayerRpgMovement>().canMove = false;
            //StartCoroutine(despawn());
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
