using System.Collections;
using System.Collections.Generic;
using ArmasCreator.GameData;
using ArmasCreator.UserData;
using ArmasCreator.Utilities;
using UnityEngine;

namespace ArmasCreator.Gameplay
{
    public class GameplayController : MonoBehaviour
    {
        public delegate void OnLoadProgressionChangeCallback(float currentProgress);
        public OnLoadProgressionChangeCallback OnLoadProgressionChange;

        public delegate void OnLoadingCompletedCallback();
        public OnLoadingCompletedCallback OnLoadingCompleted;

        public delegate void OnStatePreGameCallBack();
        public OnStatePreGameCallBack OnStatePreGame;

        public delegate void OnStateStartCallBack();
        public OnStateStartCallBack OnStateStart;

        public delegate void OnStatePlayCallBack();
        public OnStatePlayCallBack OnStatePlay;

        public delegate void OnStateResultCallback();
        public OnStateResultCallback OnStateResult;

        public PlayerGroupController PlayerGroupController { get; private set; }
        public GameDataManager GameDataManager { get; private set; }
        public UserDataManager UserDataManager { get; private set; }

        private void Awake()
        {
            SharedContext.Instance.Add(this);

            PlayerGroupController = new PlayerGroupController();
        }

        void Start()
        {

        }

        void Update()
        {

        }
    }
}
    
