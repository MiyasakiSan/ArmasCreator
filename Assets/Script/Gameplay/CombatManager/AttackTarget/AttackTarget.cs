using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public abstract class AttackTarget :NetworkBehaviour
{
    public abstract void receiveAttack(float damage);
}
