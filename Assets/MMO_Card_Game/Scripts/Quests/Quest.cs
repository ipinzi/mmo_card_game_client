using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Quests
{
    public class Quest : ScriptableObject
    {
        [ReadOnly]
        [TableColumnWidth(10, true)]
        public string id = Guid.NewGuid().ToString();
        [TableColumnWidth(100, true), OnValueChanged(nameof(RenameAsset))]
        public string questName;
        [TableColumnWidth(150, true)]
        public List<QuestTask> tasks;
        [TextArea]
        [TableColumnWidth(150, true)]
        public string questText = "";
        [PreviewField]
        [TableColumnWidth(20, true)]
        public Sprite questImage;
        [ReadOnly]
        [TableColumnWidth(30, true)]
        public string questType;

        [HideInInspector] public int progress = 0;
        
        private void RenameAsset()
        {
            #if UNITY_EDITOR
                var path = AssetDatabase.GetAssetPath(this);
                AssetDatabase.RenameAsset(path, questName);
            #endif
        }
    }
    
    [Serializable]
    public class QuestSerializableData
    {
        public string id;
        public string questName;
        //public string questText = "";
        public string questType = "";
        public List<QuestTaskSerializableData> tasks;
        
        public QuestSerializableData(Quest quest)
        {
            id = quest.id;
            questName = quest.questName;
            //questText = quest.questText;
            tasks = new List<QuestTaskSerializableData>();
            foreach (var task in quest.tasks)
            {
                var serializableTask = new QuestTaskSerializableData(task);
                tasks.Add(serializableTask);
            }
            questType = quest.questType;
        }
    }
}