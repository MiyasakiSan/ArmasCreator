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
        [SerializeField]
        private Button backButton;

        [Header("ProfileContent")]
        [SerializeField]
        private GameObject profileContent;

        [Header("Animator")]
        [SerializeField]
        private Animator profileAnimator;


        public GameObject Profile => profileContent;

        private void Awake()
        {
            backButton.onClick.AddListener(() =>
            {
                CloseProfilePanel();
            });
            btn_Item.onClick.AddListener(() =>
            {
                profileAnimator.SetTrigger("FadeOutCD");
                profileAnimator.SetTrigger("FadeOut");
                profileAnimator.SetTrigger("FadeInItem");

            });

            btn_Equipment.onClick.AddListener(() =>
            {
                profileAnimator.SetTrigger("FadeOutCD");
                profileAnimator.SetTrigger("FadeOut");
                profileAnimator.SetTrigger("FadeInEquipment");
            });

            btn_Quest.onClick.AddListener(() =>
            {
                profileAnimator.SetTrigger("FadeOutCD");
                profileAnimator.SetTrigger("FadeOut");
                profileAnimator.SetTrigger("FadeInQuest");
            });

            btn_Challenges.onClick.AddListener(() =>
            {
                profileAnimator.SetTrigger("FadeOut");
                profileAnimator.SetTrigger("FadeInChallenge");
            });

            btn_Option.onClick.AddListener(() =>
            {
                profileAnimator.SetTrigger("FadeOutCD");
                profileAnimator.SetTrigger("FadeOut");
                profileAnimator.SetTrigger("FadeInOption");
            });
        }

        public void OpenProfilePanel()
        {
            DeActivateAllContent();
            optionPanelController.ShowOptionPanel();
            top_btn_Option.OnSelected();
            profileAnimator.SetTrigger("FadeIn");
            profileAnimator.SetTrigger("FadeInOption");
        }

        public void CloseProfilePanel()
        {
            profileAnimator.SetTrigger("FadeOutCD");
            profileAnimator.SetTrigger("FadeOut");
            profileAnimator.SetTrigger("FadeOutProfile");
            DeActivateAllContent();
            Time.timeScale = 1f;
        }

        public void ActiveChallengeDetailAnim()
        {
            profileAnimator.SetTrigger("FadeInCD");
        }

        public void ResetFadeOutCDTrigger()
        {
            profileAnimator.ResetTrigger("FadeOutCD");
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

        public void SetUpItem()
        {
            itemsPanelController.SetUpItemPanel();
            top_btn_Item.OnSelected();
        }

        public void SetUpEquipment()
        {
            equipmentPanelController.SetUpEquipmentPanel();
            top_btn_Equipment.OnSelected();
        }

        public void SetUpQuest()
        {
            profileQuestPanelController.SetUpQuestPanel();
            top_btn_Quest.OnSelected();
        }

        public void SetUpChallenge()
        {
            challengesPanelController.SetUpChallengePanel();
            top_btn_Challenges.OnSelected();
        }

        public void SetUpOption()
        {
            optionPanelController.ShowOptionPanel();
            top_btn_Option.OnSelected();
        }
    }
}