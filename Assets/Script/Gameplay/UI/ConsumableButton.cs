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
using UnityEngine.EventSystems;

namespace ArmasCreator.UI
{
    public class ConsumableButton : Button , IPointerEnterHandler , IPointerExitHandler
    {
        [SerializeField]
        protected Image displayItemImage;

        [SerializeField]
        protected TMP_Text displayItemDetail;

        [SerializeField]
        protected ItemDetailBoxController itemDetailBoxController;

        protected Sprite defaultSprite;

        protected GameDataManager gameDataManager;

        protected UserDataManager userDataManager;

        protected CoroutineHelper coroutineHelper;

        protected bool isHandleHasValue;

        protected AsyncOperationHandle<SpriteAtlas> handle;

        protected Coroutine loadingSpriteCoroutine;

        private ConsumeableItemModel itemInfo; /*====>  put some item info here See in Game Data*/
        public ConsumeableItemModel ItemInfo => itemInfo;

        private string itemName;
        private string itemDetail;
        private Sprite itemIconSprite;
        private Sprite itemIconTypeSprite;

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
            var exist = gameDataManager.TryGetConsumableItemInfoById(itemId, out var itemInfo);

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

            if (ItemInfo.SubType == SubType.Health)
            {
                itemIconTypeSprite = atlas.GetSprite("RecoveryStatus");
            }
            else if (ItemInfo.SubType == SubType.Stamina)
            {
                itemIconTypeSprite = atlas.GetSprite("EnergyStatus");
            }

            itemIconSprite = sprite;

            itemName = itemInfo.Name;

            itemDetail = "recovery " + itemInfo.SubType.ToString().ToLower() + " " + itemInfo.ConsumePercent.ToString() + " %";

            Debug.Log(itemDetail);

            displayItemImage.sprite = sprite;

            displayItemDetail.text = userDataManager.UserData.UserDataInventory.ConsumableItems[itemInfo.ID].ToString();
            // Set other information here

            loadingSpriteCoroutine = null;
        }

        public void SetItemDetailController(ItemDetailBoxController newItemDetailBoxController)
        {
            itemDetailBoxController = newItemDetailBoxController;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            var thisRectTranform = this.GetComponent<RectTransform>().anchoredPosition;
            itemDetailBoxController.GetComponent<RectTransform>().anchoredPosition = new Vector2(thisRectTranform.x + 105, thisRectTranform.y - 155);
            itemDetailBoxController.OnMouseEnterItemSlot(itemIconSprite, itemIconTypeSprite, itemName, itemDetail);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            itemDetailBoxController.OnMouseExitItemSlot();
        }
    }
}

