using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public interface IDamagable<T>
{
    float currentHealth { get; }
}
public interface IStaminaUsable<T>
{

    float currentStamina { get; }
    void reduceStamina(T amount);
    IEnumerator RegenStamina();
    IEnumerator ReduceStaminaOverTime(T amount);
}
