using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArmasCreator.Gameplay;
using ArmasCreator.Utilities;

namespace ArmasCreator.UI
{
    public class ItemReceivedPanelController : MonoBehaviour
    {
        private GameplayController gameplayController;

        private Dictionary<string, int> itemReceivedIdAndAmountDictionary = new Dictionary<string, int>();

        [SerializeField]
        private CraftableResult craftableResultNodePrefab;

        [SerializeField]
        private Transform Content;

        void Awake()
        {
            if (gameplayController == null)
            {
                gameplayController = SharedContext.Instance.Get<GameplayController>();
                gameplayController.OnReceivedItem += addItemToItemReceivedDictionary;
                gameplayController.OnReceivedAllItem += PopulateAllReceivedItem;
            }
        }

        private void addItemToItemReceivedDictionary(string ID , int Amount)
        {
            itemReceivedIdAndAmountDictionary.Add(ID, Amount);
        }

        private void PopulateAllReceivedItem()
        {
            if(itemReceivedIdAndAmountDictionary.Count == 0)
            {
                return;
            }

            foreach (var receivedItem in itemReceivedIdAndAmountDictionary)
            {
                CraftableResult ins_CraftableResultNode = Instantiate(craftableResultNodePrefab, Content);
                ins_CraftableResultNode.interactable = false;
                ins_CraftableResultNode.OnInstantiate();
                ins_CraftableResultNode.SetDisplayItem(receivedItem.Key,receivedItem.Value);
            }
        }
    }
}
