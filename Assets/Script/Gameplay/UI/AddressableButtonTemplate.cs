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

namespace ArmasCreator.UI
{
    public class AddressableButtonTemplate : Button
    {
        [SerializeField]
        protected Image displayItemImage;

        protected Sprite defaultSprite;

        protected GameDataManager gameDataManager;

        protected UserDataManager userDataManager;

        protected CoroutineHelper coroutineHelper;

        protected bool isHandleHasValue;

        protected AsyncOperationHandle<SpriteAtlas> handle;

        protected Coroutine loadingSpriteCoroutine;

        private ItemInfoModel itemInfo; /*====>  put some item info here See in Game Data*/
        public ItemInfoModel ItemInfo => itemInfo;

        protected virtual void OnDestroy()
        {
            ClearDisplayItemInfo();
            this.onClick.RemoveAllListeners();
        }
        public void ClearDisplayItemInfo()
        {
            ClearSpriteHandle();

            if (loadingSpriteCoroutine != null)
            {
                coroutineHelper.StopCoroutine(loadingSpriteCoroutine);
                loadingSpriteCoroutine = null;
            }

            itemInfo = null;
        }

        private void ClearSpriteHandle()
        {
            if (!isHandleHasValue) return;

            displayItemImage.sprite = defaultSprite;

            //reset information here

            Addressables.Release(handle);

            handle = default;
            isHandleHasValue = false;
        }

        public void SetDisplayItem(string itemId)
        {
            //var exist = gameDataManager.TryGetEmojiInfo(emojiId, out var emojiInfo);   ============ use try get to get itemInfo

            if (/*!exist*/ false)
            {
                Debug.LogError("Fail to find info for a itemId: " + itemId);
                gameObject.SetActive(false);
                return;
            }

            displayItemImage.gameObject.SetActive(true);

            SetItemInfo(itemInfo);
        }

        private void SetItemInfo(ItemInfoModel itemInfo)
        {
            ClearDisplayItemInfo();

            this.itemInfo = itemInfo;

            loadingSpriteCoroutine = coroutineHelper.StartCoroutine(LoadingSpriteRoutine());
        }

        private IEnumerator LoadingSpriteRoutine()
        {
            handle = Addressables.LoadAssetAsync<SpriteAtlas>(itemInfo.AtlasIcon);

            isHandleHasValue = true;

            yield return new WaitUntil(() => handle.IsDone);

            var atlas = handle.Result;

            var sprite = atlas.GetSprite(itemInfo.IconName);

            displayItemImage.sprite = sprite;

            // Set other information here

            loadingSpriteCoroutine = null;
        }
    }
}

