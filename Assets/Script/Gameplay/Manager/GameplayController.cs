using System.Collections;
using System.Collections.Generic;
using ArmasCreator.GameData;
using ArmasCreator.UserData;
using ArmasCreator.Utilities;
using UnityEngine;
using ArmasCreator.GameMode;
using UnityEngine.SceneManagement;
using ArmasCreator.UI;
using ArmasCreator.Gameplay.UI;
using UnityEngine.Rendering.Universal;

namespace ArmasCreator.Gameplay
{
    public class GameplayController : MonoBehaviour
    {
        public enum Gameplays
        {
            none,
            Town,
            Story,
            Challenge,
            PreGame,
            Result
        }

        public Gameplays CurrentGameplays;

        private float currentDamageDelt = 0;

        private float currentStageTime = 0;

        private float currentDamageTaken = 0;

        [SerializeField]
        private ProfilePanelController profilePanelController;

        [SerializeField]
        private OptionPanelController optionPanelController;

        public delegate void OnLoadProgressionChangeCallback(float currentProgress);
        public OnLoadProgressionChangeCallback OnLoadProgressionChange;

        public delegate void OnLoadingCompletedCallback();
        public OnLoadingCompletedCallback OnLoadingCompleted;

        public delegate void OnStatePreGameCallBack();
        public OnStatePreGameCallBack OnStatePreGame;

        public delegate void OnStateStartCallBack();
        public OnStateStartCallBack OnStateStart;

        public delegate void OnStatePlayCallBack();
        public OnStatePlayCallBack OnStatePlay;

        public delegate void OnStateResultCallback();
        public OnStateResultCallback OnStateResult;

        public delegate void OnPlayerDealDamageCallback();
        public OnPlayerDealDamageCallback OnPlayerDealDamage;

        public PlayerGroupController PlayerGroupController { get; private set; }
        //public GameDataManager GameDataManager { get; private set; }
        //public UserDataManager UserDataManager { get; private set; }

        private GameModeController gameModeController;
        private GameDataManager gameDataManager;
        private LoadingPopup loadingPopup;
        private UIPlayerController uiPlayerController;

        private QuestInfo currentQuestInfo;

        [SerializeField]
        private Canvas profileCanvas;

        //TODO : implement player interaction

        public bool Interacable;

        private bool isPreGameFinished;
        private Coroutine PreGameCoroutine;

        private void Awake()
        {
            SharedContext.Instance.Add(this);
            DontDestroyOnLoad(this);

            PlayerGroupController = new PlayerGroupController();

            gameModeController = SharedContext.Instance.Get<GameModeController>();
            gameDataManager = SharedContext.Instance.Get<GameDataManager>();
            loadingPopup = SharedContext.Instance.Get<LoadingPopup>();
            Interacable = true;

            CurrentGameplays = Gameplays.Town;
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.LeftShift) && Input.GetKeyUp(KeyCode.Q) && CurrentGameplays == Gameplays.Challenge)
            {
                EnterGameplayResult();
            }

            if (!Interacable) { return; }

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (profileCanvas.worldCamera == null)
                {
                    var cameraData = Camera.main.GetUniversalAdditionalCameraData();
                    if (cameraData.cameraStack.Count > 0)
                    {
                        profileCanvas.worldCamera = cameraData.cameraStack[0];
                    }
                    else
                    {
                        profileCanvas.worldCamera = Camera.main;
                    }
                }
                if (profilePanelController.Profile.activeSelf)
                {
                    profilePanelController.CloseProfilePanel();
                    Time.timeScale = 1f;
                }
                else if (!profilePanelController.Profile.activeSelf)
                {
                    profilePanelController.OpenProfilePanel();
                    Time.timeScale = 0f;
                }
            }
        }

        public void EnterChallengeStage(QuestInfo questInfo)
        {
            CurrentGameplays = Gameplays.Challenge;

            currentQuestInfo = questInfo;

            loadingPopup.LoadSceneAsync(questInfo.SceneName);
            loadingPopup.OnLoadingSceneFinished += EnterPreGameStage;
        }

        private void EnterPreGameStage()
        {
            CurrentGameplays = Gameplays.PreGame;
            isPreGameFinished = false;

            uiPlayerController = SharedContext.Instance.Get<UIPlayerController>();

            Debug.Assert(uiPlayerController != null, "UIPlayerController is null");
            uiPlayerController.Hide();

            loadingPopup.OnLoadingSceneFinished -= EnterPreGameStage;

            if(PreGameCoroutine != null)
            {
                StopCoroutine(PreGameCoroutine);
                PreGameCoroutine = null;
            }

            PreGameCoroutine = StartCoroutine(PreGameEnumerator());
        }

        private IEnumerator PreGameEnumerator()
        {
            yield return new WaitUntil(() => isPreGameFinished);

            CurrentGameplays = Gameplays.Challenge;
            loadingPopup.FadeBlack();
        }

        public void PreGameFinish()
        {
            isPreGameFinished = true;

            uiPlayerController.Show();

            Debug.Log("===================== Pre Game Finish ==========================");
        }

        public void EnterGameplayResult()
        {
            //TODO : Add something to player
            CurrentGameplays = Gameplays.Result;
            Debug.Log("Show Result");
            uiPlayerController.Hide();

            StartCoroutine(ShowGameResultCoroutine());
        }

        private IEnumerator ShowGameResultCoroutine()
        {
            OnStateResult?.Invoke();
            yield return new WaitUntil(() => ResultPanelController.IsResultSequenceFinished);

            Dispose();
            loadingPopup.LoadSceneAsync("Town");
        }

        public void ReturnToMainmenu()
        {
            Dispose();
            loadingPopup.LoadSceneAsync("Mainmenu");
            Time.timeScale = 1f;
        }

        public void UpdatePlayerDamageDelt(float damage)
        {
            currentDamageDelt += damage;

            OnPlayerDealDamage?.Invoke();
        }

        private void Dispose()
        {
            SharedContext.Instance.Remove(this);
            currentQuestInfo = null;
            optionPanelController.RemoveListener();
            Interacable = true;
            Destroy(this.gameObject);
        }
    }
}
    
