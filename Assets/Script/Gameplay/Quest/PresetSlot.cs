using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace ArmasCreator.Gameplay.UI
{
    public class PresetSlot : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup presetSlotCanvasGroup;
        [SerializeField]
        private Button presetButton;
        [SerializeField]
        private QuestPanelController questPanelController;

        public Action<PresetSlot> PresetInfo;

        public QuestInfo questInfo;

        public enum PresetType
        {
            Challenge,
            LocalPreset
        }

        [SerializeField]
        private PresetType presetType;

        public PresetType Type => presetType;

        private void Awake()
        {
            presetButton.onClick.AddListener(() =>
            {
                OnSelectedPresetSlot();
                questPanelController.OnClickPreset(presetType);
            });
        }

        public void OnSelectedPresetSlot()
        {
            presetSlotCanvasGroup.alpha = 1;
            PresetInfo?.Invoke(this);
        }

        public void OnDeselectedAdjustSlot()
        {
            presetSlotCanvasGroup.alpha = 0.5f;
        }
    }
}
