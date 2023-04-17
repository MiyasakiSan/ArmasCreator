using System.Collections;
using System.Collections.Generic;
using ArmasCreator.GameData;
using ArmasCreator.UserData;
using ArmasCreator.Utilities;
using UnityEngine;
using ArmasCreator.GameMode;
using ArmasCreator.Gameplay.UI;
using ArmasCreator.UI;


namespace ArmasCreator.Gameplay
{
    public class UIShopController : MonoBehaviour
    {
        [SerializeField]
        private ShopPanelController shopController;

        [SerializeField]
        private GameObject playerCanvas;

        [SerializeField]
        private GameObject shopVcam;

        [SerializeField]
        private Outline ShopOutline;

        [SerializeField]
        private UIRotateTowardPlayer interactUI;

        private GameplayController gameplayController;

        private void Awake()
        {
            gameplayController = SharedContext.Instance.Get<GameplayController>();

            ShopOutline.OutlineWidth = 0f;
        }

        IEnumerator ShowUIShop()
        {
            playerCanvas.SetActive(false);
            gameplayController.Interacable = false;
            shopController.ShowMainShop();

            yield return new WaitForSeconds(1.5f);

            gameplayController.SetCursorLock(false);
        }

        IEnumerator HideUIShop()
        {
            shopController.HideMainShop();

            yield return new WaitForSeconds(2.7f);

            playerCanvas.SetActive(true);
            gameplayController.Interacable = true;
            gameplayController.SetCursorLock(true);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) { return; }

            if (Input.GetKeyDown(KeyCode.T) && !shopVcam.activeSelf)
            {
                shopVcam.SetActive(true);

                StartCoroutine(ShowUIShop());

                other.GetComponent<PlayerRpgMovement>().canMove = false;
                other.GetComponent<CombatRpgManager>().canBattle = false;
                other.GetComponent<PlayerRpgMovement>().ResetAnimBoolean();
                other.GetComponent<PlayerRpgMovement>().StopSFX();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && shopVcam.activeSelf)
            {
                shopVcam.SetActive(false);

                StartCoroutine(HideUIShop());

                other.GetComponent<PlayerRpgMovement>().canMove = true;
                other.GetComponent<CombatRpgManager>().canBattle = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) { return; }

            ShopOutline.OutlineWidth = 4.5f;

            interactUI.Show();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) { return; }

            ShopOutline.OutlineWidth = 0f;

            interactUI.Hide();
        }
    }
}
  
