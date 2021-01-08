using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Cards
{
    [CreateAssetMenu(menuName = "MMO Card Game/Deck", fileName = "Deck", order = 0)]
    public class Deck : ScriptableObject
    {
        public string deckName;
        public List<Card> cards;

        private void Awake()
        {
            cards = new List<Card>();
        }
    }
    
    [System.Serializable]
    public class DeckSerializableData
    {
        public string deckName;
        public List<string> cards;
    }
}