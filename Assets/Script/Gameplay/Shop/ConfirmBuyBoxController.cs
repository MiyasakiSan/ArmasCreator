using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using ArmasCreator.UserData;
using ArmasCreator.Utilities;

namespace ArmasCreator.UI
{
    public class ConfirmBuyBoxController : MonoBehaviour
    {
        [SerializeField]
        private ShopPanelController shopPanelController;
        [SerializeField]
        private BuyShopPanelController buyShopPanelController;
        [SerializeField]
        private Button arrowUpButton;
        [SerializeField]
        private Button arrowDownButton;
        [SerializeField]
        private Button buyButton;
        [SerializeField]
        private Button cancelButton;
        [SerializeField]
        private Image itemIcon;
        [SerializeField]
        private TMP_Text itemNameText;
        [SerializeField]
        private TMP_Text itempriceText;
        [SerializeField]
        private TMP_InputField buyAmountText;
        [SerializeField]
        private GameObject confirmBuyBoxContent;

        private UserDataManager userDataManager;

        private float itemPrice;

        private string itemId;
        // Start is called before the first frame update

        private void Awake()
        {
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
        }
        void Start()
        {
            buyAmountText.onValueChanged.AddListener(delegate 
            { 
                OnItemAmountChanged();
            });
            buyAmountText.onEndEdit.AddListener(delegate
            {
                OnEndEdititemAmount();
            });
            arrowUpButton.onClick.AddListener(() =>
            {
                if (userDataManager.UserData.UserDataInventory.ConsumableItems.TryGetValue(itemId,out int itemAmount))
                {
                    if (float.Parse(buyAmountText.text) < 99 - itemAmount)
                    {
                        buyAmountText.text = (float.Parse(buyAmountText.text) + 1).ToString();
                    }
                }
                else
                {
                    if (float.Parse(buyAmountText.text) < 99)
                    {
                        buyAmountText.text = (float.Parse(buyAmountText.text) + 1).ToString();
                    }
                }
            });
            arrowDownButton.onClick.AddListener(() =>
            {
                if (float.Parse(buyAmountText.text) > 0)
                {
                    buyAmountText.text = (float.Parse(buyAmountText.text) - 1).ToString();
                }
            });
            buyButton.onClick.AddListener(() => 
            {
                bool buyComplete = true;
                if (userDataManager.UserData.Coins < itemPrice * float.Parse(buyAmountText.text))
                {
                    buyComplete = false;
                }
                if (buyComplete)
                {
                    var newCoin = userDataManager.UserData.Coins - itemPrice * float.Parse(buyAmountText.text);
                    userDataManager.UserData.UpdateCoin(newCoin);
                    userDataManager.UserData.UserDataInventory.AddConsumeItem(itemId, int.Parse(buyAmountText.text));
                    shopPanelController.UpdatePlayerCoinsText();
                    buyShopPanelController.UpdateItemBag();
                    buyAmountText.text = "0";
                    shopPanelController.HideConfirmBuyShop();
                }
            });
            cancelButton.onClick.AddListener(() =>
            {
                shopPanelController.HideConfirmBuyShop();
            });
        }

        public void SetUpConfirmBuyBox(Sprite newItemIconSprite,string newItemName,float newPrice,string newItemId)
        {
            itemIcon.sprite = newItemIconSprite;
            itemNameText.text = newItemName;
            itemPrice = newPrice;
            itemId = newItemId;
            buyAmountText.text = "0";
            itempriceText.text = "0 s";
        }

        public void OnItemAmountChanged()
        {
            var exists = userDataManager.UserData.UserDataInventory.ConsumableItems.TryGetValue(itemId, out int itemAmount);
            if(float.Parse(buyAmountText.text) > 99 - itemAmount && exists)
            {
                buyAmountText.text = (99 - itemAmount).ToString();
            }
            if(float.Parse(buyAmountText.text) < 0)
            {
                buyAmountText.text = (0).ToString();
            }
            var resultPrice = float.Parse(buyAmountText.text) * itemPrice;
            if(resultPrice > userDataManager.UserData.Coins)
            {
                buyAmountText.text = Mathf.FloorToInt(userDataManager.UserData.Coins / itemPrice).ToString();
            }
            itempriceText.text = (itemPrice * float.Parse(buyAmountText.text)).ToString() + " s";
        }

        public void OnEndEdititemAmount()
        {
            if (string.IsNullOrEmpty(buyAmountText.text))
            {
                buyAmountText.text = (0).ToString();
            }
        }

        public void OpenConfirmBox()
        {
            shopPanelController.ResetAllTrigger();
            shopPanelController.ShowConfirmBuyShop();
        }
    }
}
