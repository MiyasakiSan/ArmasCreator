using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class WeaponShealthDraw : NetworkBehaviour
{
    [SerializeField] CombatRpgManager combatManager;
    [SerializeField] GameObject WeaponHolder;
    [SerializeField] GameObject WeaponShealth;
    [SerializeField] GameObject WeaponHeld;

    public GameObject currentWeaponInHand;
    public GameObject currentWeaponInShealth;
    void Start()
    {
        WeaponHeld = combatManager.heldWeapon.gameObject;
        if (!IsOwner) { return; }
        SpawnWeaponHeldServerRpc();
    }
    [ServerRpc(Delivery =RpcDelivery.Reliable)]
    public void SpawnWeaponHeldServerRpc()
    {
        SpawnWeaponHeldClientRpc();
    }
    [ClientRpc]
    public void SpawnWeaponHeldClientRpc()
    {
        currentWeaponInShealth = Instantiate(WeaponHeld, WeaponShealth.transform);
        //currentWeaponInShealth.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
    }
    public void DrawWeaponEvent()
    {
        if (!IsOwner) { return; }
        DrawWeaponServerRpc();
    }
    [ServerRpc(Delivery = RpcDelivery.Reliable)]
    public void DrawWeaponServerRpc()
    {
        DrawWeaponClientRpc();
    }
    [ClientRpc]
    public void DrawWeaponClientRpc()
    {
        currentWeaponInHand = Instantiate(WeaponHeld, WeaponHolder.transform);
        //currentWeaponInHand.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        Destroy(currentWeaponInShealth.gameObject);
        currentWeaponInShealth = null;
    }
    public void ShealthWeaponEvent()
    {
        if (!IsOwner) { return; }
        ShealthWeaponServerRpc();
    }
    [ServerRpc(RequireOwnership =false)]
    public void ShealthWeaponServerRpc()
    {
        ShealthWeaponClientRpc();
    }
    [ClientRpc]
    public void ShealthWeaponClientRpc()
    {
        currentWeaponInShealth = Instantiate(WeaponHeld, WeaponShealth.transform);
        //currentWeaponInShealth.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        currentWeaponInShealth.transform.parent = this.WeaponShealth.transform;
        Destroy(currentWeaponInHand.gameObject);
        currentWeaponInHand = null;
    }
}
