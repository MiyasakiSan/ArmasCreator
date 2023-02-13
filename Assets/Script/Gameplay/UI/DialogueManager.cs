using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArmasCreator.Utilities;
using UnityEngine.UI;
using ArmasCreator.GameData;
using ArmasCreator.UserData;
using ArmasCreator.UI;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using UnityEngine.AddressableAssets;

namespace ArmasCreator.UI
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField]
        private UIDialoguePanel npcPanel;

        [SerializeField]
        private UIDialoguePanel playerPanel;

        [SerializeField]
        private Animator dialogueBG;

        private DialogueModel dialogueInfo;
        private int sequenceIndex;

        private bool isShowingDialogue;

        private const string CharacterIconAtlas = "AtlasCharacterIcon";
        public bool IsShowingDialogue => isShowingDialogue;

        private GameDataManager gameDataManager;

        private UserDataManager userDataManager;

        protected CoroutineHelper coroutineHelper;

        protected bool isHandleHasValue;

        protected AsyncOperationHandle<SpriteAtlas> handle;

        protected Coroutine loadingSpriteCoroutine;

        [SerializeField]
        private string testDialogueId;

        [SerializeField]
        private string currentDialogueId;

        public QuestModel CurrentQuestInfo;

        private void Awake()
        {
            SharedContext.Instance.Add(this);
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            gameDataManager = SharedContext.Instance.Get<GameDataManager>();

            userDataManager = SharedContext.Instance.Get<UserDataManager>();

            coroutineHelper = SharedContext.Instance.Get<CoroutineHelper>();

            LoadCharacterIconSpriteAtlas();

            playerPanel.OnDialogueClick += ShowDialogue;
            npcPanel.OnDialogueClick += ShowDialogue;

            playerPanel.Hide();
            npcPanel.Hide();
        }

        private void Update()
        {
            //if (Input.GetKeyUp(KeyCode.RightShift) && Input.GetKeyUp(KeyCode.U))
            //{
            //    SetDialogueSequence(testDialogueId);
            //}
        }

        private void LoadCharacterIconSpriteAtlas()
        {
            ClearCharacterIcon();
            loadingSpriteCoroutine = coroutineHelper.StartCoroutine(LoadingSpriteRoutine());
        }

        public void ClearCharacterIcon()
        {
            ClearSpriteHandle();

            if (loadingSpriteCoroutine != null)
            {
                coroutineHelper.StopCoroutine(loadingSpriteCoroutine);
                loadingSpriteCoroutine = null;
            }

            dialogueInfo = null;
        }

        private void ClearSpriteHandle()
        {
            if (!isHandleHasValue) return;

            playerPanel.Reset();

            npcPanel.Reset();

            //reset information here

            Addressables.Release(handle);

            handle = default;
            isHandleHasValue = false;
        }

        public void SetDialogueSequence(string dialogueId)
        {
            Debug.Log("Show " + dialogueId);
            bool exist = gameDataManager.TryGetDialogueInfo(dialogueId, out DialogueModel dialogue);

            if (!exist) 
            {
                Debug.LogError("Can't show dialogue");
                return; 
            }

            currentDialogueId = dialogueId;
            playerPanel.Reset();
            npcPanel.Reset();
            dialogueInfo = dialogue;
            isShowingDialogue = true;
            sequenceIndex = 0;
            dialogueBG.SetTrigger("show");

            StartCoroutine(InitDialogue());
        }

        private IEnumerator InitDialogue()
        {
            yield return new WaitForSeconds(1.0f);

            playerPanel.Show();
            npcPanel.Show();

            yield return new WaitForSeconds(0.5f);

            ShowDialogue();
        }

        private IEnumerator LoadingSpriteRoutine()
        {
            handle = Addressables.LoadAssetAsync<SpriteAtlas>(CharacterIconAtlas);

            isHandleHasValue = true;

            yield return new WaitUntil(() => handle.IsDone);

            // Set other information here

            loadingSpriteCoroutine = null;
        }

        private void ShowDialogue()
        {
            if (dialogueInfo == null) { return; }

            if (sequenceIndex >= dialogueInfo.DialogSequences.Count)
            {
                npcPanel.Hide();

                playerPanel.Hide();

                dialogueInfo = null;
                dialogueBG.SetTrigger("hide");

                if (CurrentQuestInfo != null)
                {
                    if (currentDialogueId == CurrentQuestInfo.EndDialogueId) 
                    {
                        userDataManager.UserData.UserDataProgression.EndQuest(CurrentQuestInfo.Id);
                        Debug.Log($"Complete Quest ID : {CurrentQuestInfo.Id} ");
                    }
                }

                Debug.Log($"Complete dialog ");

                currentDialogueId = null;
                CurrentQuestInfo = null;
                return;
            }

            var sequence = dialogueInfo.DialogSequences[sequenceIndex];

            if (sequence == null)
            {
                return;
            }

            if(sequence.CharacterName == "main")
            {
                npcPanel.HideSentence();

                if(sequenceIndex == 0)
                {
                    npcPanel.HideImage();
                }

                playerPanel.SetDisplayImage(handle.Result.GetSprite(sequence.CharacterId));
                playerPanel.SetDisplayText(string.Format(sequence.Sentence, PlayerPrefs.GetString("PName")));
                playerPanel.SetHeaderText(PlayerPrefs.GetString("PName"));
                playerPanel.ShowSentence();
                playerPanel.ShowImage();
            }
            else
            {
                playerPanel.HideSentence();

                if (sequenceIndex == 0)
                {
                    playerPanel.HideImage();
                }

                npcPanel.SetDisplayImage(handle.Result.GetSprite(sequence.CharacterId));
                npcPanel.SetDisplayText(sequence.Sentence);
                npcPanel.SetHeaderText(sequence.CharacterName);
                npcPanel.ShowSentence();
                npcPanel.ShowImage();
            }

            sequenceIndex++;
        }

        private void OnDestroy()
        {
            npcPanel.OnDialogueClick -= ShowDialogue;
            playerPanel.OnDialogueClick -= ShowDialogue;
        }
    }
}

