using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArmasCreator.Utilities;

namespace ArmasCreator.Sounds
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField]
        private AudioSource UISource;

        [SerializeField]
        private AudioSource BackGroundSource;

        void Awake()
        {
            DontDestroyOnLoad(this);
            SharedContext.Instance.Add(this);
        }

        public void PlaySound(SoundObject soundInfo)
        {

        }

        private void OnDestroy()
        {
            SharedContext.Instance.Remove(this);
            Destroy(this.gameObject);
        }
    }

}

