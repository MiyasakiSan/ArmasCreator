using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArmasCreator.GameMode;
using ArmasCreator.Utilities;
using Unity.Netcode;
using TMPro;

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

        private GameModeController gameModeController;

        private bool IsSinglePlayer => gameModeController.IsSinglePlayerMode;

        private void Awake()
        {
            SharedContext.Instance.Add(this);
            gameModeController = SharedContext.Instance.Get<GameModeController>();
        }

        private void Start()
        {
            playerNameText.text = PlayerPrefs.GetString("PName");
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


