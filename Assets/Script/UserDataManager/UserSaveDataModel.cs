using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ArmasCreator.Gameplay;

namespace ArmasCreator.UserData
{
    [Serializable]
    public class UserSaveDataModel 
    {
        public string UserId;

        public int Coins;

        public List<UserSavePresetModel> AllSavePresets;
    }

    public class UserSavePresetModel
    {
        public string MapId;
        public List<QuestInfo> Presets = new List<QuestInfo>(); 
    }
}

