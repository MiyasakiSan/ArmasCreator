using System.Collections;
using System.Collections.Generic;
using ArmasCreator.GameData;
using ArmasCreator.UserData;
using ArmasCreator.Utilities;
using UnityEngine;
using ArmasCreator.GameMode;

namespace ArmasCreator.Gameplay
{
    public class UIQuestController : MonoBehaviour
    {
        private float atkMultiplier;
        private float speedMultiplier;
        private float hpMultiplier;

        private GameplayController gameplayController;
        private UserDataManager userDataManager;

        private string currentMapId;
        private Dictionary<string, PresetInfo> presetInfos = new Dictionary<string, PresetInfo>();

        [SerializeField]
        private GameObject questCanvas;

        [SerializeField]
        private GameObject playerCanvas;

        [SerializeField]
        private GameObject questVcam;

        [SerializeField]
        private Animator anim;

        private GameDataManager gameDataManager;

        private void Awake()
        {
            gameplayController = SharedContext.Instance.Get<GameplayController>();
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
            gameDataManager = SharedContext.Instance.Get<GameDataManager>();
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
            anim.SetTrigger("hide");

            StartCoroutine(HideUIQuest());
        }

        IEnumerator ShowUIQuest()
        {
            yield return new WaitForSeconds(1.5f);

            playerCanvas.SetActive(false);

            gameplayController.Interacable = false;
        }

        IEnumerator HideUIQuest()
        {
            yield return new WaitForSeconds(1.5f);

            playerCanvas.SetActive(true);
            questCanvas.SetActive(false);

            gameplayController.Interacable = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) { return; }

            if (Input.GetKeyDown(KeyCode.T) && !questVcam.activeSelf)
            {
                ShowQuestCanvas();
                other.GetComponent<PlayerRpgMovement>().canMove = false;
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
    
