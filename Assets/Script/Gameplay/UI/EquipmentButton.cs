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
    public class EquipmentButton : Button
    {
        [SerializeField]
        protected Image displayItemImage;

        [SerializeField]
        protected TMP_Text displayItemDetail;

        protected Sprite defaultSprite;

        protected GameDataManager gameDataManager;

        protected UserDataManager userDataManager;

        protected CoroutineHelper coroutineHelper;

        protected bool isHandleHasValue;

        protected AsyncOperationHandle<SpriteAtlas> handle;

        protected Coroutine loadingSpriteCoroutine;

        private ItemInfoModel itemInfo; /*====>  put some item info here See in Game Data*/
        public ItemInfoModel ItemInfo => itemInfo;

        protected override void Awake()
        {
            gameDataManager = SharedContext.Instance.Get<GameDataManager>();
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
            coroutineHelper = SharedContext.Instance.Get<CoroutineHelper>();
            defaultSprite = displayItemImage.sprite;
        }

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
            var exist = gameDataManager.TryGetItemInfoWithType(itemId, ItemType.Equipable, out var itemInfo);

            if (!exist)
            {
                Debug.LogError("Fail to find info for a itemId: " + itemId);
                displayItemImage.gameObject.SetActive(false);
                displayItemDetail.gameObject.SetActive(false);
                return;
            }

            displayItemImage.gameObject.SetActive(true);
            displayItemDetail.gameObject.SetActive(true);

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
            displayItemDetail.text = itemInfo.Name;

            // Set other information here

            loadingSpriteCoroutine = null;
        }
    }
}

