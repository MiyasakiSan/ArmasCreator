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

    EventInstance TigerIdleSFX;
    EventInstance TigerFireSFX;

    void Awake()
    {
        soundManager = SharedContext.Instance.Get<SoundManager>();
        rb = GetComponent<Rigidbody>();

        ShrimpLegSFX = soundManager.CreateInstance(soundManager.fModEvent.LegSFX);
        ShrimpLightningSFX = soundManager.CreateInstance(soundManager.fModEvent.ShrimpLightning);
        ShrimpMachineSFX = soundManager.CreateInstance(soundManager.fModEvent.MachineSFX);
        ShrimpPlasmaBallSFX = soundManager.CreateInstance(soundManager.fModEvent.PlasmaBall);

        TigerIdleSFX = soundManager.CreateInstance(soundManager.fModEvent.TigerIdle);
        TigerFireSFX = soundManager.CreateInstance(soundManager.fModEvent.TigerFireIdle);
    }

    #region Shrimp SFX
    public void PlayShrimpLegSFX()
    {
        soundManager.AttachInstanceToGameObject(ShrimpLegSFX, gameObject.transform, rb);

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
    #endregion

    public void TigerRoarFX()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        soundManager.PlayOneShot(soundManager.fModEvent.TigerRoar, player);
    }

    public void TigerBiteSFX()
    {
        soundManager.PlayOneShot(soundManager.fModEvent.TigerBite, gameObject);
    }

    public void TigerBiteComboSFX()
    {
        soundManager.PlayOneShot(soundManager.fModEvent.TigerBiteCombo, gameObject);
    }

    public void TigerClawSFX()
    {
        soundManager.PlayOneShot(soundManager.fModEvent.TigerBiteCombo, gameObject);
    }

    public void TigerCrashSFX()
    {
        soundManager.PlayOneShot(soundManager.fModEvent.TigerBiteCombo, gameObject);
    }

    public void PlayTigerIdleSFX()
    {
        soundManager.AttachInstanceToGameObject(TigerIdleSFX, gameObject.transform, rb);

        TigerIdleSFX.getPlaybackState(out var playBackState);
        if (playBackState.Equals(PLAYBACK_STATE.STOPPED))
            TigerIdleSFX.start();
    }

    public void StopTigerIdleSFX()
    {
        TigerIdleSFX.stop(STOP_MODE.ALLOWFADEOUT);
    }

    public void PlayTigerFireSFX()
    {
        soundManager.AttachInstanceToGameObject(TigerFireSFX, gameObject.transform, rb);

        TigerFireSFX.getPlaybackState(out var playBackState);
        if (playBackState.Equals(PLAYBACK_STATE.STOPPED))
            TigerFireSFX.start();
    }

    public void StopTigerFireSFX()
    {
        TigerFireSFX.stop(STOP_MODE.ALLOWFADEOUT);
    }

    public void PlayTigerSpikeSpawnSFX()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        soundManager.PlayOneShot(soundManager.fModEvent.TigerSpikeSpawn, player);
    }

    public void PlayTigerSpikeGroundFX()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        soundManager.PlayOneShot(soundManager.fModEvent.TigerSpikeSpawn, player);
    }

    public void PlayTigerSpinSFX()
    {
        soundManager.PlayOneShot(soundManager.fModEvent.TigerSpinAttack, gameObject);
    }

    public void PlayTigerSwipeSFX()
    {
        soundManager.PlayOneShot(soundManager.fModEvent.TigerSwipettack, gameObject);
    }

    public void PlayTigerTailBumpSFX()
    {
        soundManager.PlayOneShot(soundManager.fModEvent.TigerTailBump, gameObject);
    }

    public void PlayTigerTailAttackSFX()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        soundManager.PlayOneShot(soundManager.fModEvent.TigerTailAttack, player);
    }

    public void PlayTigerUltimateSFX()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        soundManager.PlayOneShot(soundManager.fModEvent.TigerUltimate, player);
    }

    public void PlayTigerDeadSFX()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        soundManager.PlayOneShot(soundManager.fModEvent.TigerDead, player);
    }

    public void PlayTigerGetHitSFX()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        soundManager.PlayOneShot(soundManager.fModEvent.TigerGetHit, player);
    }
}
