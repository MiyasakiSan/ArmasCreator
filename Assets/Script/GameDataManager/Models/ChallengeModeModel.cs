using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArmasCreator.GameData
{
    [Serializable]
    public class ChallengeModeModel 
    {
        [JsonProperty("map_id")]
        public string MapID;

        [JsonProperty("scene_name")]
        public string SceneName;

        [JsonProperty("default_hp")]
        public float DefaultHp;

        [JsonProperty("default_atk")]
        public float DefaultAtk;

        [JsonProperty("default_speed")]
        public float DefaultSpeed;

        [JsonProperty("bad_score")]
        public int BadScore;

        [JsonProperty("okay_score")]
        public int OkayScore;

        [JsonProperty("good_score")]
        public int GoodScore;

        [JsonProperty("duration")]
        public float Duration;
    }
}



   
