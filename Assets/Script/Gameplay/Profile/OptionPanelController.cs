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
        private Button btn_ToTitle;
        [SerializeField]
        private Button btn_Setting;
        [SerializeField]
        private ProfilePanelController profilePanelController;

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
            btn_Setting.onClick.AddListener(() =>
            {
                profilePanelController.ActiveSettingDetailAnim();
            });
        }

        public void RemoveListener()
        {
            btn_ToTitle.onClick.RemoveAllListeners();
            btn_Setting.onClick.RemoveAllListeners();
        }
    }
}
