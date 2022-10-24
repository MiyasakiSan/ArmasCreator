using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArmasCreator.GameData
{
    [Serializable]
    public class GameInfoModel 
    {
        [JsonProperty("result_time_rating")]
        public Dictionary<string, float> ResultTimeRating;

        [JsonProperty("damage_delt_rating")]
        public Dictionary<string, float> DamageDeltRating;

        [JsonProperty("damage_taken_rating")]
        public Dictionary<string, float> DamageTakenRating;
    }
}
