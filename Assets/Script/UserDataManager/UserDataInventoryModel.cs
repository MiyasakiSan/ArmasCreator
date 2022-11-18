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

        public Dictionary<string, bool> EquipableItems = new Dictionary<string, bool>();

        public Dictionary<string, int> CraftableItems = new Dictionary<string, int>();

        public List<string> Recipes = new List<string>();

        public void Init(UserDataManager userData)
        {
            this.userData = userData;
        }

        public void SetupUserInventory(UserSaveDataModel saveModel)
        {
            ConsumableItems = new Dictionary<string, int>();
            EquipableItems = new Dictionary<string, bool>();
            CraftableItems = new Dictionary<string, int>();
            Recipes = new List<string>();

            ConsumableItems = saveModel.ConsumableItems;
            EquipableItems = saveModel.EquipableItems;
            CraftableItems = saveModel.CraftableItems;
            Recipes = saveModel.recipes;
        }

        public void AddItem(string itemId, int amount)
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
                        AddEquipItem(itemId);
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

        public void AddEquipItem(string itemId)
        {
            if (EquipableItems.ContainsKey(itemId))
            {
                EquipableItems[itemId] = false;
            }
            else
            {
                EquipableItems.Add(itemId, false);
            }

            userData.UserData.UpdateSaveInventory(ConsumableItems, EquipableItems, Recipes, CraftableItems);
        }

        public void RemoveEquipItem(string itemId)
        {
            if (EquipableItems.ContainsKey(itemId))
            {
                EquipableItems.Remove(itemId);
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

        public void SetEquipItem(string itemId)
        {
            bool exist = EquipableItems.TryGetValue(itemId,out bool isEquipped);

            if(!exist) 
            { 
                Debug.LogError("WTF IS GOING ON U RE trying to equip not owned item");
                return;
            }

            EquipableItems[itemId] = true;

            userData.UserData.UpdateSaveInventory(ConsumableItems, EquipableItems, Recipes, CraftableItems);
        }

        public List<string> GetAllEquipItemIds()
        {
            List<string> equipItemList = new List<string>();

            foreach(KeyValuePair<string, bool> item in EquipableItems)
            {
                if (!item.Value) { continue; }

                equipItemList.Add(item.Key);
            }

            return equipItemList;
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

