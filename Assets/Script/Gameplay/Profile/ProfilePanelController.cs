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
                profileAnimator.SetTrigger("FadeOutSD");
                profileAnimator.SetTrigger("FadeOutCD");
                profileAnimator.SetTrigger("FadeOut");
                profileAnimator.SetTrigger("FadeInItem");
                AllInteractableTrue();
                btn_Item.interactable = false;

            });

            btn_Equipment.onClick.AddListener(() =>
            {
                profileAnimator.SetTrigger("FadeOutSD");
                profileAnimator.SetTrigger("FadeOutCD");
                profileAnimator.SetTrigger("FadeOut");
                profileAnimator.SetTrigger("FadeInEquipment");
                AllInteractableTrue();
                btn_Equipment.interactable = false;
            });

            btn_Quest.onClick.AddListener(() =>
            {
                profileAnimator.SetTrigger("FadeOutCD");
                profileAnimator.SetTrigger("FadeOut");
                profileAnimator.SetTrigger("FadeInQuest");
                AllInteractableTrue();
                btn_Quest.interactable = false;
            });

            btn_Challenges.onClick.AddListener(() =>
            {
                profileAnimator.SetTrigger("FadeOutSD");
                profileAnimator.SetTrigger("FadeOut");
                profileAnimator.SetTrigger("FadeInChallenge");
                AllInteractableTrue();
                btn_Challenges.interactable = false;
            });

            btn_Option.onClick.AddListener(() =>
            {
                profileAnimator.SetTrigger("FadeOutCD");
                profileAnimator.SetTrigger("FadeOut");
                profileAnimator.SetTrigger("FadeInOption");
                AllInteractableTrue();
                btn_Option.interactable = false;
            });
        }

        public void OpenProfilePanel()
        {
            DeActivateAllContent();
            AllInteractableTrue();
            btn_Option.interactable = false;
            top_btn_Option.OnSelected();
            profileAnimator.SetTrigger("FadeIn");
            profileAnimator.SetTrigger("FadeInOption");
        }

        public void CloseProfilePanel()
        {
            profileAnimator.SetTrigger("FadeOutSD");
            profileAnimator.SetTrigger("FadeOutCD");
            profileAnimator.SetTrigger("FadeOut");
            profileAnimator.SetTrigger("FadeOutProfile");
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

        public void ActiveSettingDetailAnim()
        {
            profileAnimator.SetTrigger("FadeInSD");
        }

        public void DeactiveSettingDetailAnim()
        {
            profileAnimator.SetTrigger("FadeOutSD");
        }

        public void ResetTrigger(string TrigggerName)
        {
            profileAnimator.ResetTrigger(TrigggerName);
        }

        public void DeActivateAllContent()
        {
            itemsPanelController.DeactiveItemPanel();
            equipmentPanelController.DeactiveEquipmentPanel();
            challengesPanelController.DeactiveChallengePanel();
            profileQuestPanelController.DeactivateQuestPanel();
            top_btn_Item.OnDeSelected();
            top_btn_Equipment.OnDeSelected();
            top_btn_Challenges.OnDeSelected();
            top_btn_Option.OnDeSelected();
            top_btn_Quest.OnDeSelected();
        }

        public void AllInteractableTrue()
        {
            btn_Item.interactable = true;
            btn_Option.interactable = true;
            btn_Equipment.interactable = true;
            btn_Quest.interactable = true;
            btn_Challenges.interactable = true;
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
            top_btn_Option.OnSelected();
        }
    }
}