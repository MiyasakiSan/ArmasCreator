using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArmasCreator.GameData
{
    public enum itemType
    {
        Consumable,
        Equipable,
    }

    public enum subType
    {
        None,
        Weapon,
        Helmet,
        Shirt,
        Glove,
        Pant,
        Shoes,
        Stamina,
        Health
    }

    [Serializable]
    public class ItemInfoModel
    {
        [JsonProperty("id")]
        public string ID;

        [JsonProperty("item_type")]
        public itemType Type;

        [JsonProperty("sub_type")]
        public subType SubType;

        [JsonProperty("sell_price")]
        public float SellPrice;

        [JsonProperty("buy_price")]
        public float BuyPrice;

        [JsonProperty("asset")]
        public string Asset;

        [JsonProperty("icon")]
        public string IconName;

        [JsonProperty("atlas_icon")]
        public string AtlasIcon;
    }

    [Serializable]
    public class ConsumeableItemModel : ItemInfoModel
    {
        [JsonProperty("consume_percent")]
        public float ConsumePercent;
    }

    [Serializable]
    public class EquipableItemModel : ItemInfoModel
    {
        [JsonProperty("atk")]
        public float ATK;

        [JsonProperty("def")]
        public float DEF;

        [JsonProperty("vit")]
        public float VIT;

        [JsonProperty("crit")]
        public float CRT;
    }

}
