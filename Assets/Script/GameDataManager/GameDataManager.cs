using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ArmasCreator.Utilities;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ArmasCreator.UI;
using ArmasCreator.UserData;

namespace ArmasCreator.GameData
{
    public class GameDataManager : MonoBehaviour
    {
        [SerializeField]
        private const string gameDataPath = "GameData/armasCreator-gameData";

        private bool isInitialize;
        public bool IsIniialize => isInitialize;

        #region General Data

        [GameData("challenges_mode_config")]
        private Dictionary<string, ChallengeModeModel> challengesModeConfig;

        [GameData("consumable_items")]
        private Dictionary<string, ConsumeableItemModel> consumeableitemInfos;

        [GameData("equipable_items")]
        private Dictionary<string, EquipableItemModel> equipableitemInfos;

        [GameData("monster_parts")]
        private Dictionary<string, ItemInfoModel> monsterPartInfos;

        [GameData("recipes")]
        private Dictionary<string, RecipeModel> recipeInfos;

        [GameData("init_equip_items")]
        private string[] initEquipItems;

        [GameData("init_consume_items")]
        private string[] initConsumeItems;

        [GameData("achievement_config")]
        private Dictionary<string, AchievementModel> achievementConfig;

        [GameData("dialogue_config")]
        private Dictionary<string, DialogueModel> dialogueConfig;

        [GameData("quest_config")]
        private Dictionary<string, QuestModel> questConfig;

        [GameData("character_config")]
        private Dictionary<string, CharacterModel> characterConfig;

        [GameData("game_info")]
        private GameInfoModel gameInfo;

        private LoadingPopup loadingPopup;

        #endregion

        private void Awake()
        {
            StartInitialize();
        }

        void Start()
        {
            loadingPopup = SharedContext.Instance.Get<LoadingPopup>();
            loadingPopup?.LoadSceneAsync("Mainmenu");
        }

        private void StartInitialize()
        {
            DontDestroyOnLoad(this);
            SharedContext.Instance.Add(this);
            LoadLocalGameConfig();
        }

        private void LoadLocalGameConfig()
        {
            var jsonFile = Resources.Load(gameDataPath) as TextAsset;

            ParseAndApplyGameDataFromJson(jsonFile.text);
        }

        private void ParseAndApplyGameDataFromJson(string jsonText)
        {
            var gameDataDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonText);

            var gameDataFields = typeof(GameDataManager).GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.GetCustomAttributes(typeof(GameDataAttribute), true).Any()).ToList();

            foreach (var item in gameDataFields)
            {
                try
                {
                    var dictName = item.GetCustomAttribute(typeof(GameDataAttribute)) as GameDataAttribute;
                    var fieldGameDataString = gameDataDictionary[dictName.JsonElement];
                    var fieldType = item.FieldType;
                    var stringValue = fieldGameDataString.ToString();
                    var itemValue = JsonConvert.DeserializeObject(stringValue, fieldType);
                    item.SetValue(this, itemValue);

                }
                catch (Exception ex)
                {
                    Debug.Log($"Error parsing data: {item.Name}");
                    Debug.LogException(ex);
                }
            }

            Debug.Log("================  Load GameData From Json Complete  =================");

