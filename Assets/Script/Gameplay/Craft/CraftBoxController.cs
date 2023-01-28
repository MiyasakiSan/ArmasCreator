using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArmasCreator.GameData;
using ArmasCreator.UserData;
using ArmasCreator.Utilities;
using TMPro;

namespace ArmasCreator.UI
{
    public class CraftBoxController : MonoBehaviour
    {
        [SerializeField]
        private CraftShopPanelController craftShopPanelController;

        [SerializeField]
        private EquipmentTypeNode equipmentTypeNodePrefab;

        [SerializeField]
        private Transform equipmentTypeBar;

        [SerializeField]
        private Transform itemRequireBox;

        [SerializeField]
        private TMP_Text moneyRequire;

        [SerializeField]
        private CraftButton craftButton;

        private List<EquipmentTypeNode> equipmentTypeNodes = new List<EquipmentTypeNode>();

        private SubType currentSubType;

        private string currentRecipeSelectedID;

        private string[] equipmentTypes = { "gunblade", "helmet", "shirt", "glove", "pant", "shoes" };

        private GameDataManager gameDataManager;

        private UserDataManager userDataManager;

        void Awake()
        {
            gameDataManager = SharedContext.Instance.Get<GameDataManager>();
            userDataManager = SharedContext.Instance.Get<UserDataManager>();
        }

        public void SetCurrentSubType(SubType subType)
        {
            currentSubType = subType;
        }

        public void UpdateCraftBoxItem()
        {
            ClearAllEquipmentTypeNode();
            foreach (string equipmentType in equipmentTypes)
            { 
                var ins_EquipmentTypeNode = Instantiate(equipmentTypeNodePrefab, equipmentTypeBar).GetComponent<EquipmentTypeNode>();
                var ins_EquipmentTypeNodeButton = ins_EquipmentTypeNode.GetComponent<Button>();

                ins_EquipmentTypeNode.SetItemRequireBox(itemRequireBox);

                var Recipes = gameDataManager.GetAllItemIdByType(ItemType.Recipe);

                foreach (string itemID in Recipes)
                {
                    var itemIDSplit = itemID.Split('_');

                    if(itemIDSplit[1] == equipmentType)
                    {
                        if (gameDataManager.GetItemSubType(itemID) == currentSubType)
                        {
                            ins_EquipmentTypeNode.SetDisplayItem(itemID);
                            ins_EquipmentTypeNode.SetMoneyRequire(moneyRequire);
                            ins_EquipmentTypeNode.SetCraftButton(craftButton);
                            ins_EquipmentTypeNodeButton.onClick.AddListener(() =>
                            {
                                DeSelectedAllButton();
                                ClearAllItemRequireNode();
                                ins_EquipmentTypeNode.OnSelectedEquipmentTypeNode();
                                currentRecipeSelectedID = itemID;
                            });
                        }
                    }
                }

                equipmentTypeNodes.Add(ins_EquipmentTypeNode);
            }
            
        }

        public void DeSelectedAllButton()
        {
            if (equipmentTypeNodes != null)
            {
                foreach (EquipmentTypeNode equipmentTypeNode in equipmentTypeNodes)
                {
                    equipmentTypeNode.OnDeSelectedEquipmentTypeNode();
                }
            }
        }

        public void ClearAllEquipmentTypeNode()
        {
            if (equipmentTypeNodes != null)
            {
                foreach (EquipmentTypeNode equipmentTypeNode in equipmentTypeNodes)
                {
                    Destroy(equipmentTypeNode.gameObject);
                }
                equipmentTypeNodes.Clear();
            }
        }

        public void ClearAllItemRequireNode()
        {
            var itemRequireNodes = itemRequireBox.GetComponentsInChildren<ItemRequireNode>();

            foreach (ItemRequireNode itemRequireNode in itemRequireNodes)
            {
                Destroy(itemRequireNode.gameObject);
            }
        }
    }
}
