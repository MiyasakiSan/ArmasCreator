using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

namespace ArmasCreator.UI
{
    public class ChallengeNode : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text headerText;
        [SerializeField]
        private Image imageBG;
        [SerializeField]
        private GameObject checkPass;

        [SerializeField]
        private Sprite selectedSprite;
        [SerializeField]
        private Sprite deSelectedSprite;

        [SerializeField]
        private ChallengeDetailPanelController challengeDetailPanelController;

        private ChallengesPanelController challengePanelController;

        public Action<string> achievementIDAction;

        private string achievementID;

        [SerializeField]
        private Button btn_ChallengeNode;

        public void SetUpButton()
        {
            btn_ChallengeNode.onClick.AddListener(() =>
            {
                challengePanelController.OnDeSelectedAll();
                OnSelectedChallengeNode();
                achievementIDAction.Invoke(achievementID);
            });
            achievementIDAction += challengeDetailPanelController.SetDisplayItem;
        }

        public void OnSelectedChallengeNode()
        {
            imageBG.sprite = selectedSprite;
        }

        public void OnDeSelectedChallengeNode()
        {
            imageBG.sprite = deSelectedSprite;
        }

        public void SetChallengeDetailPanelController(ChallengeDetailPanelController newChallengeDetailPanelController)
        {
            challengeDetailPanelController = newChallengeDetailPanelController;
        }

        public void SetChallengePanelController(ChallengesPanelController newChallengesPanelController)
        {
            challengePanelController = newChallengesPanelController;
        }

        public void SetAchievementID(string newAchievementID)
        {
            achievementID = newAchievementID;
        }

        public void SetHeaderText(string newHeaderText)
        {
            headerText.text = newHeaderText;
        }

        public void SetCheckPass(bool isPass)
        {
            checkPass.SetActive(isPass);
        }
    }
}
