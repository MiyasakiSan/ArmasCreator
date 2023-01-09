using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using ArmasCreator.UserData;
using ArmasCreator.GameData;
using ArmasCreator.Utilities;

namespace ArmasCreator.UI
{
    public class ConfirmSellBoxController : MonoBehaviour
    {
        [SerializeField]
        private ShopPanelController ShopPanelController;
        [SerializeField]
        private SellShopPanelController sellShopPanelController;
        [SerializeField]
        private Button arrowUpButton;
        [SerializeField]
        private Button arrowDownButton;
        [SerializeField]
        private Button sellButton;
        [SerializeField]
        private Button cancelButton;
        [SerializeField]
        private Image itemIcon;
        [SerializeField]
        private TMP_Text itemNameText;
        [SerializeField]
        private TMP_Text itempriceText;
        [SerializeField]
        private TMP_InputField sellAmountText;
        [SerializeField]
        private GameObject confirmSellBoxContent;

        private UserDataManager userDataManager;

        private GameDataManager gameDataManager;

        private float itemPrice;

        private string itemId;
        // Start is called before the first frame update

        private void Awake()
        {
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
            gameDataManager = SharedContext.Instance.Get<GameDataManager>();
        }
        void Start()
        {
            sellAmountText.onValueChanged.AddListener(delegate 
            { 
                OnItemAmountChanged();
            });
            sellAmountText.onEndEdit.AddListener(delegate
            {
                OnEndEdititemAmount();
            });
            arrowUpButton.onClick.AddListener(() =>
            {
                if (gameDataManager.GetItemType(itemId) == ItemType.Consumable)
                {
                    if (userDataManager.UserData.UserDataInventory.ConsumableItems.TryGetValue(itemId, out int itemAmount))
                    {
                        if (float.Parse(sellAmountText.text) < itemAmount)
                        {
                            sellAmountText.text = (float.Parse(sellAmountText.text) + 1).ToString();
                        }
                    }
                }
                else
                {
                    if (userDataManager.UserData.UserDataInventory.CraftableItems.TryGetValue(itemId, out int itemAmount))
                    {
                        if (float.Parse(sellAmountText.text) < itemAmount)
                        {
                            sellAmountText.text = (float.Parse(sellAmountText.text) + 1).ToString();
                        }
                    }
                }
            });
            arrowDownButton.onClick.AddListener(() =>
            {
                if (float.Parse(sellAmountText.text) > 0)
                {
                    sellAmountText.text = (float.Parse(sellAmountText.text) - 1).ToString();
                }
            });
            sellButton.onClick.AddListener(() => 
            {
                bool sellComplate = true;
                if (gameDataManager.GetItemType(itemId) == ItemType.Consumable)
                {
                    userDataManager.UserData.UserDataInventory.ConsumableItems.TryGetValue(itemId, out int itemAmount);
                    if (itemAmount < int.Parse(sellAmountText.text))
                    {
                        sellComplate = false;
                    }
                }
                else
                {
                    userDataManager.UserData.UserDataInventory.CraftableItems.TryGetValue(itemId, out int itemAmount);
                    if (itemAmount < int.Parse(sellAmountText.text))
                    {
                        sellComplate = false;
                    }
                }
                if (sellComplate)
                {
                    var newCoin = userDataManager.UserData.Coins + itemPrice * float.Parse(sellAmountText.text);
                    userDataManager.UserData.UpdateCoin(newCoin);
                    if (gameDataManager.GetItemType(itemId) == ItemType.Consumable)
                    {
                        userDataManager.UserData.UserDataInventory.RemoveConsumeItem(itemId, int.Parse(sellAmountText.text));
                    }
                    else if (gameDataManager.GetItemType(itemId) == ItemType.Craftable)
                    {
                        userDataManager.UserData.UserDataInventory.RemoveCraftItem(itemId, int.Parse(sellAmountText.text));
                    }
                    ShopPanelController.UpdatePlayerCoinsText();
                    sellShopPanelController.UpdateItemBag();
                    sellAmountText.text = "0";
                    confirmSellBoxContent.SetActive(false);
                }
            });
            cancelButton.onClick.AddListener(() =>
            {
                confirmSellBoxContent.SetActive(false);
            });
        }

        public void SetUpConfirmBuyBox(Sprite newItemIconSprite,string newItemName,float newPrice,string newItemId)
        {
            itemIcon.sprite = newItemIconSprite;
            itemNameText.text = newItemName;
            itemPrice = newPrice;
            itemId = newItemId;
            sellAmountText.text = "0";
            itempriceText.text = "0 s";
        }

        public void OnItemAmountChanged()
        {
            if (userDataManager.UserData.UserDataInventory.ConsumableItems.TryGetValue(itemId, out int itemAmount))
            {
                if (float.Parse(sellAmountText.text) > itemAmount)
                {
                    sellAmountText.text = itemAmount.ToString();
                }
                if (float.Parse(sellAmountText.text) < 0)
                {
                    sellAmountText.text = (0).ToString();
                }
                itempriceText.text = (itemPrice * float.Parse(sellAmountText.text)).ToString() + " s";
            }
        }

        public void OnEndEdititemAmount()
        {
            if (string.IsNullOrEmpty(sellAmountText.text))
            {
                sellAmountText.text = (0).ToString();
            }
        }

        public void OpenConfirmBox()
        {
            confirmSellBoxContent.SetActive(true);
        }
    }
}
