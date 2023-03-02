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
        private ConsumableButton preNextItem;

        [SerializeField]
        private ConsumableButton prePreviousItem;

        [SerializeField]
        private TextMeshProUGUI itemName;

        [SerializeField]
        private GameObject content;

        [SerializeField]
        private Animator itemBarAnimator;

        [SerializeField]
        private CombatRpgManager combatController;

        private int currentIndex;
        private int previousIndex;
        private int prePreviousIndex;
        private int nextIndex;
        private int preNextIndex;
        private List<string> allConsumableItemIds = new List<string>();

        private bool canUseItem;

        private UserDataManager userDataManager;

        public void Init(UserDataManager userDataManager)
        {
            this.userDataManager = userDataManager;

            allConsumableItemIds = userDataManager.UserData.UserDataInventory.GetAllConsumableItemIds();
            currentIndex = 0;
            canUseItem = true;

            Populate(currentIndex);
        }

        public void Hide()
        {
            content.SetActive(false);
            canUseItem = false;
        }

        public void Show()
        {
            content.SetActive(true);
            canUseItem = true;
        }

        private void SetIndex()
        {
            currentIndex %= allConsumableItemIds.Count;

            previousIndex = currentIndex - 1;
            prePreviousIndex = currentIndex - 2;
            nextIndex = currentIndex + 1;
            preNextIndex = currentIndex + 2;

            if (currentIndex < 0)
            {
                currentIndex = allConsumableItemIds.Count + currentIndex;
                currentIndex %= allConsumableItemIds.Count;
            }

            if (previousIndex < 0)
            {
                previousIndex = allConsumableItemIds.Count + previousIndex;
                previousIndex %= allConsumableItemIds.Count;
            }

            if(nextIndex > allConsumableItemIds.Count - 1)
            {
                nextIndex %= allConsumableItemIds.Count;
            }

            if (prePreviousIndex < 0)
            {
                prePreviousIndex = allConsumableItemIds.Count + prePreviousIndex;
                prePreviousIndex %= allConsumableItemIds.Count;
            }

            if (preNextIndex > allConsumableItemIds.Count - 1)
            {
                preNextIndex %= allConsumableItemIds.Count;
            }
        }

        private void UseItem()
        {
            if (!canUseItem) { return; }

            if (combatController.isUsingItem) { return; }

            Debug.Log($"Use {allConsumableItemIds[currentIndex]}");

            combatController.UseItem(allConsumableItemIds[currentIndex]);
        }

        public void Populate(int plusIndex)
        {
            currentIndex += plusIndex;
            SetIndex();

            previousItem.SetDisplayItem(allConsumableItemIds[Mathf.Abs(previousIndex)]);
            nextItem.SetDisplayItem(allConsumableItemIds[Mathf.Abs(nextIndex)]);
            currentItem.SetDisplayItem(allConsumableItemIds[Mathf.Abs(currentIndex)]);
            prePreviousItem.SetDisplayItem(allConsumableItemIds[Mathf.Abs(prePreviousIndex)]);
            preNextItem.SetDisplayItem(allConsumableItemIds[Mathf.Abs(preNextIndex)]);

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
                itemBarAnimator.SetTrigger("Right");
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
            {
                itemBarAnimator.SetTrigger("Left");
            }
        }
    }
}

