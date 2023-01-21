using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArmasCreator.Utilities;
using ArmasCreator.UserData;
using TMPro;

namespace ArmasCreator.UI
{
    public class ShopPanelController : MonoBehaviour
    {
        [Header("MainShopPanel")]
        [SerializeField]
        private Button buyButton;
        [SerializeField]
        private Button sellButton;
        [SerializeField]
        private TMP_Text playerCoinsText;

        [Header("SubControllerScript")]
        [SerializeField]
        private BuyShopPanelController buyShopPanelController;

        [Header("Animator")]
        [SerializeField]
        private Animator buyShopPanelAnimator;

        [Header("BuyShop")]
        [SerializeField]
        private Button buyShopBackButton;

        [Header("SellShop")]
        [SerializeField]
        private Button sellShopBackButton;

        private UserDataManager userDataManager;
        private void Awake()
        {
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
        }
        // Start is called before the first frame update
        void Start()
        {
            buyButton.onClick.AddListener(() =>
            {
                ShowBuyShopPanel();
            });
            sellButton.onClick.AddListener(() =>
            {
                ShowSellShopPanel();
            });
            buyShopBackButton.onClick.AddListener(() =>
            {
                HideBuyShopPanel();
            });
            sellShopBackButton.onClick.AddListener(() =>
            {
                HideSellShopPanel();
            }); 
        }

        private void OnDestroy()
        {
            buyButton.onClick.RemoveAllListeners();
            sellButton.onClick.RemoveAllListeners();
            buyShopBackButton.onClick.RemoveAllListeners();
            sellShopBackButton.onClick.RemoveAllListeners();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ShowMainShop()
        {
            buyShopPanelAnimator.SetTrigger("showMainShop");
            playerCoinsText.text = userDataManager.UserData.Coins.ToString() + " s";
        }
        public void HideMainShop()
        {
            ResetAllTrigger();
            buyShopPanelAnimator.SetTrigger("hideConfirmBuy");
            buyShopPanelAnimator.SetTrigger("hideConfirmSell");
            buyShopPanelAnimator.SetTrigger("hideBuyShop");
            buyShopPanelAnimator.SetTrigger("hideSellShop");
            buyShopPanelAnimator.SetTrigger("hideMainShop");
        }
        public void ShowBuyShopPanel()
        {
            ResetAllTrigger();
            buyShopPanelAnimator.SetTrigger("showBuyShop");
        }
        public void ShowSellShopPanel()
        {
            ResetAllTrigger();
            buyShopPanelAnimator.SetTrigger("showSellShop");
        }
        public void ShowConfirmBuyShop()
        {
            ResetAllTrigger();
            buyShopPanelAnimator.SetTrigger("showConfirmBuy");
        }
        public void ShowConfirmSellShop()
        {
            ResetAllTrigger();
            buyShopPanelAnimator.SetTrigger("showConfirmSell");
        }
        public void HideBuyShopPanel()
        {
            ResetAllTrigger();
            HideConfirmBuyShop();
            buyShopPanelAnimator.SetTrigger("hideBuyShop");
        }
        public void HideSellShopPanel()
        {
            ResetAllTrigger();
            HideConfirmSellShop();
            buyShopPanelAnimator.SetTrigger("hideSellShop");
        }
        public void HideConfirmBuyShop()
        {
            ResetAllTrigger();
            buyShopPanelAnimator.SetTrigger("hideConfirmBuy");
        }
        public void HideConfirmSellShop()
        {
            ResetAllTrigger();
            buyShopPanelAnimator.SetTrigger("hideConfirmSell");
        }

        public void ResetAllTrigger()
        {
            buyShopPanelAnimator.ResetTrigger("showBuyShop");
            buyShopPanelAnimator.ResetTrigger("showSellShop");
            buyShopPanelAnimator.ResetTrigger("showConfirmBuy");
            buyShopPanelAnimator.ResetTrigger("showConfirmSell");
            buyShopPanelAnimator.ResetTrigger("hideBuyShop");
            buyShopPanelAnimator.ResetTrigger("hideSellShop");
            buyShopPanelAnimator.ResetTrigger("hideConfirmBuy");
            buyShopPanelAnimator.ResetTrigger("hideConfirmSell");
        }
        public void UpdatePlayerCoinsText()
        {
            playerCoinsText.text = userDataManager.UserData.Coins.ToString() + " s";
        }
    }
}
