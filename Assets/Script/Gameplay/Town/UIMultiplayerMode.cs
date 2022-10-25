using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArmasCreator.Gameplay.UI;

namespace ArmasCreator.Gameplay
{
    public class UIMultiplayerMode : MonoBehaviour
    {
        [SerializeField]
        private GameObject mulitplayerPanel;

        [SerializeField]
        private GameObject multiplayerVcam;

        [SerializeField]
        private GameObject playerCanvas;

        [SerializeField]
        private LobbyPanelController lobbyPanelController;

        void Start()
        {
            multiplayerVcam.SetActive(false);
        }

        public void ShowMulitplayerPanel()
        {
            playerCanvas.SetActive(false);

            multiplayerVcam.SetActive(true);

            //TODO : Wait some shit

            lobbyPanelController.Anim_Show();
        }

        public void HideMultiplayerPanel()
        { 
            multiplayerVcam.SetActive(false);

            //TODO : Wait some shit

            lobbyPanelController.Anim_Hide();

            playerCanvas.SetActive(true);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) { return; }

            if (Input.GetKeyDown(KeyCode.T))
            {
                ShowMulitplayerPanel();
            }
        }
    }
}
    
