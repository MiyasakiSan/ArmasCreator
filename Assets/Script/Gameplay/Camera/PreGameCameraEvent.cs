using ArmasCreator.Gameplay;
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
    private float timeToSkipTo;

    private void Awake()
    {
        gameplayController = SharedContext.Instance.Get<GameplayController>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && gameplayController.CurrentGameplays == Gameplays.PreGame)
        {
            currentDirector.time = timeToSkipTo;
        }
    }

    public void PreGameFinishEvent()
    {
        Debug.Assert(gameplayController != null, "gameplayController is null");
        gameplayController.PreGameFinish();
    }
}
