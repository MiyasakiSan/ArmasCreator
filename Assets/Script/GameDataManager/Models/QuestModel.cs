using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArmasCreator.GameData
{
    public enum QuestType
    {
        None,
        Main,
        Side
    }

    public enum QuestSubType
    {
        None,
        Elimination,
        Destination,
        Conversation,
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

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("type")]
        public QuestType Type { get; set; }

        [JsonProperty("sub_Type")]
        public QuestSubType SubType { get; set; }

        [JsonProperty("rewards")]
        public Dictionary<string, int> Rewards { get; set; }

        [JsonProperty("previous_quest_id")]
        public List<string> PreviousQuestId { get; set; }

        [JsonProperty("next_quest_id")]
        public string NextQuestId { get; set; }

        [JsonProperty("start_dialogue_id")]
        public string StartDialogueId { get; set; }

        [JsonProperty("end_dialogue_id")]
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
        public float TimeAmount;
    }

    public class RequestQuestModel : QuestModel
    {
        [JsonProperty("request_item_ids")]
        public Dictionary<string, int> RequestItemIds;
    }

    public class ConversationQuestModel : QuestModel
    {
        [JsonProperty("npc_id")]
        public string NpcId;
    }
}
