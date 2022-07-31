using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void OnClickRTMM() //Return to Mainmenu Button
    {
        SceneManager.LoadScene("Mainmenu",LoadSceneMode.Single);
    }

}
