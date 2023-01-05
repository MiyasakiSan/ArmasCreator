using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ArmasCreator.UI
{
    public class ShopPanelController : MonoBehaviour
    {
        [Header("MainShopPanel")]
        [SerializeField]
        private Button buyButton;
        [SerializeField]
        private Button sellButton;

        [Header("SubControllerScript")]
        [SerializeField]
        private BuyShopPanelController buyShopPanelController;

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
