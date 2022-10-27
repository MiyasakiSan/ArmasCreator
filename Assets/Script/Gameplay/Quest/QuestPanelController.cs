using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArmasCreator.Gameplay;
using ArmasCreator.GameData;
using ArmasCreator.UserData;
using ArmasCreator.Utilities;
using ArmasCreator.GameMode;
using System;

namespace ArmasCreator.Gameplay.UI
{
    public class QuestPanelController : MonoBehaviour
    {
        [Header("Banner List")]
        [SerializeField]
        private List<BannerSlot> bannerList;

        [Header("Attack")]
        [SerializeField]
        private List<AdjustDifficultySlot> attackDifficultySlot;
        [Header("Health")]
        [SerializeField]
        private List<AdjustDifficultySlot> healthDifficultySlot;
        [Header("Speed")]
        [SerializeField]
        private List<AdjustDifficultySlot> speedDifficultySlot;

        [Header("PresetSlot")]
        [SerializeField]
        private List<PresetSlot> presetSlot;

        [SerializeField]
        private OnTriggerEvent questBoardCollider;

        private GameDataManager gameDataManager;

        private UserDataManager userDataManager;

        private GameplayController gameplayController;

        private List<string> mapIds;
        private const string constructionSiteId = "cs-cm";
        private const string parkId = "pk-cm";

        private float atkMultiplier;
        private float speedMultiplier;
        private float hpMultiplier;

        private PresetSlot selectedPresetSlot;
        private string selectedMapId;

        private void Awake()
        {
            gameplayController = SharedContext.Instance.Get<GameplayController>();
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
            gameDataManager = SharedContext.Instance.Get<GameDataManager>();
        }

        // Start is called before the first frame update
        void Start()
        {
            DeselectAllBanner();

            LoadChallengeModeConfig();
            //TODO: Load Json data
        }

        private void LoadChallengeModeConfig()
        {
            gameDataManager.GetAllChallengeMapId(out List<string> mapIds);

            this.mapIds = mapIds;

            SetBannerMapId(mapIds);
            SetupDifficultySlot();
        }

        public void OnClickBanner()
        {
            DeselectAllBanner();
        }

        public void OnClickAdjustSlot(AdjustDifficultySlot.SlotType slotType)
        {
            if(slotType == AdjustDifficultySlot.SlotType.Attack)
            {
                DeselectAllAdjustSlot(attackDifficultySlot);
            }
            else if(slotType == AdjustDifficultySlot.SlotType.Health)
            {
                DeselectAllAdjustSlot(healthDifficultySlot);
            }
            else if(slotType == AdjustDifficultySlot.SlotType.Speed)
            {
                DeselectAllAdjustSlot(speedDifficultySlot);
            }
        }

        public void OnClickPreset(PresetSlot.PresetType presetType)
        {
            if(presetType == PresetSlot.PresetType.Challenge)
            {
                SetAllAdjustSlotIsCanEdit(false);
            }
            else if(presetType == PresetSlot.PresetType.LocalPreset)
            {
                SetAllAdjustSlotIsCanEdit(true);
            }
        }

        public void DeselectAllPresetSlot()
        {
            foreach (PresetSlot presetSlot in presetSlot)
            {
                presetSlot.OnDeselectedAdjustSlot();
            }
        }

        public void SetAllAdjustSlotIsCanEdit(bool isCanEdit)
        {
            foreach (AdjustDifficultySlot adjustSlot in attackDifficultySlot)
            {
                adjustSlot.SetIsCanEdit(isCanEdit);
            }

            foreach (AdjustDifficultySlot adjustSlot in healthDifficultySlot)
            {
                adjustSlot.SetIsCanEdit(isCanEdit);
            }

            foreach (AdjustDifficultySlot adjustSlot in speedDifficultySlot)
            {
                adjustSlot.SetIsCanEdit(isCanEdit);
            }
        }

        public void DeselectAllAdjustSlot(List<AdjustDifficultySlot> listAdjustDifficultySlot)
        {
            foreach (AdjustDifficultySlot adjustSlot in listAdjustDifficultySlot)
            {
                adjustSlot.OnDeselectedAdjustSlot();
            }
        }

        public void DeselectAllBanner()
        {
            foreach (BannerSlot bannerSlot in bannerList)
            {
                bannerSlot.OnDeselectedBanner();
            }
        }

