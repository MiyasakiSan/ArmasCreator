using ArmasCreator.Gameplay;
using ArmasCreator.UI;
using ArmasCreator.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using static ArmasCreator.Gameplay.GameplayController;

public class PreGameCameraEvent : MonoBehaviour
{
    private GameplayController gameplayController;

    [SerializeField]
    private PlayableDirector currentDirector;

    [SerializeField]
    private LoadingPopup loadingPopup;

    [SerializeField]
    private float timeToSkipTo;

    [SerializeField]
    private BossNameFadeController bossNameFadeController;

    private void Awake()
    {
        gameplayController = SharedContext.Instance.Get<GameplayController>();

        loadingPopup = SharedContext.Instance.Get<LoadingPopup>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && gameplayController.CurrentGameplays == Gameplays.PreGame)
        {
            currentDirector.time = timeToSkipTo;

            loadingPopup.FadeBlack(false);
        }
    }

    public void PreGameFinishEvent()
    {
        Debug.Assert(gameplayController != null, "gameplayController is null");
        gameplayController.PreGameFinish();
    }

    public void ShowBossName()
    {
        bossNameFadeController.OnStartFade();
    }
}
