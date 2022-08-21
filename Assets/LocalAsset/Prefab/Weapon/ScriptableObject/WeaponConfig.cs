using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon",menuName = "Weapon")]
public class WeaponConfig : ScriptableObject
{
    public Weapon Weapon;
    public string ComboParamName;
}
