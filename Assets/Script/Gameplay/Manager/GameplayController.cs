using System.Collections;
using System.Collections.Generic;
using ArmasCreator.GameData;
using ArmasCreator.UserData;
using ArmasCreator.Utilities;
using UnityEngine;
using ArmasCreator.GameMode;
using UnityEngine.SceneManagement;

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
            Result
        }

        public Gameplays CurrentGameplays;

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

        public PlayerGroupController PlayerGroupController { get; private set; }
        //public GameDataManager GameDataManager { get; private set; }
        //public UserDataManager UserDataManager { get; private set; }

        private GameModeController gameModeController;
        private GameDataManager gameDataManager;

        private QuestInfo currentQuestInfo;

        private void Awake()
        {
            SharedContext.Instance.Add(this);
            DontDestroyOnLoad(this);

            PlayerGroupController = new PlayerGroupController();

            gameModeController = SharedContext.Instance.Get<GameModeController>();
            gameDataManager = SharedContext.Instance.Get<GameDataManager>();
        }

        void Start()
        {
            CurrentGameplays = Gameplays.Town;
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Q) && CurrentGameplays == Gameplays.Challenge)
            {
                EnterGameplayResult();
            }
        }

        public void EnterChallengeStage(QuestInfo questInfo)
        {
            CurrentGameplays = Gameplays.Challenge;

            currentQuestInfo = questInfo;

            SceneManager.LoadScene(questInfo.SceneName, LoadSceneMode.Single);
        }

        public void EnterGameplayResult()
        {
            //TODO : Add something to player
            CurrentGameplays = Gameplays.Result;
            Debug.Log("Show Result");

            StartCoroutine(ShowGameResultCoroutine());
        }

        private IEnumerator ShowGameResultCoroutine()
        {

            yield return new WaitForSeconds(3f);

            Dispose();
            SceneManager.LoadScene("Town", LoadSceneMode.Single);
        }

        private void Dispose()
        {
            SharedContext.Instance.Remove(this);
            currentQuestInfo = null;
            Destroy(this.gameObject);
        }


    }
}
    
