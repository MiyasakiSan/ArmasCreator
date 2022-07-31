using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IMeleeWeapon : Weapon
{
    float attackSpeed { get; }
    float staggerDamage { get; }
    statusEffect weaponStatus { get; }
    BoxCollider meleeCollider { get; }
    public void showHitbox_OnAnim()
    {
        this.GetComponent<Collider>().enabled = true;
    }
    public void hideHitbox_OnAnim()
    {
        this.GetComponent<Collider>().enabled = false;
    }
}
public enum statusEffect
{
    burn,
    paralysis,
    bleed,
    poison,
    none
}
