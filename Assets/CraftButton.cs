using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ArmasCreator.UserData;
using ArmasCreator.GameData;
using ArmasCreator.Utilities;
using System;
using TMPro;

namespace ArmasCreator.UI
{
    public class CraftButton : MonoBehaviour , IPointerDownHandler , IPointerUpHandler
    {
        [SerializeField]
        private CraftBoxController craftBoxController;

        [SerializeField]
        private CraftShopPanelController craftShopPanelController;

        [SerializeField]
        private Image ButtonOutline;

        [SerializeField]
        private Button confirmCraftButton;

        [SerializeField]
        private TMP_Text craftedDetailText;

        private string currentSelectedID;

        private UserDataManager userDataManager;

        private GameDataManager gameDataManager;

        private bool isCraftRequireComplete = false;

        private bool isHold = false;

        private float clickTime = 0;

        private float maxTime = 5;

        private RecipeModel recipeInfo;

        void Awake()
        {
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
            gameDataManager = SharedContext.Instance.Get<GameDataManager>();
        }

        void Start()
        {
            confirmCraftButton.onClick.AddListener(() => {
                OnClickCraftComplete();
            });
        }

    void Update()
        {
            if (isHold & isCraftRequireComplete)
            {
                clickTime += Time.deltaTime;

                ButtonOutline.fillAmount = clickTime / maxTime;

                if (clickTime >= maxTime)
                {
                    gameDataManager.TryGetEquipItemInfoById(recipeInfo.Craft_item_id, out var equipableItemInfo);
                    userDataManager.UserData.UserDataInventory.AddEquipItem(equipableItemInfo.ID, equipableItemInfo.SubType);
                    foreach(var craftItem in recipeInfo.Recipe)
                    {
                        userDataManager.UserData.UserDataInventory.RemoveCraftItem(craftItem.Key, craftItem.Value);
                    }
                    userDataManager.UserData.UpdateCoin(userDataManager.UserData.Coins - recipeInfo.BuyPrice);
                    clickTime = 0;
                    isCraftRequireComplete = false;
                    ButtonOutline.fillAmount = clickTime / maxTime;
                    OnCraftComplete();
                    craftShopPanelController.UpdatePlayerCoin();
                }
            }
            if(!isHold)
            {
                clickTime = 0;
                ButtonOutline.fillAmount = clickTime / maxTime;
            }

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(!isCraftRequireComplete)
            {
                return;
            }

            isHold = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isHold = false;
        }

        public void OnClickEquipmentTypeNode(string newCurrentSelectedID)
        {
            currentSelectedID = newCurrentSelectedID;

            isCraftRequireComplete = false;

            var exist = gameDataManager.TryGetRecipeInfo(currentSelectedID, out var newRecipeInfo);

            if (!exist)
            {
                return;
            }

            recipeInfo = newRecipeInfo;

            if (userDataManager.UserData.Coins >= recipeInfo.BuyPrice)
            {
                foreach (var recipe in recipeInfo.Recipe.Keys)
                {
                    var itemAmount = userDataManager.UserData.UserDataInventory.CraftableItems[recipe];
                    if (itemAmount < recipeInfo.Recipe[recipe])
                    {
                        return;
                    }
                }
                isCraftRequireComplete = true;
            }
        }

        public void OnCraftComplete()
        {
            gameDataManager.TryGetEquipItemInfoById(recipeInfo.Craft_item_id, out var equipmentInfo);
            craftedDetailText.text = equipmentInfo.Name + " is crafted";
            craftShopPanelController.ShowCraftComplete();
        }

        public void OnClickCraftComplete()
        {
            craftBoxController.UpdateCraftBoxItem();
            craftBoxController.ClearAllItemRequireNode();
            craftShopPanelController.HideCraftComplete();
        }
    }
}