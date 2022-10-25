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

        public Action PresetId;

        public enum PresetType
        {
            Challenge,
            LocalPreset
        }

        [SerializeField]
        private PresetType presetType;

        private void Awake()
        {
            presetButton.onClick.AddListener(() =>
            {
                questPanelController.OnClickPreset(presetType);
                OnSelectedPresetSlot();
            });
        }

        public void OnSelectedPresetSlot()
        {
            presetSlotCanvasGroup.alpha = 1;
            PresetId?.Invoke();
        }

        public void OnDeselectedAdjustSlot()
        {
            presetSlotCanvasGroup.alpha = 0.5f;
        }
    }
}
