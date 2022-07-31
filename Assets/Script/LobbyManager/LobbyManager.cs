using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] public Button StartGameButton;
    public string sceneName;

    public void onclickStartGame()
    {
        NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
    public void onclickBack()
    {
        Destroy(NetworkManager.Singleton.gameObject);
        SceneManager.LoadScene("Mainmenu2");
    }
}
