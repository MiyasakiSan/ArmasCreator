using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
public class EnemyStat : AttackTarget, IDamagable<float>
{
    private float maxHealth;
    public float MaxHealth => maxHealth;

    [SerializeField]
    NetworkVariable<float> NetworkcurrentHealth = new NetworkVariable<float>();
    public UnityAction<float> onHealthUpDate;

    public float CurrentHealth
    {
        get { return NetworkcurrentHealth.Value; }
        set { NetworkcurrentHealth.Value = value; }
    }
    [ServerRpc]
    public void currentHealthServerRpc(float value)
    {
        CurrentHealth = value;
    }
    public override void receiveAttack(float damage)
    {
        receiveAttackServerRpc(damage);
    }
    [ServerRpc(RequireOwnership = false)]
    public void receiveAttackServerRpc(float damage)
    {
        CurrentHealth -= damage;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner) { return; }
        currentHealthServerRpc(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
