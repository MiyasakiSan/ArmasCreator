using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crossForwardTest : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    public float speed;

    private Rigidbody rb;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 crossForward = Vector3.Cross(target.transform.position - transform.position, transform.up).normalized;

        rb.AddForce(crossForward *speed *Time.deltaTime,ForceMode.VelocityChange);
    }
}
