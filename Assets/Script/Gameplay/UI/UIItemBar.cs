using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArmasCreator.GameData;
using ArmasCreator.UserData;
using UnityEngine.UI;
using ArmasCreator.UI;
using TMPro;

namespace ArmasCreator.Gameplay.UI
{
    public class UIItemBar : MonoBehaviour
    {
        [SerializeField]
        private ConsumableButton currentItem;

        [SerializeField]
        private ConsumableButton nextItem;

        [SerializeField]
        private ConsumableButton previousItem;

        [SerializeField]
        private TextMeshProUGUI itemName;

        [SerializeField]
        private GameObject content;

        private int currentIndex;
        private int previousIndex;
        private int nextIndex;

        private List<string> allConsumableItemIds = new List<string>();

        private UserDataManager userDataManager;

        public void Init(UserDataManager userDataManager)
        {
            this.userDataManager = userDataManager;

            allConsumableItemIds = userDataManager.UserData.UserDataInventory.GetAllConsumableItemIds();
            currentIndex = 0;

            Populate();
        }

        public void Hide()
        {
            content.SetActive(false);
        }

        public void Show()
        {
            content.SetActive(true);
        }

        private void SetIndex()
        {
            if (currentIndex < 0)
            {
                currentIndex = allConsumableItemIds.Count - 1;
            }
            else if (currentIndex > allConsumableItemIds.Count - 1)
            {
                currentIndex = 0;
            }

            previousIndex = currentIndex - 1;
            nextIndex = currentIndex + 1;

            if (previousIndex < 0)
            {
                previousIndex = allConsumableItemIds.Count - 1;
            }

            if(nextIndex > allConsumableItemIds.Count - 1)
            {
                nextIndex = 0;
            }
        }

        private void UseItem()
        {
            Debug.Log($"Use {allConsumableItemIds[currentIndex]}");
        }

        public void Populate()
        {
            SetIndex();

            previousItem.SetDisplayItem(allConsumableItemIds[previousIndex]);
            nextItem.SetDisplayItem(allConsumableItemIds[nextIndex]);
            currentItem.SetDisplayItem(allConsumableItemIds[currentIndex]);

            itemName.text = currentItem.ItemInfo.Name;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                UseItem();
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
            {
                currentIndex++;
                Populate();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
            {
                currentIndex--;
                Populate();
            }
        }
    }
}

