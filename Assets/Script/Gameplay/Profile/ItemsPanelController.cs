using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArmasCreator.UI;
using ArmasCreator.GameData;
using ArmasCreator.Utilities;
using ArmasCreator.UserData;

namespace ArmasCreator.UI
{
    public class ItemsPanelController : MonoBehaviour
    {
        [SerializeField]
        private GameObject consumableItemNodePrefab;
        [SerializeField]
        private GameObject craftableItemNodePrefab;
        [SerializeField]
        private GameObject blankItemNodePrefab;
        [SerializeField]
        private Transform contentTransform;
        [SerializeField]
        private GameObject mainContent;

        private UserDataManager userDataManager;

        private List<GameObject> listButton = new List<GameObject>();

        // Start is called before the first frame update
        void Awake()
        {
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
        }

        public void SetUpItemPanel()
        {
            if (!mainContent.activeSelf)
            {
                mainContent.SetActive(true);
            }
            PopulateInventoryNode();
        }

        public void DeactiveItemPanel()
        {
            if (listButton != null)
            {
                foreach (GameObject itemButton in listButton)
                {
                    Destroy(itemButton);
                }
                listButton.Clear();
            }
        }

        public void OnFadeIn()
        {

        }

        public void OnFadeOut()
        {

        }

        public void PopulateInventoryNode()
        {
            var consumableItems = userDataManager.UserData.UserDataInventory.ConsumableItems;
            var craftableItems = userDataManager.UserData.UserDataInventory.CraftableItems;

            foreach (string itemId in consumableItems.Keys)
            {
                GameObject insInventoryNode = Instantiate(consumableItemNodePrefab, contentTransform);
                ConsumableButton insConsumable_btn = insInventoryNode.GetComponent<ConsumableButton>();
                insConsumable_btn.SetDisplayItem(itemId);
                listButton.Add(insConsumable_btn.gameObject);
            }
            foreach (string itemId in craftableItems.Keys)
            {
                GameObject insInventoryNode = Instantiate(craftableItemNodePrefab, contentTransform);
                CraftableButton insCraftable_btn = insInventoryNode.GetComponent<CraftableButton>();
                insCraftable_btn.SetDisplayItem(itemId);
                listButton.Add(insCraftable_btn.gameObject);
            }
            for (int BlankNode = 0; BlankNode < 48 - consumableItems.Keys.Count + craftableItems.Keys.Count; BlankNode++)
            {

                listButton.Add(Instantiate(blankItemNodePrefab, contentTransform));
            }
        }
    }
}
