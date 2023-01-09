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

       public void ShowBuyShopPanel()
       {

       }
       public void ShowSellShopPanel()
       {

       }

       public void UpdatePlayerCoinsText()
       {
            playerCoinsText.text = userDataManager.UserData.Coins.ToString() + " s";
        }
    }
}
