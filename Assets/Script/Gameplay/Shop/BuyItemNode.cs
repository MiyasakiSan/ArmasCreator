using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArmasCreator.GameData;
using ArmasCreator.UserData;
using ArmasCreator.Utilities;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using UnityEngine.AddressableAssets;
using TMPro;


namespace ArmasCreator.UI
{
    public class BuyItemNode : Button
    {
        private Sprite defaultSprite;

        private GameDataManager gameDataManager;

        private CoroutineHelper coroutineHelper;

        protected bool isHandleHasValue;

        protected AsyncOperationHandle<SpriteAtlas> handle;

        protected Coroutine loadingSpriteCoroutine;

        private ConsumeableItemModel itemInfo; /*====>  put some item info here See in Game Data*/
        public ConsumeableItemModel ItemInfo => itemInfo;

        [SerializeField]
        private Image displayItemImage;

        [SerializeField]
        private TMP_Text itemNameText;

        [SerializeField]
        private TMP_Text itemPriceText;

        [SerializeField]
        private GameObject selectedBG;

        private float itemPrice;

        private ConfirmBuyBoxController confirmBuyBoxController;

        protected virtual void OnDestroy()
        {
            ClearDisplayItemInfo();
            this.onClick.RemoveAllListeners();
        }

        protected override void Awake()
        {
            gameDataManager = SharedContext.Instance.Get<GameDataManager>();
            coroutineHelper = SharedContext.Instance.Get<CoroutineHelper>();
            defaultSprite = displayItemImage.sprite;
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
            var exist = gameDataManager.TryGetConsumeItemInfo(itemId, out var itemInfo);

            if (!exist)
            {
                Debug.LogError("Fail to find info for a itemId: " + itemId);
                displayItemImage.gameObject.SetActive(false);
                itemNameText.gameObject.SetActive(false);
                itemPriceText.gameObject.SetActive(false);
                return;
            }

            displayItemImage.gameObject.SetActive(true);
            itemNameText.gameObject.SetActive(true);
            itemPriceText.gameObject.SetActive(true);

            SetItemInfo(itemInfo);
        }

        private void SetItemInfo(ConsumeableItemModel itemInfo)
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

            itemNameText.text = itemInfo.Name;

            itemPrice = itemInfo.BuyPrice;

            itemPriceText.text = itemInfo.BuyPrice.ToString() + " s";

            loadingSpriteCoroutine = null;
        }

        public void SetUPNewConfirmBuyBox(ConfirmBuyBoxController newConfirmBuyBoxController)
        {
            confirmBuyBoxController = newConfirmBuyBoxController;
        }

        public void SetUpConfirmBuyBox()
        {
            confirmBuyBoxController.SetUpConfirmBuyBox(displayItemImage.sprite, itemNameText.text, itemPrice,itemInfo.ID);
        }
    }
}
