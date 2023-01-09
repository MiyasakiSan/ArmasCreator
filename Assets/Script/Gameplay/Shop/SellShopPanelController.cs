using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArmasCreator.GameData;
using ArmasCreator.UserData;
using ArmasCreator.Utilities;

namespace ArmasCreator.UI
{
    public class SellShopPanelController : MonoBehaviour
    {
        [SerializeField]
        private Transform inventoryContent;
        [SerializeField]
        private SellConsumableItemNode sellConsumableItemNodePrefab;
        [SerializeField]
        private SellCraftableItemNode sellCraftableItemNodePrefab;
        [SerializeField]
        private GameObject blankNodePrefab;
        [SerializeField]
        private ConfirmSellBoxController confirmSellBoxController;

        private GameDataManager gameDataManager;

        private UserDataManager userDataManager;

        private List<SellConsumableItemNode> sellConsumableItemNodes = new List<SellConsumableItemNode>();

        private List<SellCraftableItemNode> sellCraftableItemNodes = new List<SellCraftableItemNode>();

        private List<GameObject> blankNodes = new List<GameObject>();

        private void Awake()
        {
            gameDataManager = SharedContext.Instance.Get<GameDataManager>();
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
        }

        // Start is called before the first frame update
        void Start()
        {
            UpdateItemBag();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PopulateSellItemNode()
        {
            var consumableItemIds = userDataManager.UserData.UserDataInventory.ConsumableItems;
            foreach (string id in consumableItemIds.Keys)
            {
                var insSellItemNode = Instantiate(sellConsumableItemNodePrefab.gameObject, inventoryContent).GetComponent<SellConsumableItemNode>();
                insSellItemNode.SetDisplayItem(id);
                insSellItemNode.SetUPNewConfirmBuyBox(confirmSellBoxController);
                insSellItemNode.onClick.AddListener(() =>
                {
                    confirmSellBoxController.OpenConfirmBox();
                    insSellItemNode.SetUpConfirmBuyBox();
                });
                sellConsumableItemNodes.Add(insSellItemNode);
            }
            var craftableItemIds = userDataManager.UserData.UserDataInventory.CraftableItems;
            foreach (string id in craftableItemIds.Keys)
            {
                var insSellItemNode = Instantiate(sellCraftableItemNodePrefab.gameObject, inventoryContent).GetComponent<SellCraftableItemNode>();
                insSellItemNode.SetDisplayItem(id);
                insSellItemNode.SetUPNewConfirmBuyBox(confirmSellBoxController);
                insSellItemNode.onClick.AddListener(() =>
                {
                    confirmSellBoxController.OpenConfirmBox();
                    insSellItemNode.SetUpConfirmBuyBox();
                });
                sellCraftableItemNodes.Add(insSellItemNode);
            }
            var allNodeThatCanSell = sellConsumableItemNodes.Count + sellCraftableItemNodes.Count;
            for (int blankNodeCount = 0; blankNodeCount < 48 - allNodeThatCanSell; blankNodeCount++)
            {
                var insBlankNode = Instantiate(blankNodePrefab, inventoryContent);
                blankNodes.Add(insBlankNode);
            }
        }
        public void ClearAllItemBag()
        {
            if (sellConsumableItemNodes.Count > 0)
            {
                foreach (SellConsumableItemNode ItemBagNodeForClear in sellConsumableItemNodes)
                {
                    Destroy(ItemBagNodeForClear.gameObject);
                }
                sellConsumableItemNodes.Clear();
            }

            if (sellCraftableItemNodes.Count > 0)
            {
                foreach (SellCraftableItemNode ItemBagNodeForClear in sellCraftableItemNodes)
                {
                    Destroy(ItemBagNodeForClear.gameObject);
                }
                sellCraftableItemNodes.Clear();
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
            ClearAllItemBag();
            PopulateSellItemNode();
        }
    }
}