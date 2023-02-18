using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArmasCreator.GameData;
using ArmasCreator.UserData;
using ArmasCreator.Utilities;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

namespace ArmasCreator.UI
{
    public class NPCQuestDetailController : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text headerText;

        [SerializeField]
        private TMP_Text detailText;

        [SerializeField]
        private Transform rewardViewContent;

        [SerializeField]
        private RewardNode rewardNode;

        protected bool isHandleHasValue;

        protected AsyncOperationHandle<SpriteAtlas> handle;

        private GameDataManager gameDataManager;

        private List<RewardNode> rewardNodeList = new List<RewardNode>();

        private void Awake()
        {
            gameDataManager = SharedContext.Instance.Get<GameDataManager>();
        }

        public void SetHeaderAndDetailText(string newHeaderText, string newDetailText)
        {
            headerText.text = newHeaderText;
            detailText.text = newDetailText;
        }

        public IEnumerator PopulateRewardNode(ConversationQuestModel questInfo)
        {
            SetHeaderAndDetailText(questInfo.Name, questInfo.Description);

            foreach (KeyValuePair<string, int> reward in questInfo.Rewards)
            {
                ItemType rewardItemType = gameDataManager.GetItemType(reward.Key);
                bool exist = gameDataManager.TryGetItemInfoWithType(reward.Key, rewardItemType, out var itemInfo);

                if (!exist)
                {
                    Debug.LogError($"{reward.Key} doesn't exist in {rewardItemType} items");
                    continue;
                }

                handle = Addressables.LoadAssetAsync<SpriteAtlas>(itemInfo.AtlasIcon);

                isHandleHasValue = true;

                yield return new WaitUntil(() => handle.IsDone);

                var atlas = handle.Result;

                var sprite = atlas.GetSprite(itemInfo.IconName);

                var ins_rewardNode = Instantiate(rewardNode, rewardViewContent);

                ins_rewardNode.SetReward(sprite, itemInfo.Name);

                rewardNodeList.Add(ins_rewardNode);
            }
        }

        public void ClearRewardNodeList()
        {
            if(rewardNodeList.Count == 0)
            {
                return;
            }

            foreach (RewardNode rewardNode in rewardNodeList)
            {
                Destroy(rewardNode.gameObject);
            }
            rewardNodeList.Clear();
        }
    }
}