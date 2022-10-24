using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArmasCreator.Utilities;

namespace ArmasCreator.UserData
{
    public class UserDataManager : MonoBehaviour
    {
        //TODO : Implement UserData for save game progression and inventory

        private UserDataModel userData;
        public UserDataModel UserData => userData;

        void Awake()
        {
            DontDestroyOnLoad(this);
            SharedContext.Instance.Add(this);

            LoadData();
        }

        private void LoadData()
        {
            userData = new UserDataModel();
            userData.Init(this);

            //Debug.LogError("Need to implement user data");
        }

        private void OnDestroy()
        {
            //userData.Dispose();
        }
    }
}
   
