using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(this.gameObject, 3f);
    }

    void Update()
    {
        rb.AddForce(transform.forward * 10f * Time.deltaTime, ForceMode.Impulse);
    }
}
