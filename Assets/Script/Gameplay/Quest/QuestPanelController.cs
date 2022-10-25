using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        // Start is called before the first frame update
        void Start()
        {
            DeselectAllBanner();

            //TODO: Load Json data
            SetMapId("JsonData");
        }

        // Update is called once per frame
        void Update()
        {

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

            DeselectAllPresetSlot(presetSlot);
        }

        public void DeselectAllPresetSlot(List<PresetSlot> listPresetSlot)
        {
            foreach (PresetSlot presetSlot in listPresetSlot)
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

        public void SetMapId(string mapId)
        {
            foreach (BannerSlot bannerSlot in bannerList)
            {
                bannerSlot.SetMapId(mapId);
            }
        }
    }
}

