using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public interface IDamagable<T>
{
    float CurrentHealth { get; }
}
public interface IStaminaUsable<T>
{

    float CurrentStamina { get; }
    void reduceStamina(T amount);
    IEnumerator RegenStamina();
    IEnumerator ReduceStaminaOverTime(T amount);
}
