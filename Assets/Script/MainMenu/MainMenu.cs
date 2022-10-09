using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TMP_Text Name;
    public GameObject EnterNameUI;
    public void ExitEnterName()
    {
        EnterNameUI.SetActive(false);
    }
    public void OnClickStartGame()
    {
        EnterNameUI.SetActive(true);
    }
    public void OnClickEnter()
    {
        PlayerPrefs.SetString("PName", Name.text);
        SceneManager.LoadScene("Town", LoadSceneMode.Single);
    }
    public void OnClickEncyclopedia()
    {
        SceneManager.LoadScene("Encyclopedia", LoadSceneMode.Single);
    }
}
