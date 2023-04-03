using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ArmasCreator.Utilities;
using ArmasCreator.UserData;
using TMPro;
using ArmasCreator.UI;
using FMOD.Studio;

public class MainMenu : MonoBehaviour
{
    public TMP_Text Name;
    public GameObject EnterNameUI;

    private UserDataManager userDataManager;
    private LoadingPopup loadingPopup;
    private SoundManager soundManager;

    EventInstance mainmenuBGM;

    private void Awake()
    {
        userDataManager = SharedContext.Instance.Get<UserDataManager>();
        loadingPopup = SharedContext.Instance.Get<LoadingPopup>();
        soundManager = SharedContext.Instance.Get<SoundManager>();

        mainmenuBGM = soundManager.CreateInstance(soundManager.fModEvent.MainmenuBGM);

        PlayMainmenuSound();
    }

    public void ExitEnterName()
    {
        EnterNameUI.SetActive(false);
    }
    public void OnClickStartGame()
    {
        if (!string.IsNullOrEmpty(userDataManager.UserData.UserID))
        {
            loadingPopup.LoadSceneAsync("Town");
            Cursor.lockState = CursorLockMode.Locked;
            return;
        }
        EnterNameUI.SetActive(true);
    }
    public void OnClickEnter()
    {
        PlayerPrefs.SetString("PName", Name.text);
        userDataManager.UserData.SetUserID(Name.text);
        loadingPopup.LoadSceneAsync("Town");
        StopMainmenuBGM();
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void OnClickEncyclopedia()
    {
        SceneManager.LoadScene("Encyclopedia", LoadSceneMode.Single);
    }

    void PlayMainmenuSound()
    {
        var player = GameObject.FindGameObjectWithTag("Player");

        soundManager.AttachInstanceToGameObject(mainmenuBGM, player.transform, player.GetComponent<Rigidbody>());

        mainmenuBGM.getPlaybackState(out var playBackState);
        if (playBackState.Equals(PLAYBACK_STATE.STOPPED))
            mainmenuBGM.start();
    }

    void StopMainmenuBGM()
    {
        mainmenuBGM.stop(STOP_MODE.ALLOWFADEOUT);
    }

    public void OnExitGame()
    {
        Application.Quit();
    }
}
