using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ArmasCreator.Utilities;
using ArmasCreator.UserData;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TMP_Text Name;
    public GameObject EnterNameUI;

    private UserDataManager userDataManager;

    private void Awake()
    {
        userDataManager = SharedContext.Instance.Get<UserDataManager>();
    }

    public void ExitEnterName()
    {
        EnterNameUI.SetActive(false);
    }
    public void OnClickStartGame()
    {
        if (!string.IsNullOrEmpty(userDataManager.UserData.UserID))
        {
            SceneManager.LoadScene("Town", LoadSceneMode.Single);
            return;
        }

        EnterNameUI.SetActive(true);
    }
    public void OnClickEnter()
    {
        PlayerPrefs.SetString("PName", Name.text);
        userDataManager.UserData.SetUserID(Name.text);
        SceneManager.LoadScene("Town", LoadSceneMode.Single);
    }
    public void OnClickEncyclopedia()
    {
        SceneManager.LoadScene("Encyclopedia", LoadSceneMode.Single);
    }
}
