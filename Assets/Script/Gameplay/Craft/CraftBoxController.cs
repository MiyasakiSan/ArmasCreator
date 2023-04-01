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
        private TMP_Text equipmentNameText;

        [SerializeField]
        private CraftButton craftButton;

        [SerializeField]
        private ItemPreviewCameraController itemPreviewCameraController;

        private List<EquipmentTypeNode> equipmentTypeNodes = new List<EquipmentTypeNode>();

        private SubType currentSubType;

        private string currentRecipeSelectedID;

        private string[] equipmentTypes = { "gunblade", "helmet", "shirt", "glove", "pant", "shoes" };

        private GameDataManager gameDataManager = SharedContext.Instance.Get<GameDataManager>();

        private UserDataManager userDataManager = SharedContext.Instance.Get<UserDataManager>();

        void Awake()
        {

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
                                itemPreviewCameraController.OnChangeItemPreview(gameDataManager.GetItemSubType(itemID), equipmentType);
                            });
                            ins_EquipmentTypeNode.SetCraftShopPanelController(craftShopPanelController);
                            var userRecipes = userDataManager.UserData.UserDataInventory.Recipes;

                            ins_EquipmentTypeNode.SetIsUnlockEquipmentTypeNode(true);

                            foreach (string recipeID in userRecipes)
                            {
                                if (recipeID == itemID)
                                {
                                    ins_EquipmentTypeNode.SetIsUnlockEquipmentTypeNode(false);
                                    continue;
                                }
                            }

                            var recipeEquipment = gameDataManager.TryGetRecipeInfo(itemID, out var recipeInfo);

                            var subType = gameDataManager.GetItemSubType(recipeInfo.Craft_item_id);

                            var userEquipment = userDataManager.UserData.UserDataInventory.EquipableItems[subType];

                            ins_EquipmentTypeNode.SetIsOwned(false);

                            ins_EquipmentTypeNode.SetEquipmentNameText(equipmentNameText);

                            foreach (var equipment in userEquipment.UnlockIds)
                            {
                                if(recipeInfo.Craft_item_id == equipment)
                                {
                                    ins_EquipmentTypeNode.SetIsOwned(true);
                                    ins_EquipmentTypeNode.SetIsUnlockEquipmentTypeNode(false);
                                    continue;
                                }
                            }
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
