using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using ArmasCreator.Gameplay;
using ArmasCreator.Utilities;

public class enemyAnimController : NetworkBehaviour
{
    public Animator anim;
    [SerializeField] private GameObject coin;

    private GameplayController gameplayController;
    // Start is called before the first frame update
    void Start()
    {
        gameplayController = SharedContext.Instance.Get<GameplayController>();
    }
    public bool currentAnimatorStateBaseIsName(string paramName)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(paramName);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    [ServerRpc]
    public void MovingServerRpc(bool value)
    {
        anim.SetBool("isMoving", value);
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
            gameplayController.EnterGameplayResult();
            return;
        }
        else
        {
            anim?.SetTrigger("isDead");
            StartCoroutine(despawn());
        }
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

    [ClientRpc]
    public void SpawnCoinClientRpc()
    {
        GameObject coinSpawn = Instantiate(coin, transform.position, Quaternion.identity);
    }

    IEnumerator despawn()
    {
        yield return new WaitForSeconds(2f);
        SpawnCoinClientRpc();
        GetComponent<NetworkObject>().Despawn();
    }
}
