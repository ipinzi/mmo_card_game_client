using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Quests
{
    [CreateAssetMenu(menuName = "MMO Card Game/Quest Database", fileName = "Quest Database", order = 0)]
    public class QuestDatabase : ScriptableObject
    {
        [InfoBox("This is the Quest Database, all quests available in game should appear here. " +
                 "If for some reason a quest is not available in this list then you can add it from the [Quests] " +
                 "dropdown in explorer view on the left, otherwise it will not appear in the game or export in JSON.")]
        [Searchable, TableList(DrawScrollView = true, MinScrollViewHeight = 200),OnValueChanged(nameof(CheckDuplicates))]
        public List<Quest> quests;

        public Quest GetQuest(string id)
        {
            return Instantiate(quests.FirstOrDefault(quest => quest.id == id));
        }
        public Quest GetQuestByName(string questName)
        {
            return Instantiate(quests.FirstOrDefault(quest => quest.questName == questName));
        }
        private void CheckDuplicates()
        {
            quests = quests.Distinct().ToList();
        }

        #if UNITY_EDITOR
            [Button("Export to JSON")]
            private void ExportToJson()
            {
                var path = EditorUtility.SaveFilePanel(
                    "Export Quest Database Data as JSON",
                    "",
                    "questDatabase.json",
                    "json");

                if (path.Length != 0)
                {
                    var serializableData = ConvertToSeralizableData();
                    var json = JsonUtility.ToJson(serializableData, true);
                    File.WriteAllText(path, json);
                    
                    Debug.Log("Quest Database Json saved.");
                }
            }
        #endif
        
        private QuestDatabaseSerializableData ConvertToSeralizableData()
        {
            var questsJsonData = new QuestDatabaseSerializableData();
            foreach (var quest in quests)
            {
                var newQuest = new QuestSerializableData(quest);
                questsJsonData.quests.Add(newQuest);
            }

            return questsJsonData;
        }
    }
    [System.Serializable]
    public class QuestDatabaseSerializableData
    {
        public List<QuestSerializableData> quests;

        public QuestDatabaseSerializableData()
        {
            quests = new List<QuestSerializableData>();
        }
    }
}