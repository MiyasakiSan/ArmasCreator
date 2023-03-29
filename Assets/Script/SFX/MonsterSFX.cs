using ArmasCreator.Utilities;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSFX : MonoBehaviour
{
    private SoundManager soundManager;

    private Rigidbody rb;

    EventInstance ShrimpLegSFX;
    EventInstance ShrimpLightningSFX;
    EventInstance ShrimpMachineSFX;
    EventInstance ShrimpPlasmaBallSFX;

    void Awake()
    {
        soundManager = SharedContext.Instance.Get<SoundManager>();
        rb = GetComponent<Rigidbody>();

        ShrimpLegSFX = soundManager.CreateInstance(soundManager.fModEvent.LegSFX);
        ShrimpLightningSFX = soundManager.CreateInstance(soundManager.fModEvent.ShrimpLightning);
        ShrimpMachineSFX = soundManager.CreateInstance(soundManager.fModEvent.MachineSFX);
        ShrimpPlasmaBallSFX = soundManager.CreateInstance(soundManager.fModEvent.PlasmaBall);
    }

    public void PlayShrimpLegSFX()
    {
        soundManager.AttachInstanceToGameObject(ShrimpLegSFX,gameObject.transform, rb);

        ShrimpLegSFX.getPlaybackState(out var playBackState);
        if (playBackState.Equals(PLAYBACK_STATE.STOPPED))
            ShrimpLegSFX.start();
    }

    public void StopShrimpLegSFX()
    {
        ShrimpLegSFX.stop(STOP_MODE.ALLOWFADEOUT);
    }

    public void PlayMachineSFX()
    {
        soundManager.AttachInstanceToGameObject(ShrimpMachineSFX, gameObject.transform, rb);

        ShrimpMachineSFX.getPlaybackState(out var playBackState);
        if (playBackState.Equals(PLAYBACK_STATE.STOPPED))
            ShrimpMachineSFX.start();
    }

    public void StopMachineSFX()
    {
        ShrimpMachineSFX.stop(STOP_MODE.ALLOWFADEOUT);
    }

    public void PlayShrimpLightingSFX()
    {
        soundManager.AttachInstanceToGameObject(ShrimpLightningSFX, gameObject.transform, rb);

        ShrimpLightningSFX.getPlaybackState(out var playBackState);
        if (playBackState.Equals(PLAYBACK_STATE.STOPPED))
            ShrimpLightningSFX.start();
    }

    public void StopShrimpLightingSFX()
    {
        ShrimpLightningSFX.stop(STOP_MODE.ALLOWFADEOUT);
    }

    public void PlayShrimpRoar()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        soundManager.PlayOneShot(soundManager.fModEvent.ShrimpRoar, player);
    }

    public void PlayShrimpSmash()
    {
        soundManager.PlayOneShot(soundManager.fModEvent.ShrimpSmash, gameObject);
    }

    public void PlayShrimpThrash()
    {
        soundManager.PlayOneShot(soundManager.fModEvent.ShrimpThrash, gameObject);
    }

    public void PlayShrimpPierce()
    {
        soundManager.PlayOneShot(soundManager.fModEvent.ShrimpPierce, gameObject);
    }

    public void PlayShrimpCrash()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        soundManager.PlayOneShot(soundManager.fModEvent.ShrimpCrash, player);
    }

    public void PlayShrimpDead()
    {
        soundManager.PlayOneShot(soundManager.fModEvent.ShrimpDead, gameObject);
    }

    public void PlayWaterBeam()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        soundManager.PlayOneShot(soundManager.fModEvent.WaterBeam, player);
    }

    public void PlayBubble()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        soundManager.PlayOneShot(soundManager.fModEvent.BubbleShoot, player);
    }

    public void PlayWaterGround(GameObject obj)
    {
        soundManager.PlayOneShot(soundManager.fModEvent.WaterGround, obj);
    }

    public void PlayThunderGround(GameObject obj)
    {
        soundManager.PlayOneShot(soundManager.fModEvent.ThunderGround, obj);
    }

    public void PlayUltimateGround(GameObject obj)
    {
        soundManager.PlayOneShot(soundManager.fModEvent.UltimateGround, obj);
    }

    public void PlayPlasmaBall(GameObject obj, Rigidbody rb)
    {
        soundManager.AttachInstanceToGameObject(ShrimpPlasmaBallSFX, obj.transform, rb);

        ShrimpPlasmaBallSFX.getPlaybackState(out var playBackState);
        if (playBackState.Equals(PLAYBACK_STATE.STOPPED))
            ShrimpPlasmaBallSFX.start();
    }

    public void StopPlasmaBall()
    {
        ShrimpPlasmaBallSFX.stop(STOP_MODE.ALLOWFADEOUT);
    }

    public void PlayShrimpGetHit()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        soundManager.PlayOneShot(soundManager.fModEvent.ShrimpGetHit, player);
    }
}
