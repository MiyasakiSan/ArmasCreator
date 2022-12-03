using ArmasCreator.Utilities;
using System.Collections.Generic;
using ArmasCreator.GameData;
using System;
using UnityEngine;

namespace ArmasCreator.UserData
{
    public class UserDataInventoryModel 
    {
        private UserDataManager userData;

        public Dictionary<string, int> ConsumableItems = new Dictionary<string, int>();

        public Dictionary<SubType, EquipmentModel> EquipableItems = new Dictionary<SubType, EquipmentModel>();

        public Dictionary<string, int> CraftableItems = new Dictionary<string, int>();

        public List<string> Recipes = new List<string>();

        public void Init(UserDataManager userData)
        {
            this.userData = userData;
        }

        public void SetupUserInventory(UserSaveDataModel saveModel)
        {
            ConsumableItems = new Dictionary<string, int>();
            EquipableItems = new Dictionary<SubType, EquipmentModel>();
            CraftableItems = new Dictionary<string, int>();
            Recipes = new List<string>();

            ConsumableItems = saveModel.ConsumableItems;
            EquipableItems = saveModel.EquipableItems;
            CraftableItems = saveModel.CraftableItems;
            Recipes = saveModel.recipes;
        }

        public void AddItem(string itemId, SubType subType = SubType.None, int amount = 0)
        {
            var gameDataManager = SharedContext.Instance.Get<GameDataManager>();

            ItemType itemType = gameDataManager.GetItemType(itemId);

            switch (itemType)
            {
                case ItemType.Consumable:
                    {
                        AddConsumeItem(itemId, amount);
                        break;
                    }
                case ItemType.Equipable:
                    {
                        AddEquipItem(itemId, subType);
                        break;
                    }
                case ItemType.Craftable:
                    {
                        AddCraftItem(itemId, amount);
                        break;
                    }
                case ItemType.Recipe:
                    {
                        AddRecipeItem(itemId);
                        break;
                    }
                default:
                    {
                        Debug.LogError("ITEM TYPE MUCH WRONG BRUHHH");
                        break;
                    }
            }
        }

        public void AddConsumeItem(string itemId, int amount)
        {
            if (ConsumableItems.ContainsKey(itemId))
            {
                ConsumableItems[itemId] += amount;
            }
            else
            {
                ConsumableItems.Add(itemId, amount);
            }

            userData.UserData.UpdateSaveInventory(ConsumableItems, EquipableItems, Recipes, CraftableItems);
        }

        public void RemoveConsumeItem(string itemId, int amount)
        {
            if (ConsumableItems.ContainsKey(itemId))
            {
                if(ConsumableItems[itemId] - amount == 0)
                {
                    ConsumableItems.Remove(itemId);
                }
                else
                {
                    ConsumableItems[itemId] -= amount;
                }
            }
            else
            {
                Debug.LogError("Cant remove unowned item");
            }

            userData.UserData.UpdateSaveInventory(ConsumableItems, EquipableItems, Recipes, CraftableItems);
        }

        public void AddEquipItem(string itemId, SubType subType)
        {
            if (EquipableItems.ContainsKey(subType))
            {
                EquipableItems[subType].UnlockIds.Add(itemId);
            }
            else
            {
                EquipableItems.Add(subType, new EquipmentModel 
                { 
                    EquippedId = itemId,
                    UnlockIds = new List<string>() { itemId }
                });
            }

            userData.UserData.UpdateSaveInventory(ConsumableItems, EquipableItems, Recipes, CraftableItems);
        }

        public void RemoveEquipItem(string itemId, SubType subType)
        {
            if (EquipableItems.ContainsKey(subType))
            {
                EquipableItems[subType].UnlockIds.Remove(itemId);
            }
            else
            {
                Debug.LogError("Cant remove unowned item");
            }

            userData.UserData.UpdateSaveInventory(ConsumableItems, EquipableItems, Recipes, CraftableItems);
        }

        public void AddCraftItem(string itemId, int amount)
        {
            if (CraftableItems.ContainsKey(itemId))
            {
                CraftableItems[itemId] += amount;
            }
            else
            {
                CraftableItems.Add(itemId, amount);
            }

            userData.UserData.UpdateSaveInventory(ConsumableItems, EquipableItems, Recipes, CraftableItems);
        }

        public void RemoveCraftItem(string itemId, int amount)
        {
            if (CraftableItems.ContainsKey(itemId))
            {
                if (CraftableItems[itemId] - amount == 0)
                {
                    CraftableItems.Remove(itemId);
                }
                else
                {
                    CraftableItems[itemId] -= amount;
                }
            }
            else
            {
                Debug.LogError("Cant remove unowned item");
            }

            userData.UserData.UpdateSaveInventory(ConsumableItems, EquipableItems, Recipes, CraftableItems);
        }

        public void AddRecipeItem(string itemId)
        {
            if (Recipes.Contains(itemId))
            {
                Debug.LogError("Cant add owned recipe");
            }
            else
            {
                Recipes.Add(itemId);
            }

            userData.UserData.UpdateSaveInventory(ConsumableItems, EquipableItems, Recipes, CraftableItems);
        }

        public void SetEquipItem(string itemId, SubType subType)
        {
            bool exist = EquipableItems.TryGetValue(subType, out var equipmentInfo);

            if(!exist) 
            { 
                Debug.LogError("WTF IS GOING ON U RE trying to equip not owned item");
                return;
            }

            EquipableItems[subType].EquippedId = itemId;

            userData.UserData.UpdateSaveInventory(ConsumableItems, EquipableItems, Recipes, CraftableItems);
        }

        public List<string> GetAllEquipItemIds()
        {
            List<string> equipItemList = new List<string>();

            foreach(KeyValuePair<SubType, EquipmentModel> item in EquipableItems)
            {
                equipItemList.Add(item.Value.EquippedId);
            }

            return equipItemList;
        }

        public List<string> GetAllConsumableItemIds()
        {
            List<string> consumeItemList = new List<string>();

            foreach (KeyValuePair<string, int> item in ConsumableItems)
            {
                consumeItemList.Add(item.Key);
            }

            return consumeItemList;
        }

        public EquipableItemModel GetEquipItemInfo(SubType itemType)
        {
            var gameDataManager = SharedContext.Instance.Get<GameDataManager>();
            var equipItemList = GetAllEquipItemIds();

            foreach(var itemId in equipItemList)
            {
                if (!gameDataManager.TryGetEquipItemInfoWithSubType(itemId, itemType, out EquipableItemModel equipItemInfo)) { continue; }

                return equipItemInfo;
            }

            Debug.LogError("Can't get any equip weapon");

            return new EquipableItemModel();
        }
    }
}

