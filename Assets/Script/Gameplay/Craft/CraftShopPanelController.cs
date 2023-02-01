using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArmasCreator.GameData;
using ArmasCreator.UserData;
using ArmasCreator.Utilities;
using ArmasCreator.Gameplay.UI;
using TMPro;

namespace ArmasCreator.UI
{
    public class CraftShopPanelController : MonoBehaviour
    {
        [SerializeField]
        private CraftBoxController craftBoxController;

        [SerializeField]
        private List<Button> bannerButtons;

        [SerializeField]
        private List<BannerSlot> bannerSlots;

        [SerializeField]
        private TMP_Text playerCurrency;

        private UserDataManager userDataManager;

        private void Awake()
        {
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
        }
        public void Start()
        {
            bannerButtons[0].onClick.AddListener(() => 
            {
                craftBoxController.SetCurrentSubType(SubType.Lion);
                DeselectedAllBannerSlot();
                bannerSlots[0].OnSelectedBanner();
                ShowCraftBox();
            });
            bannerButtons[1].onClick.AddListener(() =>
            {
                craftBoxController.SetCurrentSubType(SubType.Shrimp);
                DeselectedAllBannerSlot();
                bannerSlots[1].OnSelectedBanner();
                ShowCraftBox();
            });
            UpdatePlayerCoin();
        }

        public void DeselectedAllBannerSlot()
        {
            foreach(BannerSlot bannerSlot in bannerSlots)
            {
                bannerSlot.OnDeselectedBanner();
            }
        }

        public void ShowCraftBox()
        {
            craftBoxController.UpdateCraftBoxItem();
        }

        public void UpdatePlayerCoin()
        {
            playerCurrency.text = userDataManager.UserData.Coins.ToString() + " s";
        }
    }
}
