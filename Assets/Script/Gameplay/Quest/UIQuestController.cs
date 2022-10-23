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
        private GameObject questVcam;

        private void Awake()
        {
            gameplayController = SharedContext.Instance.Get<GameplayController>();
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
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
            QuestInfo startQuestInfo = new QuestInfo(currentMapId, atkMultiplier, speedMultiplier, hpMultiplier, 1800);

            //TODO : do something
        }

        public void ShowQuestCanvas()
        {
            questVcam.SetActive(true);

            questCanvas.SetActive(true);

            //TODO : Wait some shit
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) { return; }

            if (questCanvas.activeSelf) { return; }

            if (Input.GetKeyDown(KeyCode.T))
            {
                ShowQuestCanvas();
            }
        }
    }

    public class PresetInfo
    {
        public List<QuestInfo> SavePresets;
    }
}
    
