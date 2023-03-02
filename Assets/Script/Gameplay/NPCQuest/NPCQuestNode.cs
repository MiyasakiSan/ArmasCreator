using ArmasCreator.GameData;
using ArmasCreator.UserData;
using ArmasCreator.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using UnityEngine.UI;
using TMPro;

namespace ArmasCreator.UI
{
    public class NPCQuestNode : Button
    {
        [SerializeField]
        protected TMP_Text displayQuestHeader;

        protected Sprite questPassSprite;

        protected GameDataManager gameDataManager;

        protected UserDataManager userDataManager;

        protected CoroutineHelper coroutineHelper;

        protected bool isHandleHasValue;

        protected AsyncOperationHandle<SpriteAtlas> handle;

        protected Coroutine loadingSpriteCoroutine;

        private ConversationQuestModel questInfo; 
        public ConversationQuestModel QuestInfo => questInfo;

        protected virtual void OnDestroy()
        {
            ClearDisplayItemInfo();
            this.onClick.RemoveAllListeners();
        }

        protected override void Awake()
        {
            gameDataManager = SharedContext.Instance.Get<GameDataManager>();
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
            coroutineHelper = SharedContext.Instance.Get<CoroutineHelper>();
        }

        public void ClearDisplayItemInfo()
        {
            ClearSpriteHandle();

            if (loadingSpriteCoroutine != null)
            {
                coroutineHelper.StopCoroutine(loadingSpriteCoroutine);
                loadingSpriteCoroutine = null;
            }

            questInfo = null;
        }

        private void ClearSpriteHandle()
        {
            if (!isHandleHasValue) return;

            //reset information here

            Addressables.Release(handle);

            handle = default;
            isHandleHasValue = false;
        }

        public void SetDisplayData(string questId)
        {
            var exist = gameDataManager.TryGetConversationQuestInfo(questId, out var questInfo);

            if (!exist)
            {
                Debug.LogError("Fail to find info for a itemId: " + questId);
                displayQuestHeader.gameObject.SetActive(false);
                return;
            }

            displayQuestHeader.gameObject.SetActive(true);

            SetQuestInfo(questInfo);
        }

        private void SetQuestInfo(ConversationQuestModel questInfo)
        {
            ClearDisplayItemInfo();

            this.questInfo = questInfo;

            SetNPCQuestNode();
        }

        private void SetNPCQuestNode()
        {
            displayQuestHeader.text = questInfo.Name;
        }
    }
}

