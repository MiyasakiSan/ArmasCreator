using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArmasCreator.GameData
{
    public enum QuestType
    {
        Main,
        Side
    }

    public enum QuestSubType
    {
        Elimination,
        Destination,
        Survival,
        Request
    }

    [Serializable]
    public class DialogueModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("dialogue_sequences")]
        public List<SequenceModel> DialogSequences;
    }

    public class SequenceModel
    {
        [JsonProperty("id")]
        public string CharacterId;

        [JsonProperty("name")]
        public string CharacterName;

        [JsonProperty("sentence")]
        public string Sentence;
    }


    [Serializable]
    public class QuestModel 
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public QuestType Type { get; set; }

        [JsonProperty("sub_Type")]
        public QuestSubType SubType { get; set; }

        [JsonProperty("rewards")]
        public Dictionary<string, int> Rewards { get; set; }

        [JsonProperty("next_quest_id")]
        public string NextQuestId { get; set; }

        [JsonProperty("start_dailogue_id")]
        public string StartDialogueId { get; set; }

        [JsonProperty("end_dailogue_id")]
        public string EndDialogueId { get; set; }
    }

    public class EliminationQuestModel : QuestModel
    {
        [JsonProperty("target_monsters")]
        public Dictionary<string ,int> TargetMonsters;
    }

    public class DestinationQuestModel : QuestModel
    {
        [JsonProperty("target_destinations")]
        public List<string> TargetDestination;
    }

    public class SurvivalQuestModel : QuestModel
    {
        [JsonProperty("time_amount")]
        public List<string> TargetDestination;
    }

    public class RequestQuestModel : QuestModel
    {
        [JsonProperty("request_item_ids")]
        public Dictionary<string, int> RequestItemIds;
    }
}
