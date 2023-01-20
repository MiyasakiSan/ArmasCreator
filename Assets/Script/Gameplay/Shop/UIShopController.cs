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

        private GameplayController gameplayController;

        private void Awake()
        {
            gameplayController = SharedContext.Instance.Get<GameplayController>();
        }

        IEnumerator ShowUIShop()
        {
            playerCanvas.SetActive(false);
            shopController.ShowMainShop();

            yield return new WaitForSeconds(1.5f);

            gameplayController.Interacable = false;
        }

        IEnumerator HideUIShop()
        {
            shopController.HideMainShop();

            yield return new WaitForSeconds(2.7f);

            playerCanvas.SetActive(true);
            gameplayController.Interacable = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) { return; }

            if (Input.GetKeyDown(KeyCode.T) && !shopVcam.activeSelf)
            {
                shopVcam.SetActive(true);

                StartCoroutine(ShowUIShop());

                other.GetComponent<PlayerRpgMovement>().canMove = false;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && shopVcam.activeSelf)
            {
                shopVcam.SetActive(false);

                StartCoroutine(HideUIShop());

                other.GetComponent<PlayerRpgMovement>().canMove = true;
            }
        }
    }
}
  
