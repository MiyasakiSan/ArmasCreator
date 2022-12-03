using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArmasCreator.GameMode;
using ArmasCreator.Utilities;
using Unity.Netcode;
using TMPro;
using ArmasCreator.UserData;

namespace ArmasCreator.Gameplay.UI
{
    public class UIPlayerController : NetworkBehaviour
    {
        [SerializeField]
        private UIStatControl playerStatUI;
        public UIStatControl PlayerStatUI => playerStatUI;

        [SerializeField]
        private TextMeshProUGUI playerNameText;
        public TextMeshProUGUI PlayerNameText => playerNameText;

        [SerializeField]
        private GameObject otherPlayerCanvas;
        public GameObject OtherPlayerCanvas => otherPlayerCanvas;

        [SerializeField]
        private GameObject content;

        [SerializeField]
        private UIItemBar itemBar;

        private GameModeController gameModeController;
        private UserDataManager userDataManager;
        private GameplayController gameplayController;

        private bool IsSinglePlayer => gameModeController.IsSinglePlayerMode;

        private void Awake()
        {
            SharedContext.Instance.Add(this);
            gameModeController = SharedContext.Instance.Get<GameModeController>();
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
        }

        private void Start()
        {
            playerNameText.text = PlayerPrefs.GetString("PName");

            itemBar.Init(userDataManager);

            gameplayController = SharedContext.Instance.Get<GameplayController>();

            if (gameplayController.CurrentGameplays == GameplayController.Gameplays.Town)
            {
                itemBar.Hide();
            }
        }

        public void Show()
        {
            content.SetActive(true);
        }

        public void Hide()
        {
            content.SetActive(false);
        }

        private void OnDestroy()
        {
            SharedContext.Instance.Remove(this);
        }
    }
}


