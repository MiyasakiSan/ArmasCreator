using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.ParticleSystemJobs;

public class Bubble : MonoBehaviour
{
    public GameObject Target;
    public bool reached;

    [SerializeField]
    private ParticleSystem bubbleParticle;

    void Start()
    {
        bubbleParticle.Pause();
    }

    void Update()
    {
        if (reached) { return; }
        if (Target == null) { return; }

        transform.position = Vector3.Lerp(transform.position, Target.transform.position, Time.deltaTime * 0.5f);

        if (Vector3.Distance(transform.position, Target.transform.position) < 0.1f)
        {
            reached = true;
            bubbleParticle.Play();
        }
    }
}
