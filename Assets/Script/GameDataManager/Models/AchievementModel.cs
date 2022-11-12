using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArmasCreator.GameData
{
    [Serializable]
    public class AchievementModel 
    {
        [JsonProperty("id")]
        public string ID;

        [JsonProperty("header")]
        public string Header;

        [JsonProperty("progress")]
        public int Progress;

        [JsonProperty("rewards")]
        public Dictionary<string,int> Rewards;
    }
}


