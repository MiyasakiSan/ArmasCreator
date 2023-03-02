using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace ArmasCreator.Gameplay.UI
{
    public class BannerSlot : MonoBehaviour
    {
        [SerializeField]
        private GameObject selectedBorderGameObject;
        [SerializeField]
        private CanvasGroup bannerSlotCanvasGroup;
        [SerializeField]
        private Button bannerButton;
        [SerializeField]
        private QuestPanelController questPanelController;

        [SerializeField]
        private string mapId;

        public Action<string> BannerMapId;

        private void Awake()
        {
            if (questPanelController != null)
            {
                bannerButton.onClick.AddListener(() =>
                {
                    questPanelController.OnClickBanner();
                    OnSelectedBanner();
                });
            }
        }

        public void SetMapId(string mapId)
        {
            this.mapId = mapId;
        }
        public void OnSelectedBanner()
        {
            selectedBorderGameObject.SetActive(true);
            bannerSlotCanvasGroup.alpha = 1;
            if (!String.IsNullOrEmpty(mapId))
            {
                BannerMapId?.Invoke(mapId);
            }
        }

        public void OnDeselectedBanner()
        {
            selectedBorderGameObject.SetActive(false);
            bannerSlotCanvasGroup.alpha = 0.5f;
        }

        private void OnDestroy()
        {
            if (questPanelController != null)
            {
                bannerButton.onClick.RemoveListener(() =>
                {
                    questPanelController.OnClickBanner();
                    OnSelectedBanner();
                });
            }
        }
    }
}
