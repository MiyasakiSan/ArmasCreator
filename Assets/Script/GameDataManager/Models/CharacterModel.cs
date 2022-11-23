using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArmasCreator.GameData
{
    [Serializable]
    public class CharacterModel 
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("icon")]
        public string IconName { get; set; }

        [JsonProperty("atlas_icon")]
        public string AtlasIcon { get; set; }

    }
}

    
