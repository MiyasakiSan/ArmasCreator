using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArmasCreator.Gameplay
{
    public class UIMultiplayerMode : MonoBehaviour
    {
        [SerializeField]
        private GameObject mulitplayerPanel;

        [SerializeField]
        private GameObject multiplayerVcam;

        void Start()
        {
            multiplayerVcam.SetActive(false);
        }

        public void ShowMulitplayerPanel()
        {
            multiplayerVcam.SetActive(true);

            mulitplayerPanel.SetActive(true);

            //TODO : Wait some shit
        }

        public void HideMultiplayerPanel()
        {
            multiplayerVcam.SetActive(false);

            mulitplayerPanel.SetActive(false);

            //TODO : Wait some shit
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) { return; }

            if(mulitplayerPanel.activeSelf) { return; }

            if (Input.GetKeyDown(KeyCode.T))
            {
                ShowMulitplayerPanel();
            }
        }
    }
}
    
