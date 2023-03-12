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
    public class UIQuestController : MonoBehaviour
    {
        private GameplayController gameplayController;
        private UserDataManager userDataManager;

        [SerializeField]
        private GameObject questCanvas;

        [SerializeField]
        private GameObject playerCanvas;

        [SerializeField]
        private GameObject questVcam;

        [SerializeField]
        private Animator anim;

        [SerializeField]
        private Outline QuestBoardOutline;

        [SerializeField]
        private QuestPanelController questPanelController;

        [SerializeField]
        private UIRotateTowardPlayer interactUI;

        private GameDataManager gameDataManager;


        private void Awake()
        {
            gameplayController = SharedContext.Instance.Get<GameplayController>();
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
            gameDataManager = SharedContext.Instance.Get<GameDataManager>();

            QuestBoardOutline.OutlineWidth = 0f;
        }

        public void ShowQuestCanvas()
        {
            questVcam.SetActive(true);
            questCanvas.SetActive(true);
            anim.SetTrigger("show");

            StartCoroutine(ShowUIQuest());
        }

        public void HideQuestCanvas()
        {
            questVcam.SetActive(false);
            questPanelController.DeselectAllBanner();
            anim.SetTrigger("hidePreset");
            anim.SetTrigger("hideAdjust");
            anim.SetTrigger("hide");

            StartCoroutine(HideUIQuest());
        }

        IEnumerator ShowUIQuest()
        {
            yield return new WaitForSeconds(1.5f);

            playerCanvas.SetActive(false);

            gameplayController.Interacable = false;
            gameplayController.SetCursorLock(false);
        }

        IEnumerator HideUIQuest()
        {
            yield return new WaitForSeconds(2.7f);

            playerCanvas.SetActive(true);
            questCanvas.SetActive(false);

            gameplayController.Interacable = true;
            gameplayController.SetCursorLock(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) { return; }

            QuestBoardOutline.OutlineWidth = 4.5f;

            interactUI.Show();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) { return; }

            QuestBoardOutline.OutlineWidth = 0f;

            interactUI.Hide();
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) { return; }

            if (Input.GetKeyDown(KeyCode.T) && !questVcam.activeSelf)
            {
                ShowQuestCanvas();
                other.GetComponent<PlayerRpgMovement>().canMove = false;
                other.GetComponent<PlayerRpgMovement>().ResetAnimBoolean();
                other.GetComponent<PlayerRpgMovement>().StopSFX();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && questVcam.activeSelf)
            {
                HideQuestCanvas();
                other.GetComponent<PlayerRpgMovement>().canMove = true;
            }
        }
    }

    public class PresetInfo
    {
        public List<QuestInfo> SavePresets;
    }
}
    
