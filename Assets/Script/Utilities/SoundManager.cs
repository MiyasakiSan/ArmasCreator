using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using ArmasCreator.Utilities;
using FMOD.Studio;

public class SoundManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0, 1)]
    public float MasterVolume = 1;
    [Range(0, 1)]
    public float MusicVolume = 1;
    [Range(0, 1)]
    public float AmbienceVolume = 1;
    [Range(0, 1)]
    public float SFXVolume = 1;

    private Bus masterBus;
    private Bus musicBus;
    private Bus ambienceBus;
    private Bus sfxBus;

    List<EventInstance> eventInstances;
    List<StudioEventEmitter> eventEmitters;

    EventInstance musicEventInstances;

    public FModEvent fModEvent;

    bool isInitlize;

    void Awake()
    {
        Initilize();
    }

    void Initilize()
    {
        SharedContext.Instance.Add(this);

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();

        isInitlize = true;

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");

        SetupVolume();

        DontDestroyOnLoad(this.gameObject);
    }

    void SetupVolume()
    {
        if (!PlayerPrefs.HasKey("MasterVolume"))
        {
            PlayerPrefs.SetFloat("MasterVolume", 1);
        }
        else
        {
            MasterVolume = PlayerPrefs.GetFloat("MasterVolume");
        }

        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 1);
        }
        else
        {
            MusicVolume = PlayerPrefs.GetFloat("MusicVolume");
        }

        if (!PlayerPrefs.HasKey("AmbienceVolume"))
        {
            PlayerPrefs.SetFloat("AmbienceVolume", 1);
        }
        else
        {
            AmbienceVolume = PlayerPrefs.GetFloat("AmbienceVolume");
        }

        if (!PlayerPrefs.HasKey("SFXVolume"))
        {
            PlayerPrefs.SetFloat("SFXVolume", 1);
        }
        else
        {
            SFXVolume = PlayerPrefs.GetFloat("SFXVolume");
        }
    }

    private void Update()
    {
        masterBus.setVolume(MasterVolume);
        musicBus.setVolume(MusicVolume);
        ambienceBus.setVolume(AmbienceVolume);
        sfxBus.setVolume(SFXVolume);
    }

    public void PlayOneShot(EventReference sound,Vector3 position)
    {
        RuntimeManager.PlayOneShot(sound,position);
    }

    public void PlayOneShot(EventReference sound,GameObject gameObject)
    {
        RuntimeManager.PlayOneShotAttached(sound,gameObject);
    }

    public EventInstance CreateInstance(EventReference reference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(reference);
        eventInstances.Add(eventInstance);
        
        return eventInstance;
    }

    public void AttachInstanceToGameObject(EventInstance eventInstance,Transform transform,Rigidbody rigidbody)
    {
        RuntimeManager.AttachInstanceToGameObject(eventInstance,transform,rigidbody);
    }

    public StudioEventEmitter InitailizerEventEmitter(EventReference eventReference,GameObject gameObject)
    {
        StudioEventEmitter emitter = gameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }

    void CleanUp()
    {
        foreach(EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }

        foreach(StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }

    void SaveVolume()
    {
        PlayerPrefs.SetFloat("MasterVolume", MasterVolume);
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetFloat("AmbienceVolume", AmbienceVolume);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
    }

    public void OnDestroy()
    {
        SaveVolume();
        CleanUp();
    }
}
