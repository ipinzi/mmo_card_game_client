using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Cards
{
    [CreateAssetMenu(menuName = "MMO Card Game/Card Database", fileName = "Card Database", order = 0)]
    public class CardDatabase : ScriptableObject
    {
        [Searchable, TableList(DrawScrollView = true, MinScrollViewHeight = 200),OnValueChanged(nameof(CheckDuplicates))]
        public List<Card> cards;

        public Card GetCard(string id)
        {
            return cards.FirstOrDefault(card => card.id == id);
        }
        private void CheckDuplicates()
        {
            cards = cards.Distinct().ToList();
        }


        [Button("Export to JSON")]
        private void ExportToJson()
        {
            var path = EditorUtility.SaveFilePanel(
                "Export Item Database Data as JSON",
                "",
                "itemDatabase.json",
                "json");

            if (path.Length != 0)
            {
                var serializableData = ConvertToSeralizableData();
                var json = JsonUtility.ToJson(serializableData, true);
                File.WriteAllText(path, json);
                
                Debug.Log("Card Database Json saved.");
            }
        }
        private CardDatabaseSerializableData ConvertToSeralizableData()
        {
            var cardsJsonData = new CardDatabaseSerializableData();
            foreach (var card in cards)
            {
                var newCard = new CardSerializableData(card);
                cardsJsonData.cards.Add(newCard);
            }

            return cardsJsonData;
        }
    }
    [System.Serializable]
    public class CardDatabaseSerializableData
    {
        public List<CardSerializableData> cards;

        public CardDatabaseSerializableData()
        {
            cards = new List<CardSerializableData>();
        }
    }
}
