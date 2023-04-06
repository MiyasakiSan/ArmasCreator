using ArmasCreator.UI;
using ArmasCreator.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    private LoadingPopup loadingPopup;
    void Start()
    {
        loadingPopup = SharedContext.Instance.Get<LoadingPopup>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {

        }
    }

    public void FinishCutscene()
    {
        loadingPopup.LoadSceneAsync("Tutorial");
    }
}
