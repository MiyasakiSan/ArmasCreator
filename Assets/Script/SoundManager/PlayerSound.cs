using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerSound : NetworkBehaviour
{
    AudioSource audioSource;
    public AudioClip[] PlayerSounds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(IsLocalPlayer && audioSource == null)
        //{
        //    audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
        //}
    }

    public void PlaySound(int Sound)
    {
        //audioSource.clip = PlayerSounds[Sound];
        //audioSource.Play();
    }
}