        private void SetBannerMapId(List<string> mapIds)
        {
            for(int i = 0; i < bannerList.Count; i++)
            {
                bannerList[i].SetMapId(mapIds[i]);
                bannerList[i].BannerMapId += SetPresetFromMapId;
            }
        }

        private void SetupDifficultySlot()
        {
            foreach(var slot in attackDifficultySlot)
            {
                slot.DifficultyLevel += SetCurrentATKMultiplier;
            }

            foreach (var slot in healthDifficultySlot)
            {
                slot.DifficultyLevel += SetCurrentHpMultiplier;
            }

            foreach (var slot in speedDifficultySlot)
            {
                slot.DifficultyLevel += SetCurrentSpeedMultiplier;
            }
        }

        private void SetPresetFromMapId(string mapId)
        {
            gameDataManager.GetAllChallengeInfoFromMapId(mapId, out List<ChallengeModeModel> challengeInfoList);
            userDataManager.UserData.UserDataQuestPreset.GetAllSavePresetFromMapId(mapId, out List<QuestInfo> questInfoList);

            int localPrestIndex = 0;

            for(int i =0; i< presetSlot.Count; i++)
            {
                if(presetSlot[i].Type == PresetSlot.PresetType.Challenge)
                {
                    var challengeInfo = challengeInfoList[i];
                    var questInfo = new QuestInfo(challengeInfo.ChallengeID,
                                                  challengeInfo.SceneName,
                                                  challengeInfo.DefaultAtk,
                                                  challengeInfo.DefaultSpeed,
                                                  challengeInfo.DefaultHp,
                                                  challengeInfo.Duration);
                    presetSlot[i].questInfo = questInfo;
                }
                else
                {
                    if(questInfoList.Count > 0)
                    {
                        var questInfo = questInfoList[localPrestIndex];
                        presetSlot[i].questInfo = questInfo;
                    }
                    else
                    {
                        string presetId = $"{mapId}-lp-0{localPrestIndex + 1}";
                        var questInfo = new QuestInfo(presetId, challengeInfoList[0].SceneName, 1.0f, 1.0f, 1.0f, 1800);
                        presetSlot[i].questInfo = questInfo;
                    }
                }

                presetSlot[i].PresetInfo += SetCurrentPreset;
            }
        }

        private void SetCurrentPreset(PresetSlot preset)
        {
            selectedPresetSlot = preset;

            var questInfo = preset.questInfo;

            var atkSlot = attackDifficultySlot.Find(x => x.Multiplier == questInfo.InitATK);
            var hpSlot = healthDifficultySlot.Find(x => x.Multiplier == questInfo.InitHp);
            var spdSlot = speedDifficultySlot.Find(x => x.Multiplier == questInfo.InitSpeed);

            atkSlot.AdjustButton.onClick?.Invoke();
            hpSlot.AdjustButton.onClick?.Invoke();
            spdSlot.AdjustButton.onClick?.Invoke();
        }

        private void SetCurrentATKMultiplier(float multiplier)
        {
            atkMultiplier = multiplier;
        }

        private void SetCurrentHpMultiplier(float multiplier)
        {
            hpMultiplier = multiplier;
        }

        private void SetCurrentSpeedMultiplier(float multiplier)
        {
            speedMultiplier = multiplier;
        }

        public void SavePresets()
        {
            bool exist = gameDataManager.TryGetSelectedChallengeModeInfo(selectedPresetSlot.questInfo.PresetId, out ChallengeModeModel challengeInfo);

            if (!exist) 
            {
                Debug.LogError("GameData doesn't contain " + selectedPresetSlot.questInfo.PresetId);
                return; 
            }

            var selectedQuestInfo = selectedPresetSlot.questInfo;


            var questInfo = new QuestInfo(selectedQuestInfo.PresetId, challengeInfo.SceneName, atkMultiplier, speedMultiplier, hpMultiplier, selectedQuestInfo.Duration);

            userDataManager.UserData.UpdateSavePreset(selectedMapId, questInfo);
        }

        public void StartChallengeQuest()
        {
            var selectedQuestInfo = selectedPresetSlot.questInfo;

            string sceneName = selectedQuestInfo.SceneName;

            QuestInfo startQuestInfo = new QuestInfo(selectedMapId, sceneName, atkMultiplier, speedMultiplier, hpMultiplier, selectedQuestInfo.Duration);

            gameplayController.EnterChallengeStage(startQuestInfo);

            //TODO : do something
        }

    }
}

