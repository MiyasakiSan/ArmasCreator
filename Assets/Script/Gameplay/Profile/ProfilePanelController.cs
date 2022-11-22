using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;

namespace ArmasCreator.UI
{
    public class ProfilePanelController : MonoBehaviour
    {
        [Header("PanelController")]
        [SerializeField]
        private OptionPanelController optionPanelController;
        [SerializeField]
        private ChallengesPanelController challengesPanelController;
        [SerializeField]
        private ProfileQuestPanelController profileQuestPanelController;
        [SerializeField]
        private EquipmentPanelController equipmentPanelController;
        [SerializeField]
        private ItemsPanelController itemsPanelController;

        [Header("Button")]
        [SerializeField]
        private Button btn_Item;
        [SerializeField]
        private Button btn_Equipment;
        [SerializeField]
        private Button btn_Quest;
        [SerializeField]
        private Button btn_Challenges;
        [SerializeField]
        private Button btn_Option;

        [Header("TopPanelButton")]
        [SerializeField]
        private TopPanelButton top_btn_Item;
        [SerializeField]
        private TopPanelButton top_btn_Equipment;
        [SerializeField]
        private TopPanelButton top_btn_Quest;
        [SerializeField]
        private TopPanelButton top_btn_Challenges;
        [SerializeField]
        private TopPanelButton top_btn_Option;

        private void Awake()
        {
            btn_Item.onClick.AddListener(() =>
            {
                DeActivateAllContent();
                itemsPanelController.SetUpItemPanel();
                top_btn_Item.OnSelected();
            });

            btn_Equipment.onClick.AddListener(() =>
            {
                DeActivateAllContent();
                equipmentPanelController.SetUpEquipmentPanel();
                top_btn_Equipment.OnSelected();
            });

            btn_Quest.onClick.AddListener(() =>
            {
                DeActivateAllContent();
                profileQuestPanelController.SetUpQuestPanel();
                top_btn_Quest.OnSelected();
            });

            btn_Challenges.onClick.AddListener(() =>
            {
                DeActivateAllContent();
                challengesPanelController.SetUpChallengePanel();
                top_btn_Challenges.OnSelected();
            });

            btn_Option.onClick.AddListener(() =>
            {
                DeActivateAllContent();
                optionPanelController.ShowOptionPanel();
                top_btn_Option.OnSelected();
            });
        }

        public void OpenProfilePanel()
        {
            DeActivateAllContent();
            optionPanelController.ShowOptionPanel();
            top_btn_Option.OnSelected();
        }

        public void CloseProfilePanel()
        {
            DeActivateAllContent();
        }

        public void DeActivateAllContent()
        {
            itemsPanelController.DeactiveItemPanel();
            equipmentPanelController.DeactiveEquipmentPanel();
            challengesPanelController.DeactiveChallengePanel();
            optionPanelController.DeactivateOptionPanel();
            profileQuestPanelController.DeactivateQuestPanel();
            top_btn_Item.OnDeSelected();
            top_btn_Equipment.OnDeSelected();
            top_btn_Challenges.OnDeSelected();
            top_btn_Option.OnDeSelected();
            top_btn_Quest.OnDeSelected();
        }
    }
}