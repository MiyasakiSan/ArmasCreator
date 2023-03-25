using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArmasCreator.Utilities;

namespace ArmasCreator.UI
{
    public class SoundSettingController : MonoBehaviour
    {
        private SoundManager soundManager;

        [SerializeField]
        private List<Slider> soundSliderList;
        
        void Awake()
        {
            soundManager = SharedContext.Instance.Get<SoundManager>();
        }

        void Start()
        {
            soundSliderList[0].value = soundManager.MasterVolume;
            soundSliderList[1].value = soundManager.MusicVolume;
            soundSliderList[2].value = soundManager.SFXVolume;
            soundSliderList[3].value = soundManager.AmbienceVolume;
        }

        public void SliderMasterVolume()
        {
            soundManager.MasterVolume = soundSliderList[0].value;
        }
        public void SliderMusicVolume()
        {
            soundManager.MasterVolume = soundSliderList[1].value;
        }
        public void SliderSFXVolume()
        {
            soundManager.MasterVolume = soundSliderList[2].value;
        }
        public void SliderAmbienceVolume()
        {
            soundManager.MasterVolume = soundSliderList[3].value;
        }
    }
}
