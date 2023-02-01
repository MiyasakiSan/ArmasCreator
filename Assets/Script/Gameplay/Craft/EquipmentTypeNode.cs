using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArmasCreator.GameData;
using ArmasCreator.Utilities;
using ArmasCreator.UserData;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using UnityEngine.AddressableAssets;
using TMPro;
using System;

namespace ArmasCreator.UI
{
    public class EquipmentTypeNode : MonoBehaviour
    {
        [SerializeField]
        private GameObject selectedBorder;

        [SerializeField]
        private GameObject lockIcon;

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private GameObject OwnedText;

        [SerializeField]
        private ItemRequireNode itemRequireNodePrefab;

        private CraftButton craftButton;

        private Transform itemRequireBox;

        private TMP_Text moneyRequire;

        private GameDataManager gameDataManager;

        private UserDataManager userDataManager;

        private CoroutineHelper coroutineHelper;

        private bool isHandleHasValue;

        private AsyncOperationHandle<SpriteAtlas> handle;

        private Coroutine loadingSpriteCoroutine;

        private Sprite defaultSprite;

        private RecipeModel itemInfo; /*====>  put some item info here See in Game Data*/
        public RecipeModel ItemInfo => itemInfo;

        private bool isUnlock = false;

        void Awake()
        {
            gameDataManager = SharedContext.Instance.Get<GameDataManager>();
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
            coroutineHelper = SharedContext.Instance.Get<CoroutineHelper>();
            defaultSprite = iconImage.sprite;
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

            iconImage.sprite = defaultSprite;

            //reset information here

            Addressables.Release(handle);

            handle = default;
            isHandleHasValue = false;
        }

        public void SetDisplayItem(string itemId)
        {
            var exist = gameDataManager.TryGetRecipeInfo(itemId, out var itemInfo);

            if (!exist)
            {
                Debug.LogError("Fail to find info for a itemId: " + itemId);
                iconImage.gameObject.SetActive(false);
                return;
            }

            iconImage.gameObject.SetActive(true);

            SetItemInfo(itemInfo);
        }

        private void SetItemInfo(RecipeModel itemInfo)
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

            iconImage.sprite = sprite;

            loadingSpriteCoroutine = null;
        }

        public void SetIsUnlockEquipmentTypeNode(bool isUnlock)
        {
            lockIcon.SetActive(isUnlock);
        }

        public void OnSelectedEquipmentTypeNode()
        {
            if (!lockIcon.activeSelf)
            {
                if (!OwnedText.activeSelf)
                {
                    PopulateItemRequireNode();
                    selectedBorder.SetActive(true);
                    if (userDataManager.UserData.Coins >= itemInfo.BuyPrice)
                    {
                        moneyRequire.text = string.Format("<color=white>{0} s</color>", itemInfo.BuyPrice.ToString());
                    }
                    else
                    {
                        moneyRequire.text = string.Format("{0} s", itemInfo.BuyPrice.ToString());
                    }
                    craftButton.OnClickEquipmentTypeNode(itemInfo.ID);
                    craftButton.SetCurrentEquipmentTypeNode(this);
                }
            }
        }

        public void SetIsOwned(bool isOwned)
        {
            OwnedText.SetActive(isOwned);
        }

        public void OnDeSelectedEquipmentTypeNode()
        {
            selectedBorder.SetActive(false);
        }

        public void SetMoneyRequire(TMP_Text newMoneyRequire)
        {
            moneyRequire = newMoneyRequire;
        }

        public void SetCraftButton(CraftButton newCraftbutton)
        {
            craftButton = newCraftbutton;
        }

        public void SetItemRequireBox(Transform newItemRequireBox)
        {
            itemRequireBox = newItemRequireBox;
        }

        public void PopulateItemRequireNode()
        {
            var exist = gameDataManager.TryGetRecipeInfo(itemInfo.ID,out var recipeInfo);

            if (exist)
            {
                foreach (string itemID in recipeInfo.Recipe.Keys)
                {
                    var ins_itemRequire = Instantiate(itemRequireNodePrefab, itemRequireBox);

                    ins_itemRequire.SetDisplayItem(itemID, recipeInfo.Recipe[itemID]);
                }
            }
        }
    }
}
