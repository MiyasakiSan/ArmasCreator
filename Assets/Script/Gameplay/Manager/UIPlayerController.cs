using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArmasCreator.GameMode;
using ArmasCreator.Utilities;
using Unity.Netcode;
using TMPro;
using ArmasCreator.UserData;

namespace ArmasCreator.Gameplay.UI
{
    public class UIPlayerController : NetworkBehaviour
    {
        [SerializeField]
        private UIStatControl playerStatUI;
        public UIStatControl PlayerStatUI => playerStatUI;

        [SerializeField]
        private TextMeshProUGUI playerNameText;
        public TextMeshProUGUI PlayerNameText => playerNameText;

        [SerializeField]
        private GameObject otherPlayerCanvas;
        public GameObject OtherPlayerCanvas => otherPlayerCanvas;

        [SerializeField]
        private GameObject content;

        [SerializeField]
        private UIItemBar itemBar;

        [SerializeField]
        private TextMeshProUGUI damageDealtText;

        [SerializeField]
        private Animator damageDealtAnim;

        private Coroutine damageDealtCoroutine;

        private GameModeController gameModeController;
        private UserDataManager userDataManager;
        private GameplayController gameplayController;

        [SerializeField]
        private Animator hurtAnim;

        [SerializeField]
        private Camera cam;

        [SerializeField]
        private RectTransform rectTransform;

        private bool IsSinglePlayer => gameModeController.IsSinglePlayerMode;

        private void Awake()
        {
            SharedContext.Instance.Add(this);
            gameModeController = SharedContext.Instance.Get<GameModeController>();
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
        }

        private void Start()
        {
            playerNameText.text = PlayerPrefs.GetString("PName");

            itemBar.Init(userDataManager);

            gameplayController = SharedContext.Instance.Get<GameplayController>();

            if (gameplayController != null)
            {
                if (gameplayController.CurrentGameplays == GameplayController.Gameplays.Town)
                {
                    itemBar.Hide();
                }
            }

            damageDealtText.gameObject.SetActive(false);
        }

        public void ShowDamage(Vector3 contactPoint, float damage)
        {
            if (damageDealtCoroutine != null)
            {
                StopCoroutine(damageDealtCoroutine);
                damageDealtCoroutine = null;
            }

            Vector3 screenPos = cam.WorldToScreenPoint(contactPoint);
            var correctPoint = new Vector2(screenPos.x / Screen.width * rectTransform.sizeDelta.x, screenPos.y / Screen.height * rectTransform.sizeDelta.y);
            damageDealtText.GetComponent<RectTransform>().anchoredPosition = screenPos;

            damageDealtCoroutine = StartCoroutine(showDeltDamage(damage));
        }

        IEnumerator showDeltDamage(float damage)
        {
            damageDealtText.gameObject.SetActive(true);
            damageDealtText.text = damage.ToString();
            damageDealtAnim.SetTrigger("hit");

            yield return new WaitForSeconds(0.75f);

            damageDealtText.gameObject.SetActive(false);
            damageDealtCoroutine = null;
        }

        public void ShowHurt()
        {
            hurtAnim.SetTrigger("Hurt");
        }

        public void Show()
        {
            content.SetActive(true);
        }

        public void Hide()
        {
            content.SetActive(false);
        }

        private void OnDestroy()
        {
            SharedContext.Instance.Remove(this);
        }
    }
}


