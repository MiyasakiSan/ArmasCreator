using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject ProjectilePrefab;

    private Transform target;
    private Rigidbody rb;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        rb = target.GetComponent<Rigidbody>();
    }

    void Update()
    {

        Vector3 lockPos = new Vector3(target.transform.position.x + (rb.velocity.x * 0.8f),
                                      target.transform.position.y -0.5f,
                                      target.transform.position.z + (rb.velocity.z * 0.8f));

        this.transform.LookAt(lockPos);
    }

    public void SpawnProjectile()
    {
        Instantiate(ProjectilePrefab, this.transform.position, this.transform.rotation);
    }
}
