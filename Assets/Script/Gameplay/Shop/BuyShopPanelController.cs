using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArmasCreator.GameData;
using ArmasCreator.Utilities;

namespace ArmasCreator.UI
{
    public class BuyShopPanelController : MonoBehaviour
    {
        [SerializeField]
        private BuyItemNode buyItemNode;
        [SerializeField]
        private Transform buyBoxContent;

        private GameDataManager gameDataManager;

        private List<BuyItemNode> buyItemNodes;

        private void Awake()
        {
            gameDataManager = SharedContext.Instance.Get<GameDataManager>();
        }

        void Start()
        {
            ClearAllItemShop();
            PopulateBuyItemNode();
        }

        public void PopulateBuyItemNode()
        {
            List<string> consumableItemIds = gameDataManager.GetAllItemIdByType(ItemType.Consumable);
            foreach (string id in consumableItemIds)
            {
                var insBuyItemNode = Instantiate(buyItemNode.gameObject, buyBoxContent).GetComponent<BuyItemNode>();
                insBuyItemNode.SetDisplayItem(id);
                buyItemNodes.Add(insBuyItemNode);
            }
        }

        public void ClearAllItemShop()
        {
            foreach(BuyItemNode buyItemNodeForClear in buyItemNodes)
            {
                Destroy(buyItemNodeForClear.gameObject);
            }
        }
    }
}
