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
            playerCoinsText.text = userDataManager.UserData.Coins.ToString() + " s";
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ShowMainShop()
        {
            buyShopPanelAnimator.SetTrigger("showMainShop");
        }
        public void HideMainShop()
        {
            HideConfirmSellShop();
            HideConfirmBuyShop();
            HideSellShopPanel();
            HideBuyShopPanel();
            buyShopPanelAnimator.SetTrigger("hideMainShop");
        }
        public void ShowBuyShopPanel()
        {
            buyShopPanelAnimator.SetTrigger("showBuyShop");
        }
        public void ShowSellShopPanel()
        {
            buyShopPanelAnimator.SetTrigger("showSellShop");
        }
        public void ShowConfirmBuyShop()
        {
            buyShopPanelAnimator.SetTrigger("showConfirmBuy");
        }
        public void ShowConfirmSellShop()
        {
            buyShopPanelAnimator.SetTrigger("showConfirmSell");
        }
        public void HideBuyShopPanel()
        {
            buyShopPanelAnimator.SetTrigger("hideBuyShop");
        }
        public void HideSellShopPanel()
        {
            buyShopPanelAnimator.SetTrigger("hideSellShop");
        }
        public void HideConfirmBuyShop()
        {
            buyShopPanelAnimator.SetTrigger("hideConfirmBuy");
        }
        public void HideConfirmSellShop()
        {
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
