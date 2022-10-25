using System.Collections;
using System.Collections.Generic;
using ArmasCreator.GameData;
using UnityEngine;
using ArmasCreator.GameMode;

namespace ArmasCreator.Gameplay
{
    public class QuestInfo 
    {
        public string MapId { get; private set; }
        public string SceneName { get; private set; }
        public float InitATK { get; private set; }
        public float InitSpeed { get; private set; }
        public float InitHp { get; private set; }
        public float Duration { get; private set; }

        public QuestInfo(string mapId, string sceneName, float initAtk, float initSpeed, float initHp,float duration)
        {
            MapId = mapId;
            SceneName = sceneName;
            InitATK = initAtk;
            InitSpeed = initSpeed;
            InitHp = initHp;
            Duration = duration;
        }

    }
}

   
