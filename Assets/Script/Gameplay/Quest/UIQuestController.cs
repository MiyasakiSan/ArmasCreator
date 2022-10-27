using System.Collections;
using System.Collections.Generic;
using ArmasCreator.GameData;
using ArmasCreator.UserData;
using ArmasCreator.Utilities;
using UnityEngine;
using ArmasCreator.GameMode;

namespace ArmasCreator.Gameplay
{
    public class UIQuestController : MonoBehaviour
    {
        private float atkMultiplier;
        private float speedMultiplier;
        private float hpMultiplier;

        private GameplayController gameplayController;
        private UserDataManager userDataManager;

        private string currentMapId;
        private Dictionary<string, PresetInfo> presetInfos = new Dictionary<string, PresetInfo>();

        [SerializeField]
        private GameObject questCanvas;

        [SerializeField]
        private GameObject playerCanvas;

        [SerializeField]
        private GameObject questVcam;

        private GameDataManager gameDataManager;

        private void Awake()
        {
            gameplayController = SharedContext.Instance.Get<GameplayController>();
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
            gameDataManager = SharedContext.Instance.Get<GameDataManager>();
        }

        void Start()
        {

        }

        public void NewPreset(string MapId)
        {

        }

        public void LoadPresets()
        {

        }

        public void SavePresets(string MapId)
        {

        }

        public void SetSelectedMap(string mapId)
        {
            currentMapId = mapId;
        }

        public void SetATKMultiplier(float value)
        {
            atkMultiplier = value;
        }

        public void SetSpeedMultiplier(float value)
        {
            speedMultiplier = value;
        }

        public void SetHpMultiplier(float value)
        {
            hpMultiplier = value;
        }

        public void StartChallengeQuest()
        {
            bool exist = gameDataManager.TryGetSelectedChallengeModeInfo(currentMapId, out ChallengeModeModel challengeModeInfo);

            if (!exist)
            {
                Debug.LogError("Doesn't have challenge info for map ID :" + currentMapId);
                return;
            }

            var initATK = challengeModeInfo.DefaultAtk * atkMultiplier;
            var initHP = challengeModeInfo.DefaultHp * hpMultiplier;
            var initSPD = challengeModeInfo.DefaultSpeed * speedMultiplier;

            QuestInfo startQuestInfo = new QuestInfo(currentMapId, challengeModeInfo.SceneName, initATK, initSPD, initHP, challengeModeInfo.Duration);

            gameplayController.EnterChallengeStage(startQuestInfo);

            //TODO : do something
        }

        public void ShowQuestCanvas()
        {
            questVcam.SetActive(true);

            questCanvas.SetActive(true);
            playerCanvas.SetActive(false);

            //TODO : Wait some shit
        }

        public void HideQuestCanvas()
        {
            questVcam.SetActive(false);

            questCanvas.SetActive(false);
            playerCanvas.SetActive(true);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) { return; }

            if (Input.GetKeyDown(KeyCode.T))
            {
                ShowQuestCanvas();
            }
            else if(Input.GetKeyDown(KeyCode.Escape))
            {
                HideQuestCanvas();
            }
        }
    }

    public class PresetInfo
    {
        public List<QuestInfo> SavePresets;
    }
}
    
