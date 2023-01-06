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
        private TMP_Text playerCurrencyText;

        [Header("SubControllerScript")]
        [SerializeField]
        private BuyShopPanelController buyShopPanelController;

        private UserDataModel userDataModel;
        private void Awake()
        {
            userDataModel = SharedContext.Instance.Get<UserDataModel>();
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
            playerCurrencyText.text = userDataModel.Coin.ToString();
        }

        // Update is called once per frame
        void Update()
        {

        }

       public void ShowBuyShopPanel()
       {

       }
       public void ShowSellShopPanel()
       {

       }
    }
}
