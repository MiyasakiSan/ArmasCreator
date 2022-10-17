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

        private void Awake()
        {
            gameplayController = SharedContext.Instance.Get<GameplayController>();
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
        }

        void Start()
        {

        }

        public void LoadPresets(string MapId)
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


    }
}
    
