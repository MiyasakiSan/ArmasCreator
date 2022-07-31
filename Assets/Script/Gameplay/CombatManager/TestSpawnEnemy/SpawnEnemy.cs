using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnEnemy : NetworkBehaviour
{
    // Start is called before the first frame update
    public GameObject EnemyPrefab;
    public Transform spawnPos;
    void Start()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            SpawnEnemyOnStart();
        }
    }
    private void SpawnEnemyOnStart()
    {
        GameObject enemy = Instantiate(EnemyPrefab, spawnPos.position, spawnPos.rotation);
        enemy.GetComponent<NetworkObject>().Spawn();
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
}
