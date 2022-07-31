using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRangedWeapon
{ 
    float shootSpeed { get; }
    float reloadSpeed { get; }
    ammoContain[] ammoSet { get; }
    
}
public struct ammoContain
{
    public RangedAmmo rangedAmmo;
    public float amount;
    public ammoContain(RangedAmmo rangedAmmo,float amount)
    {
        this.rangedAmmo = rangedAmmo;
        this.amount = amount;
    }
}