            Debug.Log(challengesModeConfig);
        }

        #region Challenge Info
        public void GetAllChallengeMapId(out List<string> mapId)
        {
            mapId = new List<string>();

            foreach (KeyValuePair<string, ChallengeModeModel> challengeInfo in challengesModeConfig)
            {
                if (mapId.Contains(challengeInfo.Value.MapID)) { continue; }

                mapId.Add(challengeInfo.Value.MapID);
            }
        }

        public void GetAllChallengeInfoFromMapId(string mapId, out List<ChallengeModeModel> challengeInfoList)
        {
            challengeInfoList = new List<ChallengeModeModel>();

            foreach (KeyValuePair<string, ChallengeModeModel> challengeInfo in challengesModeConfig)
            {
                if (challengeInfo.Value.MapID == mapId)
                {
                    challengeInfoList.Add(challengeInfo.Value);
                }
            }
        }

        public bool TryGetSelectedChallengeModeInfo(string challengeId, out ChallengeModeModel challengeModeInfo)
        {
            bool exist = challengesModeConfig.TryGetValue(challengeId, out challengeModeInfo);

            if (!exist)
            {
                challengeModeInfo = null;
                return false;
            }
            else
            {
                return true;
            }
        } 
        #endregion

        #region Item info
        public Dictionary<SubType, EquipmentModel> GetAllInitEquipItems()
        {
            var initItemDict = new Dictionary<SubType, EquipmentModel>();

            foreach (string itemId in initEquipItems)
            {
                var subType = equipableitemInfos[itemId].SubType;
                initItemDict.Add(subType, new EquipmentModel 
                { 
                    EquippedId = itemId,
                    UnlockIds = new List<string>() { itemId }
                });
            }

            return initItemDict;
        }

        public Dictionary<string, int> GetAllInitConsumeItems()
        {
            var initItemDict = new Dictionary<string, int>();

            foreach (string itemId in initConsumeItems)
            {
                initItemDict.Add(itemId, 5);
            }

            return initItemDict;
        } 

        public bool TryGetEquipItemInfoWithSubType(string id, SubType itemType, out EquipableItemModel equipItemInfo)
        {
            bool exist = equipableitemInfos.TryGetValue(id, out EquipableItemModel itemInfo);

            if (!exist) 
            {
                Debug.LogError($"{id} doesn't exist in equipable items");

                equipItemInfo = new EquipableItemModel();

                return false; 
            }

            if (itemInfo.SubType != SubType.Weapon)
            {
                equipItemInfo = new EquipableItemModel();

                return false;
            }

            equipItemInfo = itemInfo;
            return true;
        }

        public bool TryGetEquipItemInfoById(string id, out EquipableItemModel equipItemInfo)
        {
            bool exist = equipableitemInfos.TryGetValue(id, out EquipableItemModel itemInfo);

            if (!exist)
            {
                Debug.LogError($"{id} doesn't exist in equipable items");

                equipItemInfo = new EquipableItemModel();

                return false;
            }

            equipItemInfo = itemInfo;
            return true;
        }

        public bool TryGetConsumeItemInfo(string id, out ConsumeableItemModel consumeItemInfo)
        {
            bool exist = consumeableitemInfos.TryGetValue(id, out ConsumeableItemModel itemInfo);

            if (!exist)
            {
                Debug.LogError($"{id} doesn't exist in consumable items");

                consumeItemInfo = new ConsumeableItemModel();

                return false;
            }

            consumeItemInfo = itemInfo;
            return true;
        }

        public bool TryGetCraftItemInfo(string id, out ItemInfoModel monsterPartInfo)
        {
            bool exist = monsterPartInfos.TryGetValue(id, out ItemInfoModel itemInfo);

            if (!exist)
            {
                Debug.LogError($"{id} doesn't exist in craft items");

                monsterPartInfo = new ItemInfoModel();

                return false;
            }

            monsterPartInfo = itemInfo;
            return true;
        }

        public bool TryGetRecipeInfo(string id, out RecipeModel recipeInfo)
        {
            bool exist = recipeInfos.TryGetValue(id, out RecipeModel itemInfo);

            if (!exist)
            {
                Debug.LogError($"{id} doesn't exist in recipe items");

                recipeInfo = new RecipeModel();

                return false;
            }

            recipeInfo = itemInfo;
            return true;
        }

        public ItemType GetItemType(string id)
        {
            bool exist = consumeableitemInfos.ContainsKey(id);

            if (exist)
            {
                return ItemType.Consumable;
            }

            exist = equipableitemInfos.ContainsKey(id);

            if (exist)
            {
                return ItemType.Equipable;
            }

            exist = monsterPartInfos.ContainsKey(id);

            if (exist)
            {
                return ItemType.Craftable;
            }

            return ItemType.Recipe;
        }

        public SubType GetItemSubType(string id)
        {
            bool exist = consumeableitemInfos.ContainsKey(id);

            if (exist)
            {
                return consumeableitemInfos[id].SubType;
            }

            exist = equipableitemInfos.ContainsKey(id);

            if (exist)
            {
                return equipableitemInfos[id].SubType;
            }

            exist = monsterPartInfos.ContainsKey(id);

            if (exist)
            {
                return monsterPartInfos[id].SubType;
            }

            exist = recipeInfos.ContainsKey(id);

            if (exist)
            {
                return recipeInfos[id].SubType;
            }

            return SubType.None;
        }

        public bool TryGetItemInfoWithType(string id,ItemType type, out ItemInfoModel itemInfo)
        {
            switch (type)
            {
                case ItemType.Consumable:
                    {
                        if (!consumeableitemInfos.ContainsKey(id)) 
                        {
                            itemInfo = new ItemInfoModel();
                            return false;
                        }

                        itemInfo = consumeableitemInfos[id];
                        return true;
                    }
                case ItemType.Equipable:
                    {
                        if (!equipableitemInfos.ContainsKey(id))
                        {
                            itemInfo = new ItemInfoModel();
                            return false;
                        }

                        itemInfo = equipableitemInfos[id];
                        return true;
                    }
                case ItemType.Craftable:
                    {
                        if (!monsterPartInfos.ContainsKey(id))
                        {
                            itemInfo = new ItemInfoModel();
                            return false;
                        }

                        itemInfo = monsterPartInfos[id];
                        return true;
                    }
                case ItemType.Recipe:
                    {
                        if (!recipeInfos.ContainsKey(id))
                        {
                            itemInfo = new ItemInfoModel();
                            return false;
                        }

                        itemInfo = recipeInfos[id];
                        return true;
                    }
                default:
                    {
                        itemInfo = new ItemInfoModel();
                        return false;
                    }
            }
        }

        public List<string> GetAllItemIdByType(ItemType itemType)
        {
            List<string> itemIds = new List<string>();

            if (itemType == ItemType.Consumable)
            {
                foreach (string Id in consumeableitemInfos.Keys)
                {
                    itemIds.Add(Id);
                }
            }
            else if (itemType == ItemType.Equipable)
            {
                foreach (string Id in equipableitemInfos.Keys)
                {
                    itemIds.Add(Id);
                }
            }
            else if (itemType == ItemType.Craftable)
            {
                foreach (string Id in monsterPartInfos.Keys)
                {
                    itemIds.Add(Id);
                }
            }
            else if (itemType == ItemType.Recipe)
            {
                foreach (string Id in recipeInfos.Keys)
                {
                    itemIds.Add(Id);
                }
            }

            return itemIds;
        }


        #endregion

        #region Achievement
        public Dictionary<string, int> GetAllInitAchievements()
        {
            var initAchievement = new Dictionary<string, int>();

            foreach (KeyValuePair<string, AchievementModel> achievement in achievementConfig)
            {
                initAchievement.Add(achievement.Key, 0);
            }

            return initAchievement;
        } 

        public bool TryGetAchievementInfo(string achievementId, out AchievementModel achievementInfo)
        {
            bool exist = achievementConfig.TryGetValue(achievementId, out AchievementModel achievement);

            if (!exist) 
            { 
                achievementInfo = new AchievementModel();
                return false; 
            }

            achievementInfo = achievement;
            return true;
        }

        #endregion

        public bool TryGetDialogueInfo(string dialogueId, out DialogueModel dialogueInfo)
        {
            bool exist = dialogueConfig.TryGetValue(dialogueId, out DialogueModel dialogue);

            if (!exist)
            {
                Debug.LogError($"{dialogueId} doesn't exist in dialogue config");

                dialogueInfo = new DialogueModel();

                return false;
            }

            dialogueInfo = dialogue;
            return true;
        }
    }
}

