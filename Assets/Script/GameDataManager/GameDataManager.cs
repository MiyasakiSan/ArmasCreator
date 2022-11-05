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

        public void GetAllChallengeMapId(out List<string> mapId )
        {
            mapId = new List<string>();

            foreach(KeyValuePair<string,ChallengeModeModel> challengeInfo in challengesModeConfig)
            {
                if(mapId.Contains(challengeInfo.Value.MapID)) { continue; }

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
    }
}

