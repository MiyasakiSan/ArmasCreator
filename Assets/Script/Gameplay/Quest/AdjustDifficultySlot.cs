using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace ArmasCreator.Gameplay.UI
{
    public class AdjustDifficultySlot : MonoBehaviour
    {
        [SerializeField]
        private GameObject selectedBGGameObject;
        [SerializeField]
        private CanvasGroup adjustSlotCanvasGroup;

        [SerializeField]
        private Button adjustButton;
        public Button AdjustButton => adjustButton;

        [SerializeField]
        private QuestPanelController questPanelController;

        public Action<float> DifficultyLevel;
        
        public enum SlotType
        {
            Attack,
            Health,
            Speed
        }

        private bool isCanEdit = true;

        [SerializeField]
        private SlotType slotType;

        public float Multiplier;

        private void Awake()
        {
            adjustButton.onClick.AddListener(() =>
            {
                questPanelController.OnClickAdjustSlot(slotType);
                OnSelectedAdjustSlot();
            });
        }

        public void OnSelectedAdjustSlot()
        {
            if (isCanEdit == true)
            {
                selectedBGGameObject.SetActive(true);
                adjustSlotCanvasGroup.alpha = 1;
                DifficultyLevel?.Invoke(Multiplier);
            }
        }

        public void OnDeselectedAdjustSlot()
        {
            if (isCanEdit == true)
            {
                selectedBGGameObject.SetActive(false);
                adjustSlotCanvasGroup.alpha = 0.5f;
            }
        }

        public void SetIsCanEdit(bool isCanEdit)
        {
            this.isCanEdit = isCanEdit;
        }
    }
}
