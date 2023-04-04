using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using ArmasCreator.GameMode;
using ArmasCreator.Utilities;
using ArmasCreator.Gameplay;

public class EnemyStat : AttackTarget, IDamagable<float>
{
    [SerializeField]
    private float maxHealth;
    public float MaxHealth => maxHealth;

    [SerializeField]
    NetworkVariable<float> NetworkcurrentHealth = new NetworkVariable<float>();
    public UnityAction<float> onHealthUpDate;

    private GameModeController gameModeController;
    private GameplayController gameplayController;

    private bool isSinglePlayer => gameModeController.IsSinglePlayerMode;

    [SerializeField]
    private float currentHealth;

    public float CurrentHealth
    {
        get
        {
            if (isSinglePlayer)
            {
                return currentHealth;
            }
            else
            {
                return NetworkcurrentHealth.Value;
            }
        }
        set
        {
            if (isSinglePlayer)
            {
                currentHealth = value;
            }
            else
            {
                NetworkcurrentHealth.Value = value;
            }
        }
    }
    [ServerRpc]
    public void currentHealthServerRpc(float value)
    {
        CurrentHealth = value;
    }
    public override void receiveAttack(float damage)
    {
        if (isSinglePlayer && currentHealth > 0)
        {
            CurrentHealth -= damage;
        }
        else
        {
            receiveAttackServerRpc(damage);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void receiveAttackServerRpc(float damage)
    {
        CurrentHealth -= damage;
    }

    private void Awake()
    {
        gameModeController = SharedContext.Instance.Get<GameModeController>();
        gameplayController = SharedContext.Instance.Get<GameplayController>();
    }

    void Start()
    {
        if (isSinglePlayer)
        {
            if (gameplayController != null)
            {
                currentHealth = maxHealth * gameplayController.CurrentQuestInfo.InitHp;
            }
            else
            {
                currentHealth = maxHealth;
            }
        }
        else
        {
            currentHealthServerRpc(maxHealth);
        }
    }
}
