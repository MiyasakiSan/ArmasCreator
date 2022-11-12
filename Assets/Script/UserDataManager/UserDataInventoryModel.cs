using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmasCreator.LocalFile;
using UnityEngine;
using ArmasCreator.Gameplay;

namespace ArmasCreator.UserData
{
    public class UserDataInventoryModel 
    {
        private UserDataManager userData;

        public Dictionary<string, int> ConsumableItems = new Dictionary<string, int>();

        public Dictionary<string, bool> EquipableItems = new Dictionary<string, bool>();

        public void Init(UserDataManager userData)
        {
            this.userData = userData;
        }

        public void SetupUserInventory(UserSaveDataModel saveModel)
        {
            ConsumableItems = new Dictionary<string, int>();
            EquipableItems = new Dictionary<string, bool>();

            ConsumableItems = saveModel.ConsumableItems;
            EquipableItems = saveModel.EquipableItems;
        }
    }
}

