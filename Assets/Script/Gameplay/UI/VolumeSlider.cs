using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArmasCreator.Utilities;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private SoundManager soundManager;
    public enum SoundType
    {
        Master,
        Music,
        Ambience,
        SFX
    }

    [Header("Volume Type")]
    public SoundType soundType;

    [SerializeField]
    private Slider volumeSlider;

    void Start()
    {
        soundManager = SharedContext.Instance.Get<SoundManager>();

        switch (soundType)
        {
            case (SoundType.Master):
                {
                    volumeSlider.value = soundManager.MasterVolume;
                    break;
                }
            case (SoundType.Ambience):
                {
                    volumeSlider.value = soundManager.AmbienceVolume;
                    break;
                }
            case (SoundType.Music):
                {
                    volumeSlider.value = soundManager.MusicVolume;
                    break;
                }
            case (SoundType.SFX):
                {
                    volumeSlider.value = soundManager.SFXVolume;
                    break;
                }
        }
    }

    void Update()
    {
        
    }

    public void OnSliderVolumeChanged()
    {
        switch (soundType)
        {
            case (SoundType.Master):
                {
                    soundManager.MasterVolume = volumeSlider.value;
                    break;
                }
            case (SoundType.Ambience):
                {
                    soundManager.AmbienceVolume = volumeSlider.value;
                    break;
                }
            case (SoundType.Music):
                {
                    soundManager.MusicVolume = volumeSlider.value;
                    break;
                }
            case (SoundType.SFX):
                {
                    soundManager.SFXVolume = volumeSlider.value;
                    break;
                }
        }
    }
}
