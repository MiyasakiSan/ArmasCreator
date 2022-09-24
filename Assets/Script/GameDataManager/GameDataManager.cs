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


namespace ArmasCreator.GameData
{
    public class GameDataManager : MonoBehaviour
    {
        [SerializeField]
        private const string gameDataPath = "GameData/armasCreator-gameData";

        private bool isInitialize;
        public bool IsIniialize => isInitialize;

        private float targetValue;
        private float currentValue;
        [SerializeField]
        private Slider loadingSlider; 
        private void Awake()
        {
            StartInitialize();
        }

        void Start()
        {

        }

        void Update()
        {

        }

        private void StartInitialize()
        {
            DontDestroyOnLoad(this);
            SharedContext.Instance.Add(this);
            LoadLocalGameConfig();
            StartCoroutine(LoadScene());
            
        }

        IEnumerator LoadScene()
        {
            yield return null;
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Mainmenu");
            asyncOperation.allowSceneActivation = false;

            while (!asyncOperation.isDone)
            {
                targetValue = asyncOperation.progress / 0.9f;
                currentValue = Mathf.MoveTowards(currentValue, targetValue, 0.25f * Time.deltaTime);
                loadingSlider.value = currentValue;

                if (Mathf.Approximately(currentValue, 1))
                {
                        asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }
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
        }
    }
}

