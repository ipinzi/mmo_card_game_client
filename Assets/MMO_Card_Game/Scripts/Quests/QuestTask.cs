using System;
using System.Collections.Generic;
using MMO_Card_Game.Scripts.Cards;
using MMO_Card_Game.Scripts.Items;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MMO_Card_Game.Scripts.Quests
{
    public class QuestTask : ScriptableObject
    {

        [ReadOnly]
        [TableColumnWidth(10, true)]
        public string id = Guid.NewGuid().ToString();
        [TableColumnWidth(100, true), OnValueChanged(nameof(RenameAsset))]
        public string taskName;
        [TextArea,TableColumnWidth(200, true)]
        public string taskText = "";
        [TableColumnWidth(10, true)]
        public int money;
        [TableColumnWidth(100, true)]
        public List<Item> itemRewards;
        [TableColumnWidth(100, true)]
        public List<Card> cardRewards;
        [PreviewField]
        [TableColumnWidth(20, true)]
        public Sprite taskImage;
        [ReadOnly]
        [TableColumnWidth(30, true)]
        public string taskType;

        private void RenameAsset()
        {
            #if UNITY_EDITOR
                var path = AssetDatabase.GetAssetPath(this);
                AssetDatabase.RenameAsset(path, taskName);
            #endif
        }
    }
    
    [Serializable]
    public class QuestTaskSerializableData
    {
        public string id;
        public string taskName;
        //public string taskText = "";
        public string taskType = "";
        public int moneyReward = 0;
        public List<string> itemRewards;
        public List<string> cardRewards;
        
        public QuestTaskSerializableData(QuestTask task)
        {
            id = task.id;
            taskName = task.taskName;
            //taskText = task.taskText;
            taskType = task.taskType;
            moneyReward = task.money;
            itemRewards = new List<string>();
            cardRewards = new List<string>();
            foreach (var item in task.itemRewards)
            {
                itemRewards.Add(item.id);
            }
            foreach (var card in task.cardRewards)
            {
                cardRewards.Add(card.id);
            }
        }
    }
}