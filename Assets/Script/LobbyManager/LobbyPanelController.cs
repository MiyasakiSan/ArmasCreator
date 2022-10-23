using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ArmasCreator.Gameplay.UI
{
    public class LobbyPanelController : MonoBehaviour
    {
        [Header("Animator")]
        [SerializeField]
        private Animator animLobbyPanelController;

        [Header("LobbyGameObject")]
        [SerializeField]
        private GameObject createRoomGameObject;

        [SerializeField]
        private GameObject joinRoomGameObject;

        public void Anim_Show()
        {
            animLobbyPanelController.SetTrigger("IsShow");
        }

        public void Anim_Hide()
        {
            animLobbyPanelController.SetTrigger("IsHide");
        }

        public void Anim_ShowLobby()
        {
            animLobbyPanelController.SetTrigger("IsShowFindLobby");
        }

        public void Anim_HidePlayOnline()
        {
            animLobbyPanelController.SetTrigger("IsShow");
            animLobbyPanelController.SetTrigger("IsHide");
        }

        public void ShowCreateLobby()
        {
            animLobbyPanelController.ResetTrigger("IsShowFindLobby");
            Anim_HidePlayOnline();
            createRoomGameObject.SetActive(true);
        }

        public void HideCreateLobby()
        {
            createRoomGameObject.SetActive(false);
        }

        public void ShowFindLobby()
        {
            animLobbyPanelController.ResetTrigger("IsShowFindLobby");
            Anim_HidePlayOnline();
            joinRoomGameObject.SetActive(true);
        }

        public void HideFindLobby()
        {
            joinRoomGameObject.SetActive(false);
        }
    }
}
