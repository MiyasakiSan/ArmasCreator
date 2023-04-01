using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using ArmasCreator.GameData;
using ArmasCreator.UserData;
using ArmasCreator.Utilities;
using UnityEngine.U2D;
using System;

namespace ArmasCreator.UI
{
    public class ChallengeDetailPanelController : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text header;
        [SerializeField]
        private TMP_Text ProgressText;
        [SerializeField]
        private RewardNode rewardNode;
        [SerializeField]
        private Transform rewardContent;

        private bool isHandleHasValue;

        private AsyncOperationHandle<SpriteAtlas> handle;

        private GameDataManager gameDataManager;

        private UserDataManager userDataManager;

        private Coroutine loadingSpriteCoroutine;

        private CoroutineHelper coroutineHelper;

        private AchievementModel achievementInfo;
        public AchievementModel AchievementInfo => achievementInfo;

        private void Awake()
        {
            gameDataManager = SharedContext.Instance.Get<GameDataManager>();
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
            coroutineHelper = SharedContext.Instance.Get<CoroutineHelper>();
        }

        void Start()
        {

        }

        void OnDestroy()
        {
            ClearDisplayItemInfo();
        }
        public void ClearDisplayItemInfo()
        {
            ClearSpriteHandle();

            if (loadingSpriteCoroutine != null)
            {
                coroutineHelper.StopCoroutine(loadingSpriteCoroutine);
                loadingSpriteCoroutine = null;
            }

            RewardNode[] rewardContentChild = rewardContent.GetComponentsInChildren<RewardNode>();
            foreach(RewardNode rewardNodeInContent in rewardContentChild)
            {
                Destroy(rewardNodeInContent.gameObject);
            }

            achievementInfo = null;
        }

        private void ClearSpriteHandle()
        {
            if (!isHandleHasValue) return;

            Addressables.Release(handle);

            handle = default;
            isHandleHasValue = false;
        }

        public void SetDisplayItem(string achievementID)
        {
            var achievementExist = gameDataManager.TryGetAchievementInfo(achievementID, out var achievementInfo);
            if (!achievementExist)
            {
                Debug.LogError("Fail to find info for a itemId: " + achievementID);
                header.gameObject.SetActive(false);
                ProgressText.gameObject.SetActive(false);
                return;
            }

            header.gameObject.SetActive(true);
            ProgressText.gameObject.SetActive(true);

            SetItemInfo(achievementInfo);
        }

        private void SetItemInfo(AchievementModel achievementInfo)
        {
            ClearDisplayItemInfo();

            this.achievementInfo = achievementInfo;

            loadingSpriteCoroutine = coroutineHelper.StartCoroutine(LoadingSpriteRoutine());
        }

        private IEnumerator LoadingSpriteRoutine()
        {
            header.text = achievementInfo.Header + $"<color=#FF0000>{achievementInfo.Progress} </color>" + "Times";
            ProgressText.text = "Completion Progress" + "   " + userDataManager.UserData.UserDataProgression.Achievements[achievementInfo.ID].ToString() + "/" + $"<color=#FF0000>{achievementInfo.Progress} </color>";
            Dictionary<string, int> rewardDictionary = achievementInfo.Rewards;
            foreach (string rewardID in rewardDictionary.Keys)
            {
                var exist = gameDataManager.TryGetItemInfoWithType(rewardID,ItemType.Recipe, out var itemInfo);

                if (!exist)
                {
                    Debug.LogError("Fail to find info for a itemId: " + rewardID);
                    header.gameObject.SetActive(false);
                    ProgressText.gameObject.SetActive(false);
                    loadingSpriteCoroutine = null;
                }
                else
                {
                    handle = Addressables.LoadAssetAsync<SpriteAtlas>(itemInfo.AtlasIcon);

                    isHandleHasValue = true;

                    yield return new WaitUntil(() => handle.IsDone);

                    var atlas = handle.Result;

                    var sprite = atlas.GetSprite(itemInfo.IconName);

                    RewardNode ins_RewardNode = Instantiate(rewardNode, rewardContent);
                    string rewardString = itemInfo.Name + " Recipe" + " x " + achievementInfo.Rewards[rewardID].ToString(); ;
                    ins_RewardNode.SetReward(sprite, rewardString);

                    loadingSpriteCoroutine = null;
                }
            }
        }
    }
}
