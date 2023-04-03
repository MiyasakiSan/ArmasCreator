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
using UnityEngine.Events;
using FMOD.Studio;

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
            Normal,
            PreGame,
            Result
        }

        public Gameplays CurrentGameplays;

        private float currentDamageDelt = 0;

        private float currentStageTime = 0;

        private float currentDamageTaken = 0;

        private int currentItemUsed = 0;

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

        public delegate void OnStateResultCallback(ResultContainer resultContainer);
        public OnStateResultCallback OnStateResult;

        public delegate void OnPlayerDealDamageCallback();
        public OnPlayerDealDamageCallback OnPlayerDealDamage;

        public PlayerGroupController PlayerGroupController { get; private set; }
        //public GameDataManager GameDataManager { get; private set; }
        //public UserDataManager UserDataManager { get; private set; }

        private GameModeController gameModeController;
        private GameDataManager gameDataManager;
        private UserDataManager userDataManager;
        private LoadingPopup loadingPopup;
        private UIPlayerController uiPlayerController;
        private SoundManager soundManager;
        private TestAnalytics analyticManager;

        private QuestInfo currentQuestInfo;
        public QuestInfo CurrentQuestInfo => currentQuestInfo;

        [SerializeField]
        private Canvas profileCanvas;

        public bool Interacable;

        private bool isPreGameFinished;
        private bool isStageFinished;
        private Coroutine PreGameCoroutine;
        private SubType questEnemyType;
        public SubType EnemyType => questEnemyType;

        public UnityAction<string, int> OnReceivedItem;
        public UnityAction OnReceivedAllItem;

        public UnityAction OnUseItem;

        EventInstance townBGM;
        EventInstance tigerBGM;
        EventInstance shrimpBGM;
        EventInstance winBGM;

        private void Awake()
        {
            SharedContext.Instance.Add(this);
            DontDestroyOnLoad(this);

            PlayerGroupController = new PlayerGroupController();

            gameModeController = SharedContext.Instance.Get<GameModeController>();
            gameDataManager = SharedContext.Instance.Get<GameDataManager>();
            loadingPopup = SharedContext.Instance.Get<LoadingPopup>();
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
            soundManager = SharedContext.Instance.Get<SoundManager>();
            analyticManager = SharedContext.Instance.Get<TestAnalytics>();
            Interacable = true;

            tigerBGM = soundManager.CreateInstance(soundManager.fModEvent.ConstructionSiteBGM);
            shrimpBGM = soundManager.CreateInstance(soundManager.fModEvent.ParkBGM);
            townBGM = soundManager.CreateInstance(soundManager.fModEvent.TownBGM);
            winBGM = soundManager.CreateInstance(soundManager.fModEvent.WinBGM);

            CurrentGameplays = Gameplays.Town;
            PlayTownBGM();
        }

        void Update()
        {
            if (isPreGameFinished && !isStageFinished)
            {
                currentStageTime += Time.deltaTime;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift) && Input.GetKeyUp(KeyCode.Q) && CurrentGameplays == Gameplays.Challenge)
            {
                EnterGameplayResult();
            }

            if (CurrentGameplays == Gameplays.Challenge ||
                CurrentGameplays == Gameplays.Normal) 
            {
                return; 
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
                    SetCursorLock(true);
                    Time.timeScale = 1f;
                }
                else if (!profilePanelController.Profile.activeSelf)
                {
                    profilePanelController.OpenProfilePanel();
                    SetCursorLock(false);
                    Time.timeScale = 0f;
                }
            }
        }

        public void EnterChallengeStage(QuestInfo questInfo)
        {
            StopTownBGM();

            if (questInfo.PresetId == "cs-cm-n" &&
                questInfo.PresetId == "cs-cm-01" &&
                questInfo.PresetId == "cs-cm-02" &&
                questInfo.PresetId == "pk-cm-n" &&
                questInfo.PresetId == "pk-cm-01" &&
                questInfo.PresetId == "pk-cm-02")
            {
                CurrentGameplays = Gameplays.Challenge;
            }
            else
            {
                CurrentGameplays = Gameplays.Normal;
            }

            currentQuestInfo = questInfo;

            loadingPopup.LoadSceneAsync(questInfo.SceneName);
            loadingPopup.OnLoadingSceneFinished += EnterPreGameStage;

            if (questInfo.SceneName == "ConstructionSite")
            {
                questEnemyType = SubType.Lion;
            }
            else
            {
                questEnemyType = SubType.Shrimp;
            }
        }

        private void EnterPreGameStage()
        {
            CurrentGameplays = Gameplays.PreGame;
            isPreGameFinished = false;
            isStageFinished = false;
            SetCursorLock(true);
            PlayMonsterBGM();

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
            isStageFinished = true;
            Debug.Log("Show Result");
            uiPlayerController.Hide();
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRpgMovement>();
            player.canMove = false;
            StopMonsterBGM();

            PlayWinBGM();

            StartCoroutine(ShowGameResultCoroutine());
        }

        private IEnumerator ShowGameResultCoroutine()
        {
            AddRewardToPlayer(2500);
            ResultContainer result = new ResultContainer(currentStageTime, 2500, (int)currentDamageDelt, (int)currentDamageTaken, currentItemUsed);

            UpdateWincount();
            OnStateResult?.Invoke(result);

            yield return new WaitUntil(() => ResultPanelController.IsResultSequenceFinished);

            analyticManager.SendResultToAnalyze(EnemyType, currentQuestInfo.PresetId, result);
            Dispose();
            loadingPopup.LoadSceneAsync("Town");
            SetCursorLock(true);
        }

        private void UpdateWincount()
        {
            if(!PlayerPrefs.HasKey(currentQuestInfo.PresetId))
            {
                PlayerPrefs.SetInt(currentQuestInfo.PresetId, 1);
            }
            else
            {
                var winCount = PlayerPrefs.GetInt(currentQuestInfo.PresetId);
                PlayerPrefs.SetInt(currentQuestInfo.PresetId, winCount + 1);
            }
        }

        private void AddRewardToPlayer(float rewardAmount)
        {
            userDataManager.UserData.UpdateCoin(rewardAmount);

            bool exist = gameDataManager.TryGetAllMonsterPart(questEnemyType, out List<ItemInfoModel> monsterPartInfoList);

            if(!exist) 
            {
                Debug.LogError("WTF is Happening");
                return; 
            }

            foreach(var monsterPartInfo in monsterPartInfoList)
            {
                int amount = UnityEngine.Random.Range(1, 3);
                userDataManager.UserData.UserDataInventory.AddCraftItem(monsterPartInfo.ID, amount);
                OnReceivedItem.Invoke(monsterPartInfo.ID, amount);
            }

            OnReceivedAllItem.Invoke();
        }

        public void ReturnToMainmenu()
        {
            Dispose();
            loadingPopup.LoadSceneAsync("Mainmenu");
            Time.timeScale = 1f;
            SetCursorLock(false);
        }

        public void UpdatePlayerDamageDelt(float damage)
        {
            currentDamageDelt += damage;

            OnPlayerDealDamage?.Invoke();
        }

        public void UpdatePlayerDamageTaken(float damage)
        {
            currentDamageTaken += damage;

            //TODO : invoke needed ?
        }

        public void UpdatePlayerItemUsed(int amount)
        {
            currentItemUsed += amount;

            //TODO : invoke needed ?

            OnUseItem.Invoke();
        }

        public void SetCursorLock(bool isLock)
        {
            if (isLock)
            {
                Cursor.lockState =  CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                //Cursor.visible = isLock;
            }
        }

        private void PlayMonsterBGM()
        {
            Debug.Log("=============== PLAY BGM ==============");

            if(questEnemyType == SubType.Lion)
            {
                PlayTigerBGM();
            }
            else
            {
                PlayShrimpBGM();
            }
        }

        private void StopMonsterBGM()
        {
            Debug.Log("=============== PLAY BGM ==============");

            if (questEnemyType == SubType.Lion)
            {
                StopTigerBGM();
            }
            else
            {
                StopShrimpBGM();
            }
        }

        public void PlayWinBGM()
        {
            var player = GameObject.FindGameObjectWithTag("Player");

            soundManager.AttachInstanceToGameObject(winBGM, player.transform, player.GetComponent<Rigidbody>());

            winBGM.getPlaybackState(out var playBackState);
            if (playBackState.Equals(PLAYBACK_STATE.STOPPED))
                winBGM.start();
        }

        public void StopWinBGM()
        {
            winBGM.stop(STOP_MODE.ALLOWFADEOUT);
        }

        public void PlayTownBGM()
        {
            var player = GameObject.FindGameObjectWithTag("Player");

            soundManager.AttachInstanceToGameObject(townBGM, player.transform, player.GetComponent<Rigidbody>());

            townBGM.getPlaybackState(out var playBackState);
            if (playBackState.Equals(PLAYBACK_STATE.STOPPED))
                townBGM.start();
        }

        public void StopTownBGM()
        {
            townBGM.stop(STOP_MODE.ALLOWFADEOUT);
        }

        public void PlayTigerBGM()
        {
            var player = GameObject.FindGameObjectWithTag("Player");

            soundManager.AttachInstanceToGameObject(tigerBGM, player.transform, player.GetComponent<Rigidbody>());

            tigerBGM.getPlaybackState(out var playBackState);
            if (playBackState.Equals(PLAYBACK_STATE.STOPPED))
                tigerBGM.start();
        }

        public void StopTigerBGM()
        {
            tigerBGM.stop(STOP_MODE.ALLOWFADEOUT);
        }

        public void PlayShrimpBGM()
        {
            var player = GameObject.FindGameObjectWithTag("Player");

            soundManager.AttachInstanceToGameObject(shrimpBGM, player.transform, player.GetComponent<Rigidbody>());

            shrimpBGM.getPlaybackState(out var playBackState);
            if (playBackState.Equals(PLAYBACK_STATE.STOPPED))
                shrimpBGM.start();
        }

        public void StopShrimpBGM()
        {
            shrimpBGM.stop(STOP_MODE.ALLOWFADEOUT);
        }

        private void Dispose()
        {
            SharedContext.Instance.Remove(this);

            StopTigerBGM();
            StopShrimpBGM();
            StopTownBGM();
            StopWinBGM();

            currentQuestInfo = null;
            questEnemyType = SubType.None;
            optionPanelController.RemoveListener();
            Interacable = true;
            Destroy(this.gameObject);
        }
    }

    public class ResultContainer
    {
        public float timeAmount;
        public int rewardAmount;
        public int damageDelt;
        public int damageTaken;
        public int itemUsed;

        public ResultContainer(float timeAmount, int rewardAmount, int damageDelt, int damageTaken, int itemUsed)
        {
            this.timeAmount = timeAmount;
            this.rewardAmount = rewardAmount;
            this.damageDelt = damageDelt;
            this.damageTaken = damageTaken;
            this.itemUsed = itemUsed;
        }
    }
}
    
