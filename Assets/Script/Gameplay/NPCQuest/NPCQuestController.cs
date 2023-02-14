using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArmasCreator.GameData;
using ArmasCreator.UserData;
using ArmasCreator.Utilities;
using UnityEngine.UI;
using TMPro;

namespace ArmasCreator.UI
{
    public class NPCQuestController : MonoBehaviour
    {
        [SerializeField]
        private Button mainQuestButton;

        [SerializeField]
        private Button subQuestButton;

        [SerializeField]
        private GameObject npcQuestNode;

        private GameDataManager gameDataManager;

        private UserDataManager userDataManager;

        private void Awake()
        {
            gameDataManager = SharedContext.Instance.Get<GameDataManager>();
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
        }

        private void Start()
        {
            TopPanelButton mainQuestButton_TopPanelButton = mainQuestButton.GetComponent<TopPanelButton>();
            mainQuestButton.onClick.AddListener(() => 
            {
                DeselectedAllTopButton();
                mainQuestButton_TopPanelButton.OnSelected();
            });
            TopPanelButton subQuestButton_TopPanelButton = subQuestButton.GetComponent<TopPanelButton>();
            subQuestButton.onClick.AddListener(() =>
            {
                DeselectedAllTopButton();
                subQuestButton_TopPanelButton.OnSelected();
            });
        }

        public void DeselectedAllTopButton()
        {
            mainQuestButton.GetComponent<TopPanelButton>().OnDeSelected();
            subQuestButton.GetComponent<TopPanelButton>().OnDeSelected();
        }

        public void OnClickMainQuestButton()
        {

        }

        public void OnClickSubQuestButton()
        {

        }
    }
}
