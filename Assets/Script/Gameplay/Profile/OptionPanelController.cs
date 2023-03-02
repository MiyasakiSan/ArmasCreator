using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ArmasCreator.Gameplay;
using ArmasCreator.Utilities;

namespace ArmasCreator.UI
{
    public class OptionPanelController : MonoBehaviour
    {
        [SerializeField]
        private GameObject mainContent;
        [SerializeField]
        private Button btn_ToTitle;

        private GameplayController gameplayController;

        private void Awake()
        {
            gameplayController = SharedContext.Instance.Get<GameplayController>();
        }
        private void Start()
        {
            btn_ToTitle.onClick.AddListener(() =>
            {
                gameplayController.ReturnToMainmenu();
            });
        }

        public void RemoveListener()
        {
            btn_ToTitle.onClick.RemoveAllListeners();
        }

        public void ShowOptionPanel()
        {
            mainContent.SetActive(true);
        }

        public void DeactivateOptionPanel()
        {
            mainContent.SetActive(false);
        }
    }
}
