using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArmasCreator.GameData;
using ArmasCreator.UserData;
using ArmasCreator.Utilities;

namespace ArmasCreator.UI
{
    public class BuyShopPanelController : MonoBehaviour
    {
        [SerializeField]
        private BuyItemNode buyItemNode;
        [SerializeField]
        private Transform buyBoxContent;
        [SerializeField]
        private ConfirmBuyBoxController confirmBuyBoxController;
        [SerializeField]
        private ConsumableButton consumableButtonPrefab;
        [SerializeField]
        private GameObject blankNodePrefab;
        [SerializeField]
        private Transform inventoryBoxContent;

        private GameDataManager gameDataManager;

        private UserDataManager userDataManager;

        private List<BuyItemNode> buyItemNodes = new List<BuyItemNode>();

        private List<ConsumableButton> consumableButtonNodes = new List<ConsumableButton>();

        private List<GameObject> blankNodes = new List<GameObject>();

        private void Awake()
        {
            gameDataManager = SharedContext.Instance.Get<GameDataManager>();
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
        }

        void Start()
        {

        }

        public void PopulateBuyItemNode()
        {
            List<string> consumableItemIds = gameDataManager.GetAllItemIdByType(ItemType.Consumable);
            foreach (string id in consumableItemIds)
            {
                var insBuyItemNode = Instantiate(buyItemNode.gameObject, buyBoxContent).GetComponent<BuyItemNode>();
                insBuyItemNode.SetDisplayItem(id);
                insBuyItemNode.SetUPNewConfirmBuyBox(confirmBuyBoxController);
                insBuyItemNode.onClick.AddListener(() =>
                {
                    confirmBuyBoxController.OpenConfirmBox();
                    insBuyItemNode.SetUpConfirmBuyBox();
                });
                buyItemNodes.Add(insBuyItemNode);
            }
        }

        public void ClearAllItemShop()
        {
            if (buyItemNodes.Count > 0)
            {
                foreach (BuyItemNode buyItemNodeForClear in buyItemNodes)
                {
                    Destroy(buyItemNodeForClear.gameObject);
                }
                buyItemNodes.Clear();
            }
        }

        public void PopulatePlayerItemBag()
        {
            var userAllConsumableItemIds = userDataManager.UserData.UserDataInventory.GetAllConsumableItemIds();
            foreach(string itemId in userAllConsumableItemIds)
            {
                var insConsumableButton = Instantiate(consumableButtonPrefab.gameObject, inventoryBoxContent).GetComponent<ConsumableButton>();
                insConsumableButton.SetDisplayItem(itemId);
                consumableButtonNodes.Add(insConsumableButton);
            }
            for(int blankNodeCount = 0;blankNodeCount < 48 - userAllConsumableItemIds.Count;blankNodeCount++)
            {
                var insBlankNode = Instantiate(blankNodePrefab, inventoryBoxContent);
                blankNodes.Add(insBlankNode);
            }
        }

        public void ClearAllItemBag()
        {
            if(consumableButtonNodes.Count > 0)
            {
                foreach (ConsumableButton ItemBagNodeForClear in consumableButtonNodes)
                {
                    Destroy(ItemBagNodeForClear.gameObject);
                }
                consumableButtonNodes.Clear();
            }

            if (blankNodes.Count > 0)
            {
                foreach (GameObject ItemBagNodeForClear in blankNodes)
                {
                    Destroy(ItemBagNodeForClear.gameObject);
                }
                blankNodes.Clear();
            }
        }

        public void UpdateItemBag()
        {
            ClearAllItemShop();
            PopulateBuyItemNode();
            ClearAllItemBag();
            PopulatePlayerItemBag();
        }
    }
}
