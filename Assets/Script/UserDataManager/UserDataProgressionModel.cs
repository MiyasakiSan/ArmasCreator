using ArmasCreator.LocalFile;
using UnityEngine;
using ArmasCreator.Gameplay;
using System.Collections.Generic;

namespace ArmasCreator.UserData
{
    public class UserDataProgressionModel 
    {
        public Dictionary<string, int> Achievements;

        private UserDataManager userData;
        public void Init(UserDataManager userData)
        {
            this.userData = userData;
        }

        public void SetupUserInventory(UserSaveDataModel saveModel)
        {
            Achievements = new Dictionary<string, int>();

            Achievements = saveModel.Achievements;
        }
    }
}

