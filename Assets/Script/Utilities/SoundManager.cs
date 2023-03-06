using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using ArmasCreator.Utilities;
using FMOD.Studio;

public class SoundManager : MonoBehaviour
{
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

        DontDestroyOnLoad(this.gameObject);
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

    public void OnDestroy()
    {
        CleanUp();
    }
}
