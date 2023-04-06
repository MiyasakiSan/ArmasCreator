using ArmasCreator.UI;
using ArmasCreator.Utilities;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    private LoadingPopup loadingPopup;
    private SoundManager soundManager;

    [SerializeField] private Rigidbody rb;
    EventInstance cutsceneBGM;
    void Start()
    {
        loadingPopup = SharedContext.Instance.Get<LoadingPopup>();

        soundManager = SharedContext.Instance.Get<SoundManager>();

        cutsceneBGM = soundManager.CreateInstance(soundManager.fModEvent.CutsceneBGM);

        playCutsceneBGM();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {

        }
    }

    public void FinishCutscene()
    {
        cutsceneBGM.stop(STOP_MODE.ALLOWFADEOUT);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        loadingPopup.LoadSceneAsync("Tutorial");
    }

    public void playCutsceneBGM()
    {
        soundManager.AttachInstanceToGameObject(cutsceneBGM, rb.gameObject.transform, rb);

        cutsceneBGM.getPlaybackState(out var playBackState);
        if (playBackState.Equals(PLAYBACK_STATE.STOPPED))
            cutsceneBGM.start();
    }

    public void tigerRoar()
    {
        soundManager.PlayOneShot(soundManager.fModEvent.TigerRoar, rb.gameObject);
    }

    public void shrimpRoar()
    {
        soundManager.PlayOneShot(soundManager.fModEvent.ShrimpRoar, rb.gameObject);
    }

    public void meteor()
    {
        soundManager.PlayOneShot(soundManager.fModEvent.Meteor, rb.gameObject);
    }

    public void meteorCrash()
    {
        soundManager.PlayOneShot(soundManager.fModEvent.MeteorCrash, rb.gameObject);
    }

    public void meteorWater()
    {
        soundManager.PlayOneShot(soundManager.fModEvent.MeteorWater, rb.gameObject);
    }

    public void meteorCorrupt()
    {
        soundManager.PlayOneShot(soundManager.fModEvent.MeteorCorrupt, rb.gameObject);
    }
}
