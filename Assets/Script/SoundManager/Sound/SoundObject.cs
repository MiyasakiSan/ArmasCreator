using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArmasCreator.Sounds
{
    [CreateAssetMenu(fileName = "New Sound Object", menuName = "SoundObject")]
    public class SoundObject : ScriptableObject
    {
        [SerializeField]
        private AudioClip soundClip;

        [SerializeField]
        private SoundType soundType;

        [SerializeField]
        private float volume;

        [SerializeField]
        private bool isLoop;
    }

    public enum SoundType
    {
        none,
        Background,
        Ambient,
        SFX,
        Player,
        UI
    }
}

    
