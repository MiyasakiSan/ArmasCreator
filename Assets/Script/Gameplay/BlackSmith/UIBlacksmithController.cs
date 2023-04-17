using ArmasCreator.Gameplay;
using ArmasCreator.UI;
using ArmasCreator.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBlacksmithController : MonoBehaviour
{
    [SerializeField]
    private CraftShopPanelController craftShopPanelController;

    [SerializeField]
    private GameObject playerCanvas;

    [SerializeField]
    private GameObject blacksmithVcam;

    private GameplayController gameplayController;

    [SerializeField]
    private Outline blacksmithOutline;

    [SerializeField]
    private UIRotateTowardPlayer interactUI;

    private void Awake()
    {
        gameplayController = SharedContext.Instance.Get<GameplayController>();

        blacksmithOutline.OutlineWidth = 0f;
    }

    IEnumerator ShowUIBlacksmith()
    {
        playerCanvas.SetActive(false);
        gameplayController.Interacable = false;
        craftShopPanelController.ShowCraftShop();

        yield return new WaitForSeconds(1.5f);

        gameplayController.SetCursorLock(false);
    }

    IEnumerator HideUIBlacksmith()
    {
        craftShopPanelController.HideCraftShop();

        yield return new WaitForSeconds(2.7f);

        playerCanvas.SetActive(true);
        gameplayController.Interacable = true;
        gameplayController.SetCursorLock(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) { return; }

        if (Input.GetKeyDown(KeyCode.T) && !blacksmithVcam.activeSelf)
        {
            blacksmithVcam.SetActive(true);

            StartCoroutine(ShowUIBlacksmith());

            other.GetComponent<PlayerRpgMovement>().canMove = false;
            other.GetComponent<CombatRpgManager>().canBattle = false;
            other.GetComponent<PlayerRpgMovement>().ResetAnimBoolean();
            other.GetComponent<PlayerRpgMovement>().StopSFX();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && blacksmithVcam.activeSelf)
        {
            blacksmithVcam.SetActive(false);

            StartCoroutine(HideUIBlacksmith());

            other.GetComponent<PlayerRpgMovement>().canMove = true;
            other.GetComponent<CombatRpgManager>().canBattle = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) { return; }

        blacksmithOutline.OutlineWidth = 4.5f;

        interactUI.Show();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) { return; }

        blacksmithOutline.OutlineWidth = 0f;

        interactUI.Hide();
    }
}
