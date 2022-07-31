using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface WeaponData 
{
    public float weaponDamage { get; }
    public float critRate { get; }
    public float weight { get; }
    public int sellPrice { get; }

}
