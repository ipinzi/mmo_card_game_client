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
        [InfoBox("This is the Card Database, all cards available in game should appear here. " +
                 "If for some reason a card is not available in this list then you can add it from the [Cards/Card Type] " +
                 "dropdown in explorer view on the left, otherwise it will not appear in the game or export in JSON.")]
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

        #if UNITY_EDITOR
        [Button("Export to JSON")]
        private void ExportToJson()
        {
            var path = EditorUtility.SaveFilePanel(
                "Export Card Database Data as JSON",
                "",
                "cardDatabase.json",
                "json");

            if (path.Length != 0)
            {
                var serializableData = ConvertToSeralizableData();
                var json = JsonUtility.ToJson(serializableData, true);
                File.WriteAllText(path, json);
                
                Debug.Log("Card Database Json saved.");
            }
        }
        #endif
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
