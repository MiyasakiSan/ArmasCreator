using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using ArmasCreator.Gameplay;
using ArmasCreator.Utilities;
using ArmasCreator.GameMode;

public class WeaponShealthDraw : NetworkBehaviour
{
    [SerializeField] CombatRpgManager combatManager;
    [SerializeField] GameObject WeaponHolder;
    [SerializeField] GameObject WeaponShealth;
    [SerializeField] GameObject WeaponHeld = null;

    private GameModeController gameModeController;
    private bool isSinglePlayer => gameModeController.IsSinglePlayerMode;

    public GameObject currentWeaponInHand;
    public GameObject currentWeaponInShealth;

    private void Awake()
    {
        gameModeController = SharedContext.Instance.Get<GameModeController>();
    }

    void Start()
    {
        WeaponHeld = combatManager.heldWeapon.gameObject;

        SpawnHeldWeapon();
    }

    public void SpawnHeldWeapon()
    {
        if (isSinglePlayer)
        {
            currentWeaponInShealth = Instantiate(WeaponHeld, WeaponShealth.transform);
        }
        else
        {
            if (!IsOwner) { return; }

            SpawnWeaponHeldServerRpc();
        }
    }

    [ServerRpc(Delivery = RpcDelivery.Reliable)]
    public void SpawnWeaponHeldServerRpc()
    {
        if (WeaponHeld != null) { return; }

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
        if (isSinglePlayer)
        {
            if (currentWeaponInHand != null)
            {
                Destroy(currentWeaponInHand.gameObject);
            }

            currentWeaponInHand = Instantiate(WeaponHeld, WeaponHolder.transform);
            //currentWeaponInHand.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
            if (currentWeaponInShealth != null)
            {
                Destroy(currentWeaponInShealth.gameObject);
            }

            currentWeaponInShealth = null;
        }
        else
        {
            if (!IsOwner) { return; }

            DrawWeaponServerRpc();
        }
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

    public void cancelShealth()
    {
        if (currentWeaponInShealth != null)
        {
            Destroy(currentWeaponInShealth.gameObject);
        }

        if (currentWeaponInHand != null)
        {
            Destroy(currentWeaponInHand.gameObject);
        }

        currentWeaponInHand = null;

        currentWeaponInShealth = null;

        currentWeaponInShealth = Instantiate(WeaponHeld, WeaponShealth.transform);
    }

    public void ShealthWeaponEvent()
    {
        if (isSinglePlayer)
        {
            if (currentWeaponInShealth != null)
            {
                Destroy(currentWeaponInShealth.gameObject);
            }

            currentWeaponInShealth = Instantiate(WeaponHeld, WeaponShealth.transform);
            //currentWeaponInShealth.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);

            if (currentWeaponInHand != null)
            {
                Destroy(currentWeaponInHand.gameObject);
            }

            currentWeaponInHand = null;
        }
        else
        {
            if (!IsOwner) { return; }

            ShealthWeaponServerRpc();
        }
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
