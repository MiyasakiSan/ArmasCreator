using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Ammo",menuName ="Ammos/ammo")]
public class RangedAmmo : ScriptableObject
{
    public GameObject ammoPrefab;
    public ammoType ammoType;
}
public enum ammoType
{
    bomb,
    shock,
    burn,
    poison,
    none
}
