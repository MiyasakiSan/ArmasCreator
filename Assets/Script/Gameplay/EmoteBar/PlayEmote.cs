using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Animations;
using Unity.Netcode;

public class PlayEmote : NetworkBehaviour
{
    // Start is called before the first frame update
    public Animator PlayerAnimator;
    public GameObject EmoteBar;
    AudioSource audioSource;
    public AudioClip[] audioClip;
    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer)
        {
            //TODO : implement Audio manager

            //if(audioSource == null)
            //{
            //    audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
            //}

            if (Input.GetKeyDown(KeyCode.B))
            {
                EmoteBar.SetActive(!EmoteBar.activeSelf);
            }
        }
    }

    public void Emote(int var)
    {
        PlayerAnimator.Play($"Dance{var}");
        EmoteServerRpc(var);
        audioSource.clip = audioClip[var - 1];
        audioSource.Play();
        EmoteBar.SetActive(!EmoteBar.activeSelf);
    }
    [ServerRpc]
    public void EmoteServerRpc(int var)
    {
        PlayerAnimator.Play($"Dance{var}");
        EmoteClientRpc(var);
    }
    [ClientRpc]
    public void EmoteClientRpc(int var)
    {
        PlayerAnimator.Play($"Dance{var}");
    }
}
