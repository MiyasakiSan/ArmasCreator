using System.Collections;
using System.Collections.Generic;
using ArmasCreator.GameData;
using ArmasCreator.UserData;
using ArmasCreator.Utilities;
using UnityEngine;
using Unity.Netcode;

namespace ArmasCreator.GameMode
{
    public class GameModeController : NetworkBehaviour
    {
        public enum GameModes
        {
            none, 
            Singleplayer, 
            MultiPlayer,
        }

        public GameModes Mode { get; private set; }

        public bool IsSinglePlayerMode => Mode == GameModes.Singleplayer;

        /// <summary>
        /// Decide if current mode is multi player mode
        /// </summary>
        public bool IsMultiPlayerMode => Mode == GameModes.MultiPlayer;

        /// <summary>
        ///  Decide if this client is master client
        /// </summary>
        public bool IsMasterClient => IsSinglePlayerMode || NetworkManager.IsClient;

        private void Awake()
        {
            SharedContext.Instance.Add(this);
            DontDestroyOnLoad(this);

            Mode = GameModes.Singleplayer;
        }

        public void SelectMode(GameModes value)
        {
            Mode = value;

            //DO something
        }
    }

}
